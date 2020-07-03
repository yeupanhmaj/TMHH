using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Commons;
using AdminPortal.Models.Common;

namespace AdminPortal.Services
{
    public class AuditService : BaseService<AuditService>
    {

        public int getTotalRecords(AuditSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return AuditDataLayer.GetInstance().getTotalRecords(connection, _criteria,_userID);
            }
        }

        public List<SearchAuditInfo> getAllAudit(int pageSize, int pageIndex, AuditSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<SearchAuditInfo> ListAudit = AuditDataLayer.GetInstance().getAuditNew(connection, _criteria,_userID);

                return ListAudit;
            }
        }

        public AuditDetailInfo GetAuditByCode(string code, string _userID)
        {
            AuditDetailInfo record = new AuditDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AuditDataLayer.GetInstance().GetAuditByCode(connection, code,_userID);
                if (record == null)
                {
                    return null;
                }

                record.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Audit.ToString();
               
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                record.Employees = new List<AuditEmployeeInfo>();
                record.Employees = AuditDataLayer.GetInstance().GetAuditEmployeesById(connection, record.AuditID.ToString());

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Audit.ToString();
              
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


        public List<QuoteAuditInfo> getListQuoteInfos(int auditID)
        {
            List<QuoteAuditInfo> ret = new List<QuoteAuditInfo>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<int> QuoteIDs = AuditDataLayer.GetInstance().getQuoteOfAudit(connection , auditID);

                foreach (int quoteID in QuoteIDs ){
                    QuoteAuditInfo temp = QuoteDataLayer.GetInstance().GetChoosedQuoteDetails(connection, quoteID);
                    temp.ProposalCodes = QuoteDataLayer.GetInstance().GetListProsalCode(connection, quoteID);
                    temp.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, quoteID);
                    ret.Add(temp);
                }
            }
            return ret;
        }


        public AuditDetailInfo getAuditInfo(int _ID, string _userID)
        {
            AuditDetailInfo record = new AuditDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AuditDataLayer.GetInstance().getAudtiGeneralInfo(connection, _ID,_userID);
                if (record == null)
                {
                    return null;
                }
                record.Quotes = getListQuoteInfos(_ID);

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Audit.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                record.Employees = new List<AuditEmployeeInfo>();
                record.Employees = AuditDataLayer.GetInstance().GetAuditEmployeesById(connection, record.AuditID.ToString());

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Audit.ToString();
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


        public  SearchAuditInfo GetAuditWordByQuoteID(int _quoteID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                SearchAuditInfo ret = AuditDataLayer.GetInstance().getAuditInfoByQuote(connection, _quoteID);
                return ret;
            }
        }

     

        public ActionMessage createAudit(AuditInfo _Audit, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {      
                    
                    AuditDataLayer.GetInstance().InsertAudit(connection, _Audit, _userI);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "123";
                    ret.err.msgString = ex.Message;
                }
            }
            return ret;
        }

        public int createAudit2(AuditInfo _Audit, string _userI)
        {
            int ret = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ret = AuditDataLayer.GetInstance().InsertAudit(connection, _Audit, _userI);
                    if(ret != 1)
                    {
                        foreach (int QuoteID in _Audit.QuoteIDs)
                        {
                            AuditDataLayer.GetInstance().InsertAuditQuote(connection, ret, QuoteID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public ActionMessage editAudit(int id, AuditInfo _Audit, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {           
                try
                {                      
                    AuditDataLayer.GetInstance().UpdateAudit(connection, id, _Audit, _userU);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "123";
                    ret.err.msgString = ex.Message;
                }            
            }
            return ret;
        }

        public ActionMessage EditEmployees(int auditID, List<AuditEmployeeInfo> _items)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    List<AuditEmployeeInfo> currentItems = AuditDataLayer.GetInstance().GetAuditEmployeesById(connection, auditID.ToString());
                    var listDeleteItem = new List<AuditEmployeeInfo>();
                    var listInsertItems = new List<AuditEmployeeInfo>();
                    if (_items == null)
                    {
                        listDeleteItem = currentItems;
                        listInsertItems = new List<AuditEmployeeInfo>();
                    }
                    else
                    {
                        listDeleteItem = currentItems.Except(_items).ToList();
                        listInsertItems = _items.Except(currentItems).ToList();
                    }
                    //add new Items
                    foreach (var item in listInsertItems)
                    {
                        AuditDataLayer.GetInstance().InsertAuditEmployee(connection, auditID, item);
                    }
                    string autoIds = "";
                    //delete Old Items

                    if (listDeleteItem.Count > 0)
                    {
                        foreach (var item in listDeleteItem)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        AuditDataLayer.GetInstance().DeleteAuditEmployees(connection, autoIds);
                    }

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

        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete Items Table 
                    List<AuditEmployeeInfo> currentItems = AuditDataLayer.GetInstance().GetAuditEmployeesById(connection, id.ToString());
                    if (currentItems.Count > 0)
                    {
                        string autoIds = "";
                        foreach (var item in currentItems)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                    //    AuditDataLayer.GetInstance().DeleteAuditEmployees(connection, autoIds);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Audit.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Audit.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete Process

                    List<string> quoteID = AuditDataLayer.GetInstance().GetQuoteByAuditIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "Audit", String.Join(", ", quoteID.ToArray()), _userID);
                    AuditDataLayer.GetInstance().Delete(connection, id);
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
                    if (ids.Length > 0)
                    {
                        //delete Items Table 
                        List<AuditEmployeeInfo> currentItems = AuditDataLayer.GetInstance().GetAuditEmployeesByIds(connection, ids);
                        if (currentItems.Count > 0)
                        {
                            string autoIds = "";
                            foreach (var item in currentItems)
                            {
                                autoIds = autoIds + item.AutoID + ',';
                            }
                            autoIds = autoIds.Remove(autoIds.Length - 1);
                            AuditDataLayer.GetInstance().DeleteAuditEmployees(connection, autoIds);
                        }

                
                        //delete comments
                        CommentSeachCriteria _criteria = new CommentSeachCriteria();
                        _criteria.TableName = TableFile.Audit.ToString();
                        string[] IDsarray = ids.Split(',');
                        foreach (string id in IDsarray)
                        {
                            _criteria.PreferId = id;
                            CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                        }

                        //delete attach files and DB of attach files
                        DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Audit.ToString(), ids);
                        //delete records
                        List<string> quoteID = AuditDataLayer.GetInstance().GetQuoteByAuditIds(connection, ids);
                        QuoteService.GetInstance().deleteProcess(connection, "Audit", String.Join(", ", quoteID.ToArray()), _userID);
                        AuditDataLayer.GetInstance().DeleteMuti(connection, ids);
                        ret.isSuccess = true;
                    }
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



        public List<string> getListAuditCode(string auditCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListAudit = AuditDataLayer.GetInstance().getListAuditCode(connection, auditCode);
                return ListAudit;
            }
        }

        public ActionMessage insertEmployees(int auditID, List<AuditEmployeeInfo> items)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    foreach (var item in items)
                    {
                        AuditDataLayer.GetInstance().InsertAuditEmployee(connection, auditID, item);
                    }

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


        public List<AuditEmployeeInfo> GetAuditDefaultMember()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    List<AuditEmployeeInfo> currentItems = AuditDataLayer.GetInstance().GetAuditDefaultMember(connection);
                    return currentItems;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }
            }
    }
}
