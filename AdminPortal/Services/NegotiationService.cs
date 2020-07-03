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
    public class NegotiationService : BaseService<NegotiationService>
    {

        public int getTotalRecords(NegotiationSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return NegotiationDataLayer.GetInstance().getTotalRecords(connection, _criteria, _userID);
            }
        }

        public List<NegotiationInfo> getAllNegotiation(int pageSize, int pageIndex, NegotiationSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return NegotiationDataLayer.GetInstance().getNegotiation(connection, _criteria, _userID);
            }
        }

        public NegotiationInfo GetNegotiation(int _ID, string _userID)
        {
            NegotiationInfo record = new NegotiationInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = NegotiationDataLayer.GetInstance().getNegotiation(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }
         
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                    record.QuoteID);      
                
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Negotiation.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);
                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Negotiation.ToString();
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
        public NegotiationInfo GetNegotiationByCode(string code)
        {
            NegotiationInfo record = new NegotiationInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = NegotiationDataLayer.GetInstance().GetNegotiationByCode(connection, code);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().GetQuoteItems(connection, record.QuoteID);
                //bidPlanDetail.ListMember = BidPlanDataLayer.GetInstance().GetBidPlanMembers(connection, _ID);

                //negotiationDetail.ListMember = NegotiationDataLayer.GetInstance().GetNegotiationMembers(connection, _ID);
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Negotiation.ToString();
                documentCriteria.PreferId = record.QuoteID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);
                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Negotiation.ToString();
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

       


        public ActionMessage createNegotiation(NegotiationInfo _Negotiation, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {         
                    ret.id = NegotiationDataLayer.GetInstance().InsertNegotiation(connection, _Negotiation, _userI);
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

        public int createNegotiation2(NegotiationInfo _Negotiation, string _userI)
        {
            int ret = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ret = NegotiationDataLayer.GetInstance().InsertNegotiation(connection, _Negotiation, _userI);
                }
                catch (Exception ex)
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public ActionMessage editNegotiation(int id, NegotiationInfo _Negotiation, string _userU, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                var chkNegotiationInfo = NegotiationDataLayer.GetInstance().getNegotiation(connection, id, _userID);
                if (chkNegotiationInfo != null)
                {
                    try
                    {
                        NegotiationDataLayer.GetInstance().UpdateNegotiation(connection, id, _Negotiation, _userU);
                        ret.isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "123";
                        ret.err.msgString = "123";
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
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Negotiation.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Negotiation.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete record
                    List<string> quoteID = NegotiationDataLayer.GetInstance().GetQuoteByNegotiationIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "Negotiation", String.Join(", ", quoteID.ToArray()), _userID);
                    NegotiationDataLayer.GetInstance().Delete(connection, id);
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
                    _criteria.TableName = TableFile.Negotiation.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Negotiation.ToString(), ids);
                    //delete records
                    List<string> quoteID = NegotiationDataLayer.GetInstance().GetQuoteByNegotiationIds(connection, ids);
                    QuoteService.GetInstance().deleteProcess(connection, "Negotiation", String.Join(", ", quoteID.ToArray()), _userID);
                    NegotiationDataLayer.GetInstance().DeleteMuti(connection, ids);
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



        public List<NegotiationLstcb> GetListNegotiationByCode(string code)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<NegotiationLstcb> ListNegotiation = NegotiationDataLayer.GetInstance().GetListNegotiationByCode(connection, code);
                return ListNegotiation;
            }
        }


     

        public NegotiationPrintModel GetNegotiationPrintModel(int _ID, string _userID)
        {
            NegotiationPrintModel record = new NegotiationPrintModel();
            NegotiationInfo info = new NegotiationInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            SearchAuditInfo auditInfo = new SearchAuditInfo();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                info = NegotiationDataLayer.GetInstance().getNegotiation(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                CustomerInfo cusInfo = CustomerDataLayer.GetInstance().getCustomer(connection, info.CustomerID);

                auditInfo = AuditDataLayer.GetInstance().getAuditInfoByQuote(connection,info.QuoteID);

                BidPlanPrint  bidPlanInfo = BidPlanDataLayer.GetInstance().getBidPlanInfoByQuote(connection, info.QuoteID);


                record.ASide = "BỆNH VIỆN TRUYỀN MÁU HUYẾT HỌC";
                record.BSide = info.CustomerName;

                record.ALocation = info.Location;
                record.APhone = info.Phone;
                record.AFax = info.Fax;
                record.ABankID = info.BankID;
                record.ATaxCode = info.TaxCode;
                record.ARepresent = info.Represent;
                record.APosition = info.Position;

                record.BLocation = cusInfo.Address;
                record.BPhone = cusInfo.Phone;
                record.BFax = cusInfo.Fax;
                record.BBankID = cusInfo.BankNumber + " " + cusInfo.BankName;
                record.BTaxCode = cusInfo.TaxCode;
                record.BRepresent = cusInfo.Surrogate;
                record.BPosition = cusInfo.Position;

                record.DateIn = info.DateIn;
                record.Term = info.Term;
                record.NegotiationCode = info.NegotiationCode;
                record.AuditTime = auditInfo.InTime;
                record.AuditCode = auditInfo.AuditCode;
                record.VATNumber = info.VATNumber;
                record.IsVAT = info.IsVAT;
                record.QuoteTotalCost = info.QuoteTotalCost;
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                    record.QuoteID);
                record.QuoteID = info.QuoteID;
                record.QuoteCode = info.QuoteCode;

                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                    record.QuoteID);


                record.BidExpirated = info.BidExpirated;

                record.BidExpiratedUnit = info.BidExpiratedUnit;

                record.BidType = info.BidType;

                return record;
            }
        }

    }
}
