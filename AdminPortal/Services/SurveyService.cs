using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Services
{
    public class SurveyService : BaseService<SurveyService>
    {

        public int GetTotalRecords(SurveySeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return SurveyDataLayer.GetInstance().getTotalRecords(connection, _criteria, _userID);
            }
        }

        public List <SurveyInfo> GetAllSurvey(int pageSize,int pageIndex, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<SurveyInfo> ListSurvey = SurveyDataLayer.GetInstance().GetAllSurvey(connection, _userID);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListSurvey.Count) return new List<SurveyInfo>();
                if (max >= ListSurvey.Count) pageSize = ListSurvey.Count - min;
                if (pageSize <= 0) return new List<SurveyInfo>();
                return ListSurvey.GetRange(min, pageSize);
            }
        }

        public List<SurveyInfo> GetAllSurveyWithCondition( SurveySeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return SurveyDataLayer.GetInstance().getSurvey(connection, _criteria, _userID);
            }
        }
        public SurveyDetailInfo GetDetailSurvey(int _ID, string _userID)
        {
            SurveyDetailInfo record = new SurveyDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = SurveyDataLayer.GetInstance().getSurveyDetail(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Survey.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

             
                record.Items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, _ID);
                record.SurveyItems = ProposalDataLayer.GetInstance().GetSurveyItem(connection, _ID);
                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Survey.ToString();
                commentCriteria.PreferId = _ID.ToString();
                record.ListComment = CommentService.GetInstance().getComment(commentCriteria);
                foreach (var item in record.ListComment)
                {
                    DocumentSeachCriteria documentCriteria2 = new DocumentSeachCriteria();
                    documentCriteria2.TableName = TableFile.Comment.ToString();
                    documentCriteria2.PreferId = item.AutoID.ToString();
                    item.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria2);
                }
                return record;
            }
        }

        public List<SurveyInfo> getSurvey(SurveySeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SurveyInfo> ListSurvey = SurveyDataLayer.GetInstance().getSurvey(connection, _criteria, _userID);
                return ListSurvey;
            }
        }


        public ActionMessage createSurvey(SurveyInfo _Survey, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail2(connection, _Survey.ProposalCode);
                    if (ProposalInfo != null)
                    {
                        _Survey.ProposalID = ProposalInfo.ProposalID;
                    }
                    else _Survey.ProposalID = 0;
                    ret.id = SurveyDataLayer.GetInstance().InsertSurvey(connection, _Survey, _userI);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString = ex.Message;
                }
            }
            return ret;
        }

        public async Task<ActionMessage> createSurvey2(SurveyInfo _Survey, string _userI, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Insert";
                    info.Description = "ngày : " + _Survey.InTime + " code : KS-" + _Survey.ProposalCode
                     + " trạng thái : " + _Survey.Status;
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = _userI;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);
                    insetId = SurveyDataLayer.GetInstance().InsertSurvey(connection, _Survey, _userI);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (insetId > -1)
            {
                ret.id = insetId;
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        foreach (ItemSurveyInfo item in _Survey.SurveyItems)
                        {
                            ProposalDataLayer.GetInstance().InsertSurveyItem(connection, item, insetId, _userI);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Survey.ToString();
                    documentInfo.PreferId = insetId.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    await FilesHelpers.UploadFile(TableFile.Survey.ToString(), insetId.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                }
                ret.isSuccess = true;
            }
            else
            {
                ret.isSuccess = false;
                ret.err.msgCode = "lỗi thêm phiếu khảo sát";
                ret.err.msgString = "lỗi thêm phiếu khảo sát";
            }
            return ret;
        }

        public async Task<ActionMessage> editSurvey(int id, SurveyInfo _Survey, string _userU , [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                try
                {

                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Edit";
                    info.Description = "ngày : " + _Survey.InTime + " code : " + _Survey.SurveyCode
                     + " trạng thái : " + _Survey.Status;
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = _userU;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);

                    SurveyDataLayer.GetInstance().UpdateSurvey(connection, id, _Survey, _userU);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Survey.ListDocument, TableFile.Survey.ToString(), id);

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ProposalDataLayer.GetInstance().DeleteSurveyItem(connection, id, _userU);
                    foreach (ItemSurveyInfo item in _Survey.SurveyItems)
                    {
                        ProposalDataLayer.GetInstance().InsertSurveyItem(connection, item, id, _userU);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            foreach (var item in files)
            {
                DocumentInfo documentInfo = new DocumentInfo();
                documentInfo.TableName = TableFile.Survey.ToString();
                documentInfo.PreferId = id.ToString();
                documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                documentInfo.FileName = item.FileName;
                documentInfo.Length = item.Length.ToString();
                documentInfo.Type = item.ContentType;
                ret = await FilesHelpers.UploadFile(TableFile.Survey.ToString(), id.ToString(), item, documentInfo.Link);
                DocumentService.GetInstance().InsertDocument(documentInfo, _userU.ToString());
            }
            ret.isSuccess = true;
            return ret;
        }

        public ActionMessage DeleteSurvey(int id, string user, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SurveyDetailInfo temp = GetDetailSurvey(id,_userID);

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Delete";

                    if (temp != null)
                    {
                        info.Description = "code : " + temp.SurveyCode + " id : " + id;
                    }
                    else
                    {
                        info.Description = " id : " + id;
                    }
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = user;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);


                    SurveyDataLayer.GetInstance().Delete(connection, id);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString =  ex.ToString();
                }
            }
            return ret;
        }
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Survey.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Survey.ToString(), ids);
                    //delete records
                    SurveyDataLayer.GetInstance().DeleteMuti(connection, ids);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString = ex.Message;
                }
            }
            return ret;
        }
    }
}
