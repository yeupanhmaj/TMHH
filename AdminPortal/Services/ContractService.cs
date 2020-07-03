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
    public class ContractService : BaseService<ContractService>
    {

        public int getTotalRecords(ContractSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ContractDataLayer.GetInstance().getTotalRecords(connection, _criteria,_userID);
            }
        }

 

        public List<ContractInfo> getAllContract(int pageSize, int pageIndex, ContractSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return ContractDataLayer.GetInstance().getContract(connection, _criteria, _userID);
                
            }
        }

        public ContractInfo getContract(int _ID, string _userID)
        {
            ContractInfo record = new ContractInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = ContractDataLayer.GetInstance().getContract(connection, _ID, _userID);
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                    record.QuoteID);
                //bidPlanDetail.ListMember = BidPlanDataLayer.GetInstance().GetBidPlanMembers(connection, _ID);
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Contract.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Contract.ToString();
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

        public NewContractInfo getContractNew(int _ID, string _userID)
        {
            NewContractInfo record = new NewContractInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = ContractDataLayer.GetInstance().getContractNew(connection, _ID);
                record.negotiation = NegotiationService.GetInstance().GetNegotiation(record.NegotiationID,_userID);
              
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Contract.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Contract.ToString();
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


        public ActionMessage createContract(ContractInfo _Contract, string _userI, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    var DecisionInfo = DecisionDataLayer.GetInstance().GetDecisionByCode(connection, _Contract.DecisionCode,_userID);
                    if (DecisionInfo != null)
                    {
                        _Contract.DecisionID = DecisionInfo.DecisionID;
                        //Bỏ proposalID từ Dicision
                        //_Contract.ProposalID = DecisionInfo.ProposalID;
                        var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail(connection, _Contract.ProposalID, "");
                        if (ProposalInfo != null)
                        {
                            _Contract.ProposalCode = ProposalInfo.ProposalCode;
                        }
                    }
                    else _Contract.DecisionID = _Contract.ProposalID = 0;
                    ret.id = ContractDataLayer.GetInstance().InsertContract(connection, _Contract, _userI);
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

        public int createContract2(ContractInfo _Contract, string _userI, string _userID)
        {
            int ret = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    _Contract.NegotiationID = NegotiationDataLayer.GetInstance().getNegotiationIDbyQuoteID(connection,_Contract.QuoteID, _userID);
                
                    //var DecisionInfo = DecisionDataLayer.GetInstance().GetDecisionByCode(connection, _Contract.DecisionCode);
                    //if (DecisionInfo != null)
                    //{
                    //    _Contract.DecisionID = DecisionInfo.DecisionID;
                    //    //_Contract.ProposalID = DecisionInfo.ProposalID;
                    //    var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail(connection, _Contract.ProposalID);
                    //    if (ProposalInfo != null)
                    //    {
                    //        _Contract.ProposalCode = ProposalInfo.ProposalCode;
                    //    }
                    //}
                    //else _Contract.DecisionID = _Contract.ProposalID = 0;
                    ret = ContractDataLayer.GetInstance().InsertContract(connection, _Contract, _userI);
                    // ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret = -1;
                }
            }
            return ret;
        }

        public ActionMessage editContract(int id, ContractInfo _Contract, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ContractDataLayer.GetInstance().UpdateContract(connection, id, _Contract, _userU);
                    ret.isSuccess = true;
                }
                //{
                //ContractSeachCriteria _criteria = new ContractSeachCriteria();
                //_criteria.ContractID = id.ToString();
                //var chkContractInfo = ContractDataLayer.GetInstance().getContract(connection, id);
                //if (chkContractInfo != null)
                //{
                //    try
                //    {
                //        var DecisionInfo = DecisionDataLayer.GetInstance().GetDecisionByCode(connection, _Contract.DecisionCode);
                //        if (DecisionInfo != null)
                //        {
                //            _Contract.DecisionID = DecisionInfo.DecisionID;
                //            //_Contract.ProposalID = DecisionInfo.ProposalID;
                //            var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail(connection, _Contract.ProposalID);
                //            if (ProposalInfo != null)
                //            {
                //                _Contract.ProposalCode = ProposalInfo.ProposalCode;
                //            }
                //        }
                //        else _Contract.DecisionID = _Contract.ProposalID = 0;
                //        ContractDataLayer.GetInstance().UpdateContract(connection, id, _Contract, _userU);
                //        ret.isSuccess = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        ret.isSuccess = false;
                //        ret.err.msgCode = "Internal Error";
                //        ret.err.msgString =  ex.ToString();
                //    }
                //}
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString =  ex.ToString();
                }              
            }
            return ret;
        }

        public ActionMessage deleteContract(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Contract.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Contract.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);


                    //delete record
                    List<string> quoteID = ContractDataLayer.GetInstance().GetQuoteByContractIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "Contract", String.Join(", ", quoteID.ToArray()), _userID);
                    ContractDataLayer.GetInstance().Delete(connection, id);
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

        public ActionMessage deleteMuti(string ids, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {       

                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Contract.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Contract.ToString(), ids);
                    //delete records
                    List<string> quoteID = ContractDataLayer.GetInstance().GetQuoteByContractIds(connection, ids);
                    QuoteService.GetInstance().deleteProcess(connection, "Contract", String.Join(", ", quoteID.ToArray()), _userID);
                    ContractDataLayer.GetInstance().DeleteMuti(connection, ids);
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
    

        //public ActionMessage deleteContracts(string ids)
        //{
        //    ActionMessage ret = new ActionMessage();

        //    SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
        //    using (SqlConnection connection = sqlConnection.GetConnection())
        //    {
        //        try
        //        {
        //            ContractDataLayer.GetInstance().DeleteContract(connection, ids);
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

        public List<string> getListContractCode(string contractCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListContract = ContractDataLayer.GetInstance().getListContractCode(connection, contractCode);
                return ListContract;
            }
        }

        public ContractInfo GetContractByCode(string code, string _userID)
        {
            ContractInfo record = new ContractInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = ContractDataLayer.GetInstance().GeContractByCode(connection, code,_userID);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().GetQuoteItems(connection, record.QuoteID);
               
                QuoteInfo quote = QuoteDataLayer.GetInstance().getQuote(connection, record.QuoteID,_userID);
                if (quote.TotalCost > 30000000)
                {
                    record.DeliveryReceiptType = 2;
                }
                else
                {
                    record.DeliveryReceiptType = 1;
                }
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Contract.ToString();
                documentCriteria.PreferId = record.QuoteID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Contract.ToString();
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

        public ContractPrintModel GetNegotiationPrintModel(int contractID, string _userID)
        {
            ContractPrintModel record = new ContractPrintModel();
            NegotiationInfo info = new NegotiationInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            NewContractInfo contractInfo = getContractNew(contractID, _userID);

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                info = NegotiationDataLayer.GetInstance().getNegotiation(connection, contractInfo.NegotiationID,_userID);
                if (record == null || info == null)
                {
                    return null;
                }

                CustomerInfo cusInfo = CustomerDataLayer.GetInstance().getCustomer(connection, info.CustomerID);

                record.ContractCode = contractInfo.ContractCode;
                record.DateIn = contractInfo.DateIn;
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
                record.NegotiationTime = info.DateIn;
                record.Term = info.Term;
                record.NegotiationCode = info.NegotiationCode;            
                record.VATNumber = info.VATNumber;
                record.IsVAT = info.IsVAT;
                record.QuoteTotalCost = info.QuoteTotalCost;
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                    record.QuoteID);
                record.QuoteID = info.QuoteID;
                record.QuoteCode = info.QuoteCode;


                record.BidType = info.BidType;
                record.BidExpirated = info.BidExpirated;
                record.BidExpiratedUnit = info.BidExpiratedUnit;
      
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection,
                            record.QuoteID);

                return record;
            }
        }

        public List<ContractSelectItem> GetContractSelectItem(string code,string _userID)
        {
            List<ContractSelectItem> record = new List<ContractSelectItem>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = ContractDataLayer.GetInstance().GetContractSelectItem(connection, code,_userID);
          

      
                return record;
            }
        }

    }
}
