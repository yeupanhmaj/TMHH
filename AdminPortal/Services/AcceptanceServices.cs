using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class AcceptanceServices : BaseService<AcceptanceServices>
    {
        public List<AcceptanceInfo> GetList(AcceptanceCriteria conditions,string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<AcceptanceInfo> ListAcceptanceInfo = AcceptanceDataLayer.GetInstance().Getlist(connection, conditions,_userID);
                return ListAcceptanceInfo;
            }
        }

        public int getTotalRecords(AcceptanceCriteria conditions, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return AcceptanceDataLayer.GetInstance().GetTotalRecords(connection, conditions,_userID);
            }
        }

        public AcceptanceInfo GetDetail(int id, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            AcceptanceInfo record = new AcceptanceInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {              
                record =  AcceptanceDataLayer.GetInstance().GetDetail(connection, id,_userID);
                if (record == null)
                {
                    return null;
                }
                record.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Acceptance.ToString();
                documentCriteria.PreferId = id.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                record.Items = new List<DeliveryReceiptItemInfoNew>();
                record.Items =   DeliveryReceiptDataLayer.GetInstance().getSelectedItems(connection, record.DeliveryReceiptID,_userID);
                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Acceptance.ToString();
                commentCriteria.PreferId = id.ToString();
                record.ListComment = CommentService.GetInstance().getComment(commentCriteria);
                foreach (var item in record.ListComment)
                {
                    DocumentSeachCriteria documentCriteria2 = new DocumentSeachCriteria();
                    documentCriteria2.TableName = TableFile.Comment.ToString();
                    documentCriteria2.PreferId = item.AutoID.ToString();
                    item.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria2);
                }
            }
            return record;
        }

        public async Task<ActionMessage> Create(AcceptanceInfo obj, [FromForm] List<IFormFile> files, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    insetId = AcceptanceDataLayer.GetInstance().Create(connection, obj, _userI);             
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
                        foreach (DeliveryReceiptItemInfoNew item in obj.Items)
                        {
                            ProposalDataLayer.GetInstance().UpdateItemAcceptance(connection, item, _userI);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if(files !=null)
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Acceptance.ToString();
                    documentInfo.PreferId = insetId.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Acceptance.ToString(), insetId.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                }
                ret.isSuccess = true;
            }
            else
            {
                ret.isSuccess = false;
                ret.err.msgCode = "lỗi thêm phiếu nghiệm thu";
                ret.err.msgString = "lỗi thêm phiếu nghiệm thu";           
            }
            return ret;
        }
        public async Task<ActionMessage> Update(AcceptanceInfo obj, [FromForm] List<IFormFile> files, string _userI)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    AcceptanceDataLayer.GetInstance().Update(connection, obj, _userI);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            DocumentService.GetInstance().DeleteDocumentsNotExitsInList(obj.ListDocument, TableFile.Acceptance.ToString(), obj.AcceptanceID);

                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        foreach (DeliveryReceiptItemInfoNew item in obj.Items)
                        {
                            ProposalDataLayer.GetInstance().UpdateItemAcceptance(connection, item, _userI);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (files != null)
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Acceptance.ToString();
                    documentInfo.PreferId = obj.AcceptanceID.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Acceptance.ToString(), obj.AcceptanceID.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                }
                ret.isSuccess = true;
            
         
            return ret;
        }

        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Acceptance.ToString(), id);

                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Acceptance.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete record
                    AcceptanceDataLayer.GetInstance().Delete(connection, id);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString = ex.ToString();
                }
            }
            return ret;
        }
        public ActionMessage DeleteMuti(string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {

                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Acceptance.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Acceptance.ToString(), ids);



                    //delete records
                    AcceptanceDataLayer.GetInstance().DeleteMuti(connection, ids);
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
