using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Commons;
using AdminPortal.Models.Common;

namespace AdminPortal.Services
{
    public class DocumentService : BaseService<DocumentService>
    {

        public List<DocumentInfo> GetDocument(DocumentSeachCriteria _criteria)
        {
            if (!string.IsNullOrEmpty(_criteria.PreferId))
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    List<DocumentInfo> ListDocument = DocumentDataLayer.GetInstance().getDocument(connection, _criteria);
                    return ListDocument;
                }
            }
            else return null;
        }

        public List<DocumentInfo> GetAllDocumentsByProposalID(int _proposalID)
        {
            List<DocumentInfo> ret = new List<DocumentInfo>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DocumentInfo> ListDocument = DocumentDataLayer.GetInstance().GetAllDocumentsByProposalID(connection, _proposalID);

            }
            return ret; 
        }




        public void DeleteDocumentsNotExitsInList(List<DocumentInfo> listDocument, string tableName, int preferId)
        { 
            DocumentSeachCriteria _cri = new DocumentSeachCriteria();
            _cri.TableName = tableName;
            _cri.PreferId = preferId.ToString();
            List<DocumentInfo> currentDocument = GetDocument(_cri);
            List<DocumentInfo> listDelete = new List<DocumentInfo>();
            if (listDocument == null)
            {
                listDelete = currentDocument;
            }
            else
            {
                listDelete = currentDocument.Where(el2 => !listDocument.Any(el1 => el1.AutoID == el2.AutoID)).ToList();
            }
         
            //delete data in tables
            if (listDelete.Count > 0) { 
                string deleteIds = "";
                List<string> filesName = new List<string>();
                foreach (DocumentInfo document in listDelete)
                {
                    deleteIds += document.AutoID + ",";
                }
                deleteIds = deleteIds.Remove(deleteIds.Length - 1);
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    DocumentDataLayer.GetInstance().DeleteDocuments(connection, deleteIds);
                }
                //delete file physical
                FilesHelpers.DeleteFiles(tableName, preferId.ToString(), filesName);
            }  
        }


        public ActionMessage InsertDocument(DocumentInfo documentInfo, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            if (!(string.IsNullOrEmpty(documentInfo.TableName)))
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        DocumentDataLayer.GetInstance().InsertDocument(connection, documentInfo, _userI);
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
            else 
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Thiếu tên bảng";
                return ret;
            }
        }

        public ActionMessage DeleteDocumentByFeature(string feature, int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    FilesHelpers.DeleteFolder(feature, id.ToString());
                    DocumentDataLayer.GetInstance().DeleteDocumentByFeatureAndID(connection, feature, id);
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

        public ActionMessage DeleteDocumentsByFeature(string feature, string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    string[] IDsarray = ids.Split(',');
                    foreach(string id in IDsarray)
                    {
                        FilesHelpers.DeleteFolder(feature, id);
                        DocumentDataLayer.GetInstance().DeleteDocumentByFeatureAndID(connection, feature, Int32.Parse(id));
                    }
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

        public ActionMessage DeleteDocument(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                   DocumentInfo info = DocumentDataLayer.GetInstance().GetDocumentById(connection,id);
                    if(info.PreferId != null && info.TableName != "")
                    {
                        FilesHelpers.DeleteFile(info.TableName, info.PreferId, info.FileName);
                    }
                    DocumentDataLayer.GetInstance().DeleteDocument(connection, id);
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

        public ActionMessage DeleteDocuments(string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    List<DocumentInfo> infos = DocumentDataLayer.GetInstance().GetDocumentsByIds(connection, ids);
                    if(infos.Count > 0)
                    {
                        foreach (DocumentInfo document in infos){
                            FilesHelpers.DeleteFile(document.TableName, document.PreferId, document.FileName);
                        }
                    }
                    DocumentDataLayer.GetInstance().DeleteDocuments(connection, ids);
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
