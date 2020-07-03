using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;

namespace AdminPortal.Services
{
    public class DecisionService : BaseService<DecisionService>
    {

        public int getTotalRecords(DecisionSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return DecisionDataLayer.GetInstance().getTotalRecords(connection, _criteria,_userID);
            }
        }

        public List <DecisionInfo> getAllDecision(int pageSize,int pageIndex, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return DecisionDataLayer.GetInstance().GetAllDecision(connection,_userID);           
            }
        }

        public List<DecisionInfo> getAllDecision(int pageSize, int pageIndex, DecisionSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return  DecisionDataLayer.GetInstance().getDecision(connection, _criteria, _userID);
           
            }
        }

        public DecisionInfo GetDecision(int _ID, string _userID)
        {
            DecisionInfo record = new DecisionInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = DecisionDataLayer.GetInstance().getDecision(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, record.QuoteID);
                record.DepartmentNames = DepartmentDataLayer.GetInstance().GetDepartmentNamesFromQuote(connection,record.QuoteID);

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Decision.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);
                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Decision.ToString();
                commentCriteria.PreferId = _ID.ToString();
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

        public List<string> GetListDecisionByCode(string code, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListAudit = DecisionDataLayer.GetInstance().GetListDecisionByCode(connection, code,_userID);
                return ListAudit;
            }
        }
        public List<DecisionInfo> getDecision(DecisionSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DecisionInfo> ListDecision = DecisionDataLayer.GetInstance().getDecision(connection, _criteria, _userID);
                return ListDecision;
            }
        }

        public ActionMessage createDecision(DecisionInfo _Decision, string _userI, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    var quoteInfo = QuoteDataLayer.GetInstance().getQuoteByCode(connection, _Decision.QuoteCode, _userID);
                    if (quoteInfo != null)
                    {
                        _Decision.QuoteID = quoteInfo.QuoteID;
            
                    }
                    else _Decision.QuoteID/* = _Decision.ProposalID*/ = 0;
                    ret.id = DecisionDataLayer.GetInstance().InsertDecision(connection, _Decision, _userI);
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

        public int createDecision2(DecisionInfo _Decision, string _userI)
        {
            int ret = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
     
                    ret = DecisionDataLayer.GetInstance().InsertDecision(connection, _Decision, _userI);
    
                }
                catch (Exception ex)
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public ActionMessage editDecision(int id, DecisionInfo _Decision, string _userU, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                var chkDecisionInfo = DecisionDataLayer.GetInstance().getDecision(connection, id, _userID);
                if (chkDecisionInfo != null)
                {
                    try
                    {
       
                        DecisionDataLayer.GetInstance().UpdateDecision(connection, id, _Decision, _userU);
                        ret.isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString =  ex.ToString();
                    }
                }
            }
            return ret;
        }

        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Decision.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Decision.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);


                    //delete record
                    List<string> quoteID = DecisionDataLayer.GetInstance().GetQuoteByDecisionIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "Decision", String.Join(", ", quoteID.ToArray()), _userID);
                    DecisionDataLayer.GetInstance().Delete(connection, id);
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

        public ActionMessage DeleteMuti(string ids, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {

                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Decision.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Decision.ToString(), ids);
                    //delete records
                    List<string> quoteID = DecisionDataLayer.GetInstance().GetQuoteByDecisionIds(connection, ids);
                    QuoteService.GetInstance().deleteProcess(connection, "Decision", String.Join(", ", quoteID.ToArray()), _userID);
                    DecisionDataLayer.GetInstance().DeleteMuti(connection, ids);
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

        //public ActionMessage deleteDecisions(string ids)
        //{
        //    ActionMessage ret = new ActionMessage();

        //    SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
        //    using (SqlConnection connection = sqlConnection.GetConnection())
        //    {
        //        try
        //        {
        //            DecisionDataLayer.GetInstance().DeleteDecision(connection, ids);
        //            ret.isSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ret.isSuccess = false;
        //            ret.err.msgCode = "Internal Error";
        //            ret.err.msgString =  ex.ToString();
        //        }
        //    }
        //    return ret;
        //}

        public List<string> getListDecisionCode(string decisionCode, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListDecision = DecisionDataLayer.GetInstance().getListDecisionCode(connection, decisionCode,_userID);
                return ListDecision;
            }
        }
        public DecisionInfo GetDecisionByCode(string code, string _userID)
        {
            DecisionInfo record = new DecisionInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = DecisionDataLayer.GetInstance().GetDecisionByCode(connection, code,_userID);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().GetQuoteItems(connection, record.QuoteID);
            
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Decision.ToString();
                documentCriteria.PreferId = record.QuoteID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Decision.ToString();
                commentCriteria.PreferId = record.QuoteID.ToString();
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

        

    }
}
