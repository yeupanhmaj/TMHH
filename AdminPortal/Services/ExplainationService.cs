using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AdminPortal.Commons;

namespace AdminPortal.Services
{
    public class ExplanationService : BaseService<ExplanationService>
    {


        public int getTotalRecords(ExplanationSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ExplanationDataLayer.GetInstance().getTotalRecords(connection, _criteria, _userID);
            }
        }

        public List <ExplanationInfo> getAllExplanation(int pageSize,int pageIndex, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<ExplanationInfo> ListExplanation = ExplanationDataLayer.GetInstance().GetAllExplanation(connection, _userID);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListExplanation.Count) return new List<ExplanationInfo>();
                if (max >= ListExplanation.Count) pageSize = ListExplanation.Count - min;
                if (pageSize <= 0) return new List<ExplanationInfo>();
                return ListExplanation.GetRange(min, pageSize);
            }
        }

        public List<ExplanationInfo> getAllExplanationWwithCondition(ExplanationSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return ExplanationDataLayer.GetInstance().getExplanation(connection, _criteria,_userID);
       
            }
        }

        public ExplanationInfo getExplanationbyId(int _ID, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ExplanationInfo> ListExplanation = ExplanationDataLayer.GetInstance().GetAllExplanation(connection, _userID);
                ExplanationInfo findExplanation = ListExplanation.Where(i => i.ExplanationID == _ID).First();
                return findExplanation;
            }
        }

        public ExplanationDetailInfo getDetailExplanation(int _ID, string _userID)
        {
            ExplanationDetailInfo record = new ExplanationDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = ExplanationDataLayer.GetInstance().getExplanationDetail(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Explanation.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                record.Items = new List<ItemPropsalInfo>();
                record.Items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, record.ProposalID);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Explanation.ToString();
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

        public List<ExplanationInfo> getExplanation(ExplanationSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ExplanationInfo> ListExplanation = ExplanationDataLayer.GetInstance().getExplanation(connection, _criteria, _userID);
                return ListExplanation;
            }
        }


        public ActionMessage createExplanation(ExplanationInfo _Explanation, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ret.id = ExplanationDataLayer.GetInstance().InsertExplanation(connection, _Explanation, _userI);
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

        public async Task<ActionMessage>  CreateExplanation2(ExplanationInfo _Explanation, string _userI, [FromForm] List<IFormFile> files)
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
                    info.Description = "ngày : " + _Explanation.InTime + " code : GTMS-" + _Explanation.ProposalCode
                     + " trạng thái : " + _Explanation.Status;
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = _userI;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);
                    insetId = ExplanationDataLayer.GetInstance().InsertExplanation(connection, _Explanation, _userI);
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
                        foreach (ItemPropsalInfo item in _Explanation.Items)
                        {
                            ProposalDataLayer.GetInstance().UpdateItemExplanation(connection, item, _userI);
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
                    documentInfo.TableName = TableFile.Explanation.ToString();
                    documentInfo.PreferId = insetId.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                     await FilesHelpers.UploadFile(TableFile.Explanation.ToString(), insetId.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                }
                ret.isSuccess = true;
            }
            else
            {
                ret.isSuccess = false;
                ret.err.msgCode = "lỗi thêm phiếu giải trình";
                ret.err.msgString = "lỗi thêm phiếu giải trình";
            }
            return ret;
        }

        public async Task<ActionMessage> EditExplanation(int id, ExplanationInfo _Explanation, string _userU, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

          

            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                try
                {
                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Edit";
                    info.Description = "ngày : " + _Explanation.InTime + " code : " + _Explanation.ExplanationCode
                     + " trạng thái : " + _Explanation.Status;
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = _userU;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);
                    ExplanationDataLayer.GetInstance().UpdateExplanation(connection, id, _Explanation, _userU);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Explanation.ListDocument, TableFile.Explanation.ToString(), id);

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    foreach (ItemPropsalInfo item in _Explanation.Items)
                    {
                        ProposalDataLayer.GetInstance().UpdateItemExplanation(connection, item, _userU);
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
                documentInfo.TableName = TableFile.Explanation.ToString();
                documentInfo.PreferId = id.ToString();
                documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                documentInfo.FileName = item.FileName;
                documentInfo.Length = item.Length.ToString();
                documentInfo.Type = item.ContentType;
                ret = await FilesHelpers.UploadFile(TableFile.Explanation.ToString(), id.ToString(), item, documentInfo.Link);
                DocumentService.GetInstance().InsertDocument(documentInfo, _userU.ToString());
            }
            ret.isSuccess = true;
            return ret;
    
        }

        public ActionMessage DeleteExplanation(int id, string user, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            ExplanationDetailInfo temp = getDetailExplanation(id,_userID);

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Delete";

                    if (temp != null)
                    {
                        info.Description = "code : " + temp.ExplanationCode + " id : " + id;
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

                    ExplanationDataLayer.GetInstance().Delete(connection, id);
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
                    _criteria.TableName = TableFile.Explanation.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Explanation.ToString(), ids);
                    //delete records
                    ExplanationDataLayer.GetInstance().DeleteMuti(connection, ids);
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
