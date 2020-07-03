using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;
using AdminPortal.Commons;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.IO;

using OfficeOpenXml;

namespace AdminPortal.Services
{
    public class QuoteService : BaseService<QuoteService>
    {
        public int getTotalRecords(QuoteSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return QuoteDataLayer.GetInstance().getTotalRecords(connection, _criteria, _userID);
            }
        }

        public List<QuoteInfo> getAllQuote(int pageSize, int pageIndex, QuoteSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
               return  QuoteDataLayer.GetInstance().GetListQuote(connection, _criteria, _userID);
            }
        }

        public QuoteInfo getQuote(int _ID, string _userID)
        {
            QuoteInfo record = new QuoteInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = QuoteDataLayer.GetInstance().getQuote(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                record.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Quote.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Quote.ToString();
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

        public QuoteInfo GetQuoteDetailsBycode(string code, string _userID)
        {
            QuoteInfo record = new QuoteInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = QuoteDataLayer.GetInstance().getQuoteByCode(connection, code, _userID);
                if (record == null)
                {
                    return null;
                }

                record.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Quote.ToString();
                documentCriteria.PreferId = record.QuoteID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().GetQuoteItems(connection, record.QuoteID);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Quote.ToString();
                commentCriteria.PreferId = record.QuoteID.ToString();
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

        public List<QuoteInfo> getQuote(QuoteSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<QuoteInfo> ListQuote = QuoteDataLayer.GetInstance().GetListQuote(connection, _criteria, _userID);
                return ListQuote;
            }
        }


        public ActionMessage createQuote(QuoteInfo _Quote, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                /* try
                 {
                     var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail2(connection, _Quote.ProposalCode);
                     if (ProposalInfo != null)
                     {
                         _Quote.ProposalID = ProposalInfo.ProposalID;
                     }
                     else _Quote.ProposalID = 0;
                     ret.id = QuoteDataLayer.GetInstance().InsertQuote(connection, _Quote, _userI);
                     ret.isSuccess = true;
                 }
                 catch (Exception ex)
                 {
                     ret.isSuccess = false;
                     ret.err.msgCode = "Internal Error";
                     ret.err.msgString = ex.Message;
                 }*/
            }
            return ret;
        }

        public async Task<ActionMessage> CreateQuoteWithAttachFIles(QuoteInfo _Quote, string _userI, IList<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            /* int insetId = -1;
             SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
             using (SqlConnection connection = sqlConnection.GetConnection())
             {
                 try
                 {
                     insetId = QuoteDataLayer.GetInstance().InsertQuote(connection, _Quote, _userI);
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
                         foreach (ItemInfo item in _Quote.Items)
                         {
                             QuoteDataLayer.GetInstance().UpdateItemDetails(connection, item, _userI , insetId);
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
                     documentInfo.TableName = TableFile.Quote.ToString();
                     documentInfo.PreferId = insetId.ToString();
                     documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                     documentInfo.FileName = item.FileName;
                     documentInfo.Length = item.Length.ToString();
                     documentInfo.Type = item.ContentType;
                     ret = await FilesHelpers.UploadFile(TableFile.Quote.ToString(), insetId.ToString(), item, documentInfo.Link);
                     DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                 }
                 ret.isSuccess = true;
             }
             else
             {
                 ret.isSuccess = false;
                 ret.err.msgCode = "lỗi thêm phiếu báo giá";
                 ret.err.msgString = "lỗi thêm phiếu báo giá";
             }*/
            return ret;

        }

        public async Task<ActionMessage> EditQuote(int id, QuoteInfo _Quote, string _userU, IList<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();

            /*SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                     QuoteDataLayer.GetInstance().UpdateQuote(connection, _Quote.QuoteID, _Quote, _userU);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            DocumentService.GetInstance().GetInstance().DeleteDocumentsNotExitsInList(_Quote.ListDocument, TableFile.Quote.ToString(), _Quote.QuoteID);

            using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        foreach (ItemInfo item in _Quote.Items)
                        {
                            QuoteDataLayer.GetInstance().UpdateItemDetails(connection, item, _userU, _Quote.QuoteID);
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
                    documentInfo.TableName = TableFile.Quote.ToString();
                    documentInfo.PreferId = _Quote.QuoteID.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Quote.ToString(), _Quote.QuoteID.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userU);
                }
                ret.isSuccess = true;*/

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
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Quote.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Quote.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    QuoteService.GetInstance().deleteProcess(connection, "Quote",id.ToString(), _userID);

                    QuoteDataLayer.GetInstance().DeleteQuoteItemsByQuotesID(connection, id.ToString());
                    //delete record
                    QuoteDataLayer.GetInstance().Delete(connection, id);
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
                    _criteria.TableName = TableFile.Quote.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Quote.ToString(), ids);
                    //delete records
                    QuoteService.GetInstance().deleteProcess(connection, "Quote", ids, _userID);
                    QuoteDataLayer.GetInstance().DeleteQuoteItemsByQuotesID(connection, ids);
                    QuoteDataLayer.GetInstance().DeleteMuti(connection, ids);
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

        public List<SelectItem> getListQuoteCode(string quoteCode , bool isContract)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SelectItem> ListQuote = QuoteDataLayer.GetInstance().getListQuoteCode(connection, quoteCode , isContract);
                return ListQuote;
            }
        }


        public ActionMessage CreateQuoteWithMutilProposal(QuoteCreateRequest QuoteObj, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    int quoteId = 0;
           
                    quoteId = QuoteDataLayer.GetInstance().InsertQuoteWithOutDetails(connection, _userI);
                    int i = 0;
                    foreach (int customerID in QuoteObj.CustomerList)
                    {
                        if (QuoteObj.CustomerList.Length == 1)
                        {
                            QuoteDataLayer.GetInstance().InsertQuoteCustomer(connection, quoteId, customerID , true);
                        }
                        else{
                            if(i == 0)
                            {
                                QuoteDataLayer.GetInstance().InsertQuoteCustomer(connection, quoteId, customerID, true);
                            }
                            else
                            {
                                QuoteDataLayer.GetInstance().InsertQuoteCustomer(connection, quoteId, customerID, false);
                            }                         
                        }
                        i++;
                    }
                    foreach (int proposalId in QuoteObj.ProposalList)
                    {
                        QuoteDataLayer.GetInstance().InsertQuoteProposal(connection, quoteId, proposalId);
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

        public List<int> getExitsQuoteProposal(QuoteCreateRequest obj, string _userI)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<int> ListQuote = QuoteDataLayer.GetInstance().getExitsQuoteProposal(connection, obj.ProposalList, obj.CustomerList, _userI);
                return ListQuote;
            }
        }



        public  async Task<ActionMessage>  importData(IFormFile file, int quoteID, int customerID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            try
            {
                string fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    // var rootFolder = @"D:\Files";
                    var rootFolder = Utils.getRootFolder();
                    var imageFolder = rootFolder + Utils.uploadFolder();
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var filePath = Path.Combine(rootFolder, fileName);
                    var fileLocation = new FileInfo(filePath);



                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                  
                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["baogia"];
                        if(workSheet == null)
                        {
                            workSheet = package.Workbook.Worksheets.First();
                        }
          
                        int totalRows = workSheet.Dimension.Rows;

                        if (totalRows < 2)
                        {
                            ret.err.msgString = "Không có dữ liệu";
                            return ret;
                        }
                        using (SqlConnection connection = sqlConnection.GetConnection())
                        {
                            QuoteDataLayer.GetInstance().deleteQuoteItem(connection,quoteID, customerID);
                        }


                        for (int i = 2; i <= totalRows; i++)
                        {
                               if(workSheet.Cells[i, 1].Value.ToString().Trim() == "") break;

                            string itemName = workSheet.Cells[i, 1].Value.ToString().Trim();
                            string itemDescription = "";
                            if (workSheet.Cells[i, 2].Value != null)
                            itemDescription = workSheet.Cells[i, 2].Value.ToString().Trim();
                            string itemUnit = workSheet.Cells[i, 3].Value.ToString().Trim();
                            string itemAmount = workSheet.Cells[i, 4].Value.ToString().Trim();
                            string itemPrice = workSheet.Cells[i, 5].Value.ToString().Trim();
                            string itemTotalPrice = workSheet.Cells[i, 6].Value.ToString().Trim();
                  
                            using (SqlConnection connection = sqlConnection.GetConnection())
                            {
                                QuoteDataLayer.GetInstance().insertQuoteItem(connection,
                                    itemName, itemDescription, itemUnit
                                   , itemPrice, itemAmount, itemTotalPrice , quoteID , customerID);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ret.err.msgString = ex.Message;
            }
        
            return ret;
        }

        public List<ItemInfo> getItemsQuote(int QuoteID, int CustomerID)
        {
            List<ItemInfo> ret = new List<ItemInfo>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                ret = QuoteDataLayer.GetInstance().GetQuoteItems(connection, QuoteID, CustomerID);

            }
            return ret;
        }

        public SelectedQuoteInfo getSelectedItemsQuote(int QuoteID, string _userID)
        {
            SelectedQuoteInfo ret = new SelectedQuoteInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                ret = QuoteDataLayer.GetInstance().getQuoteInfo(connection, QuoteID, _userID);
                ret.items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, QuoteID);
            }
            return ret;
        }

        public ActionMessage updateQuoteNew(int QuoteID, QuoteUpdateRequest data)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                QuoteDataLayer.GetInstance().updateQuoteNew(connection, QuoteID, data);
                ret.isSuccess = true;

            }
            return ret;
        }
        public ActionMessage selectQuote(int QuoteID,  int customerID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                QuoteDataLayer.GetInstance().chooseQuote(connection, QuoteID, customerID);
                ret.isSuccess = true;

            }
            return ret;
        }

        public List<searchQuoteRespone> searchQuoteCanCreateAudit(string text)
        {
            List<searchQuoteRespone> ret = new List<searchQuoteRespone>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                ret = QuoteDataLayer.GetInstance().searchQuoteCanCreateAudit(connection, text);

            }
            return ret;
        }

        public List<searchProposalItemRespone> searchQuoteItem(string text)
        {
            List<searchProposalItemRespone> ret = new List<searchProposalItemRespone>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                ret = QuoteDataLayer.GetInstance().searchQuoteItem(connection, text);

            }
            return ret;
        }

        public List<SelectItem> getListquoteCodeCanCreateBiplan(string quoteCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SelectItem> ListQuote = QuoteDataLayer.GetInstance().getListquoteCodeCanCreateBiplan(connection, quoteCode);
                return ListQuote;
            }
        }

        public List<SelectItem> getListquoteCodeCanCreateNegotiation(string quoteCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SelectItem> ListQuote = QuoteDataLayer.GetInstance().getListquoteCodeCanCreateNegotiation(connection, quoteCode);
                return ListQuote;
            }
        }


        public List<SelectItem> getListquoteCodeCanCreateDecision(string quoteCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SelectItem> ListQuote = QuoteDataLayer.GetInstance().getListquoteCodeCanCreateDecision(connection, quoteCode);
                return ListQuote;
            }
        }
        public List<SelectItem> getListquoteCodeCanCreateContract(string quoteCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<SelectItem> ListQuote = QuoteDataLayer.GetInstance().getListquoteCodeCanCreateContract(connection, quoteCode);
                return ListQuote;
            }
        }

        public void deleteProcess(SqlConnection connection, string feature, string quoteIds, string _userID)
        {
            switch (feature)
            {
                case "Quote":
                    List<string> auditIds = AuditDataLayer.GetInstance().GetAuditByQuoteIds(connection, quoteIds);
                    if (auditIds.Count > 0)
                    {
                        AuditService.GetInstance().DeleteMuti(String.Join(", ", auditIds.ToArray()),_userID);
                    }
                    List<string> bidPlans = BidPlanDataLayer.GetInstance().GetBidPlanByQuoteIds(connection, quoteIds);
                    if (bidPlans.Count > 0)
                    {
                        BidPlanService.GetInstance().DeleteMuti(String.Join(", ", bidPlans.ToArray()),_userID);
                    }
                    List<string> negotiationIDs = NegotiationDataLayer.GetInstance().GetNegotiationByQuoteIds(connection, quoteIds);
                    if (negotiationIDs.Count > 0)
                    {
                        NegotiationService.GetInstance().DeleteMuti(String.Join(", ", negotiationIDs.ToArray()), _userID);
                    }
                    List<string> decisonIDs = DecisionDataLayer.GetInstance().GetDecisionByQuoteIds(connection, quoteIds);
                    if (decisonIDs.Count > 0)
                    {
                        DecisionService.GetInstance().DeleteMuti(String.Join(", ", decisonIDs.ToArray()), _userID);
                    }
                    List<string> contractIDs = ContractDataLayer.GetInstance().GetContractByQuoteIds(connection, quoteIds,_userID);
                    if (contractIDs.Count > 0)
                    {
                        ContractService.GetInstance().deleteMuti(String.Join(", ", contractIDs.ToArray()), _userID);
                    }
                    List<string> deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    List<string> acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "Audit":
                    bidPlans = BidPlanDataLayer.GetInstance().GetBidPlanByQuoteIds(connection, quoteIds);
                    if (bidPlans.Count > 0)
                    {
                        BidPlanService.GetInstance().DeleteMuti(String.Join(", ", bidPlans.ToArray()), _userID);
                    }
                    negotiationIDs = NegotiationDataLayer.GetInstance().GetNegotiationByQuoteIds(connection, quoteIds);
                    if (negotiationIDs.Count > 0)
                    {
                        NegotiationService.GetInstance().DeleteMuti(String.Join(", ", negotiationIDs.ToArray()), _userID);
                    }
                    decisonIDs = DecisionDataLayer.GetInstance().GetDecisionByQuoteIds(connection, quoteIds);
                    if (decisonIDs.Count > 0)
                    {
                        DecisionService.GetInstance().DeleteMuti(String.Join(", ", decisonIDs.ToArray()), _userID);
                    }
                    contractIDs = ContractDataLayer.GetInstance().GetContractByQuoteIds(connection, quoteIds,_userID);
                    if (contractIDs.Count > 0)
                    {
                        ContractService.GetInstance().deleteMuti(String.Join(", ", contractIDs.ToArray()), _userID);
                    }
                    deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "BidPlan":
                    negotiationIDs = NegotiationDataLayer.GetInstance().GetNegotiationByQuoteIds(connection, quoteIds);
                    if (negotiationIDs.Count > 0)
                    {
                        NegotiationService.GetInstance().DeleteMuti(String.Join(", ", negotiationIDs.ToArray()), _userID);
                    }
                    decisonIDs = DecisionDataLayer.GetInstance().GetDecisionByQuoteIds(connection, quoteIds);
                    if (decisonIDs.Count > 0)
                    {
                        DecisionService.GetInstance().DeleteMuti(String.Join(", ", decisonIDs.ToArray()), _userID);
                    }
                    contractIDs = ContractDataLayer.GetInstance().GetContractByQuoteIds(connection, quoteIds,_userID);
                    if (contractIDs.Count > 0)
                    {
                        ContractService.GetInstance().deleteMuti(String.Join(", ", contractIDs.ToArray()), _userID);
                    }
                    deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "Negotiation":
                    decisonIDs = DecisionDataLayer.GetInstance().GetDecisionByQuoteIds(connection, quoteIds);
                    if (decisonIDs.Count > 0)
                    {
                        DecisionService.GetInstance().DeleteMuti(String.Join(", ", decisonIDs.ToArray()), _userID);
                    }
                    contractIDs = ContractDataLayer.GetInstance().GetContractByQuoteIds(connection, quoteIds,_userID);
                    if (contractIDs.Count > 0)
                    {
                        ContractService.GetInstance().deleteMuti(String.Join(", ", contractIDs.ToArray()), _userID);
                    }
                    deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "Decision":
                    contractIDs = ContractDataLayer.GetInstance().GetContractByQuoteIds(connection, quoteIds,_userID);
                    if (contractIDs.Count > 0)
                    {
                        ContractService.GetInstance().deleteMuti(String.Join(", ", contractIDs.ToArray()), _userID);
                    }
                    deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "Contract":
                    deliveryReceiptIDs = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptByQuoteIds(connection, quoteIds);
                    if (deliveryReceiptIDs.Count > 0)
                    {
                        DeliveryReceiptServices.GetInstance().DeleteMuti(String.Join(", ", deliveryReceiptIDs.ToArray()), _userID);
                    }
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                case "DeliveryReceipt":
                    acceptanceIDs = AcceptanceDataLayer.GetInstance().GetAcceptanceByQuoteIds(connection, quoteIds);
                    if (acceptanceIDs.Count > 0)
                    {
                        AcceptanceServices.GetInstance().DeleteMuti(String.Join(", ", acceptanceIDs.ToArray()));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
