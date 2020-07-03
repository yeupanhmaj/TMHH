using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Commons;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;

namespace AdminPortal.Services
{
    public class BidPlanService : BaseService<BidPlanService>
    {

        public int getTotalRecords(BidPlanSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return BidPlanDataLayer.GetInstance().getTotalRecords(connection, _criteria,_userID);
            }
        }

        public List <BidPlanInfo> getAllBidPlan(int pageSize,int pageIndex, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<BidPlanInfo> ListBidPlan = BidPlanDataLayer.GetInstance().GetAllBidPlan(connection,_userID);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListBidPlan.Count) return new List<BidPlanInfo>();
                if (max >= ListBidPlan.Count) pageSize = ListBidPlan.Count - min;
                if (pageSize <= 0) return new List<BidPlanInfo>();
                return ListBidPlan.GetRange(min, pageSize);
            }
        }

        public List<BidPlanInfo> getAllBidPlan(int pageSize, int pageIndex, BidPlanSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return BidPlanDataLayer.GetInstance().getBidPlan(connection, _criteria, _userID);
            }
        }

        public BidPlanInfo getBidPlan(int _ID, string _userID)
        {
            BidPlanInfo record = new BidPlanInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = BidPlanDataLayer.GetInstance().getBidPlan(connection, _ID, _userID);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, record.QuoteID);

                //bidPlanDetail.ListMember = BidPlanDataLayer.GetInstance().GetBidPlanMembers(connection, _ID);
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.BidPlan.ToString();
                documentCriteria.PreferId = _ID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.BidPlan.ToString();
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


        public BidPlanInfo GetBidPlanByCode(string code)
        {
            BidPlanInfo record = new BidPlanInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = BidPlanDataLayer.GetInstance().GetBidPlanByCode(connection, code);
                if (record == null)
                {
                    return null;
                }

                record.Items = new List<ItemInfo>();
                record.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, record.QuoteID);
                //bidPlanDetail.ListMember = BidPlanDataLayer.GetInstance().GetBidPlanMembers(connection, _ID);
                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.BidPlan.ToString();
                documentCriteria.PreferId = record.QuoteID.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.BidPlan.ToString();
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


        public ActionMessage createBidPlan(BidPlanInfo _BidPlan, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ret.id = BidPlanDataLayer.GetInstance().InsertBidPlan(connection, _BidPlan, _userI);
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

        public async Task<ActionMessage> createBidPlan2(BidPlanInfo _BidPlan, string _userI, IList<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    QuoteInfo quoteInfo = QuoteDataLayer.GetInstance().getQuote(connection, _BidPlan.QuoteID, _userID);
                    if (quoteInfo != null)
                    {
                        _BidPlan.QuoteID = quoteInfo.QuoteID;
             
                    }
                    else _BidPlan.QuoteID /*= _BidPlan.ProposalID*/ = 0;
                    insetId = BidPlanDataLayer.GetInstance().InsertBidPlan(connection, _BidPlan, _userI);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (insetId > -1)
                {
                    ret.id = insetId;
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.BidPlan.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.BidPlan.ToString(), insetId.ToString(), item, documentInfo.Link);
                        DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                    }
                    ret.isSuccess = true;
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "lỗi thêm biên bản họp giá";
                    ret.err.msgString = "lỗi thêm biên bản họp giá";
                }
                return ret;

            }
        }

        public async Task<ActionMessage> editBidPlan(int id, BidPlanInfo _BidPlan, string _userU, IList<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                var chkBidPlanInfo = BidPlanDataLayer.GetInstance().getBidPlan(connection, id, _userID);
                if (chkBidPlanInfo != null)
                {
                    try
                    {
                        QuoteInfo quoteInfo = QuoteDataLayer.GetInstance().getQuote(connection, _BidPlan.QuoteID, _userID);
                        if (quoteInfo != null)
                        {
                            _BidPlan.QuoteID = quoteInfo.QuoteID;

                            //var ProposalInfo = ProposalDataLayer.GetInstance().getProposalDetail(connection, _BidPlan.ProposalID);
                            //if (ProposalInfo != null)
                            //{
                            //    _BidPlan.ProposalCode = ProposalInfo.ProposalCode;
                            //}
                        }
                        else _BidPlan.QuoteID /*= _BidPlan.ProposalID*/ = 0;
                        BidPlanDataLayer.GetInstance().UpdateBidPlan(connection, id, _BidPlan, _userU);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_BidPlan.ListDocument, TableFile.BidPlan.ToString(), _BidPlan.BidPlanID);

                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.BidPlan.ToString();
                    documentInfo.PreferId = _BidPlan.BidPlanID.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.BidPlan.ToString(), _BidPlan.BidPlanID.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userU);
                }
                ret.isSuccess = true;


                return ret;
            }
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
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.BidPlan.ToString(), id);


                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.BidPlan.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete record
                    List<string> negotiationIDs = NegotiationDataLayer.GetInstance().GetNegotiationByBidPlanIds(connection, id.ToString());
                    List<string> quoteID = BidPlanDataLayer.GetInstance().GetQuoteByBidPlanIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "BidPlan", String.Join(", ", quoteID.ToArray()), _userID);
                    BidPlanDataLayer.GetInstance().Delete(connection, id);
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
                    _criteria.TableName = TableFile.BidPlan.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.BidPlan.ToString(), ids);
                    List<string> quoteID = BidPlanDataLayer.GetInstance().GetQuoteByBidPlanIds(connection, ids);
                    QuoteService.GetInstance().deleteProcess(connection, "BidPlan", String.Join(", ", quoteID.ToArray()), _userID);
                    BidPlanDataLayer.GetInstance().DeleteMuti(connection, ids);
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

        public List<string> getListBidPlanCode(string bidPlanCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListBidPlan = BidPlanDataLayer.GetInstance().getListBidPlanCode(connection, bidPlanCode);
                return ListBidPlan;
            }
        }

    }
}
