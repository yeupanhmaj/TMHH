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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdminPortal.Services
{
    public class ProposalService : BaseService<ProposalService>
    {

        public int getTotalRecords(ProposalSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().getTotalRecords(connection, _criteria, _userID);
            }
        }

        public List<ProposalInfo> getAllProposal(int pageSize, int pageIndex, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<ProposalInfo> ListProposal = ProposalDataLayer.GetInstance().GetAllProposal(connection, _userID);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListProposal.Count) return new List<ProposalInfo>();
                if (max >= ListProposal.Count) pageSize = ListProposal.Count - min;
                if (pageSize <= 0) return new List<ProposalInfo>();
                return ListProposal.GetRange(min, pageSize);
            }
        }

        public List<string> getListProposalCode(string proposalCode, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListProposal = ProposalDataLayer.GetInstance().getListProposalCode(connection, proposalCode, _userID);
                return ListProposal;
            }
        }

        public List<ProposalInfo> getAllProposal(int pageSize, int pageIndex, ProposalSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                return ProposalDataLayer.GetInstance().getProposal(connection, _criteria,  _userID);

            }
        }

        public ProposalDetailInfo getDetailProposal(int _ID, string _userID)
        {
            ProposalDetailInfo proposalDetail = new ProposalDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                proposalDetail = ProposalDataLayer.GetInstance().getProposalDetail(connection, _ID, _userID);
                if (proposalDetail == null)
                {
                    return null;
                }
                proposalDetail.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Proposal.ToString();
                documentCriteria.PreferId = _ID.ToString();
                proposalDetail.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                proposalDetail.Items = new List<ItemPropsalInfo>();
                proposalDetail.Items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, _ID);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Proposal.ToString();
                commentCriteria.PreferId = _ID.ToString();
                proposalDetail.ListComment = CommentService.GetInstance().getComment(commentCriteria);
                foreach (var item in proposalDetail.ListComment)
                {
                    DocumentSeachCriteria documentCriteria2 = new DocumentSeachCriteria();
                    documentCriteria2.TableName = TableFile.Comment.ToString();
                    documentCriteria2.PreferId = item.AutoID.ToString();
                    item.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria2);
                }
                return proposalDetail;
            }
        }
        public ProposalDetailInfo GetDetailProposalByCode(string code, string _userID)
        {
            ProposalDetailBase proposalDetail = new ProposalDetailBase();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            ProposalDetailInfo ret;
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                proposalDetail = ProposalDataLayer.GetInstance().getProposalDetailByCode(connection, code, _userID);
                if (proposalDetail == null)
                {
                    return null;
                }
                ret = new ProposalDetailInfo(proposalDetail);
                ret.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.Proposal.ToString();
                documentCriteria.PreferId = proposalDetail.ProposalID.ToString();
                ret.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);

                ret.Items = new List<ItemPropsalInfo>();
                ret.Items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, proposalDetail.ProposalID);

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.Proposal.ToString();
                commentCriteria.PreferId = proposalDetail.ProposalID.ToString();
                ret.ListComment = CommentService.GetInstance().getComment(commentCriteria);
                foreach (var item in ret.ListComment)
                {
                    DocumentSeachCriteria documentCriteria2 = new DocumentSeachCriteria();
                    documentCriteria2.TableName = TableFile.Comment.ToString();
                    documentCriteria2.PreferId = item.AutoID.ToString();
                    item.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria2);
                }

            }
            return ret;
        }

        public ProposalDetailWithContactItemsInfo GetProposalWithContactItemsByCode(string code, string _userID)
        {
            ProposalDetailBase proposalDetail = new ProposalDetailBase();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            ProposalDetailWithContactItemsInfo ret;
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                proposalDetail = ProposalDataLayer.GetInstance().getProposalDetailByCode(connection, code, _userID);
                if (proposalDetail == null)
                {
                    return null;
                }
                ret = new ProposalDetailWithContactItemsInfo(proposalDetail);

                int QuoteID = ProposalDataLayer.GetInstance().getQuoteIDWithHaveFinalContact(connection, proposalDetail.ProposalID);
                QuoteInfo quote = QuoteDataLayer.GetInstance().getQuote(connection, QuoteID, _userID);
                if (quote.TotalCost > 30000000)
                {
                    ret.DeliveryReceiptType = 2;
                }
                else
                {
                    ret.DeliveryReceiptType = 1;
                }


                ret.Items = new List<ItemInfo>();
                ret.Items = QuoteDataLayer.GetInstance().GetQuoteItems(connection, QuoteID);

            }
            return ret;
        }


        public ProposalInfo getProposalbyId(int _ID, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalInfo> ListProposal = ProposalDataLayer.GetInstance().GetAllProposal(connection, _userID);
                ProposalInfo findProposal = ListProposal.Where(i => i.ProposalID == _ID).First();
                return findProposal;
            }
        }

        public List<ProposalInfo> getProposal(ProposalSeachCriteria _criteria, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalInfo> ListProposal = ProposalDataLayer.GetInstance().getProposalByID(connection, _criteria, _userID);
                return ListProposal;
            }
        }
        public int createProposal2(ProposalInfo _Proposal, string _userI)
        {
            int ret = -1;



            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {               
                    int year = DateTime.Now.Year;
                    int maxPropCode = ProposalDataLayer.GetInstance().GetMaxPropCode(connection, year);
                    string departmentCode = DepartmentDataLayer.GetInstance().GetDepartmentCodeById(connection, _Proposal.DepartmentID);

                    _Proposal.ProposalCode = departmentCode + year.ToString() + maxPropCode.ToString().PadLeft(4, '0');
                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Insert";
                    info.Description = "ngày : " + _Proposal.DateIn + " khoa/phòng chịu trách nhiệm ID " + _Proposal.CurDepartmentID + " code " + _Proposal.ProposalCode
                        + " khoa phòng đề xuất ID : " + _Proposal.DepartmentID + " trạng thái : " + _Proposal.Status;
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = _userI;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);

                    ret = ProposalDataLayer.GetInstance().InsertProposal(connection, _Proposal, _userI);
                    ProposalDataLayer.GetInstance().UpdateMaxCode(connection, maxPropCode, year);

                    //   ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ret;
        }

        public ActionMessage createProposal(ProposalInfo _Proposal, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    int year = DateTime.Now.Year;
                    int maxPropCode = ProposalDataLayer.GetInstance().GetMaxPropCode(connection, year);
                    string departmentCode = DepartmentDataLayer.GetInstance().GetDepartmentCodeById(connection, _Proposal.DepartmentID);

                    _Proposal.ProposalCode = departmentCode + year.ToString() + maxPropCode;
                    ret.id = ProposalDataLayer.GetInstance().InsertProposal(connection, _Proposal, _userI);
                    ProposalDataLayer.GetInstance().UpdateMaxCode(connection, maxPropCode, year);
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

        public ActionMessage insertItems(int proposalId, List<ItemPropsalInfo> items, DateTime? dateIn, int departmentID)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    var reserve = ProposalDataLayer.GetInstance().GetReserve(connection, dateIn);
                    if (reserve != null)
                    {
                        foreach (var item in items)
                        {
                            int reserveID = reserve.ReserveID;
                            var CheckReserve = ProposalDataLayer.GetInstance().CheckReserve(connection, item.ItemID, item.Amount, reserveID, departmentID);
                            if(CheckReserve != null)
                            {
                                item.IsExceedReserve = CheckReserve.IsExceedReserve;
                                item.NumExceedReserve = CheckReserve.NumExceedReserve;
                                item.IsReservered = CheckReserve.IsReservered;
                            }
                            else
                            {
                                if(ProposalDataLayer.GetInstance().CheckIsHosReserve(connection, item.ItemID, reserveID))
                                {
                                    item.IsExceedReserve = false;
                                    item.NumExceedReserve = 0;
                                    item.IsReservered = true;
                                }
                                else if (item.Amount > 0)
                                {
                                    item.IsExceedReserve = true;
                                    item.NumExceedReserve = item.Amount;
                                    item.IsReservered = false;
                                }
                                else
                                {
                                    item.IsExceedReserve = false;
                                    item.NumExceedReserve = 0;
                                    item.IsReservered = false;
                                }
                            }
                            ProposalDataLayer.GetInstance().InsertProposalItem(connection, proposalId, item);
                        }
                        ret.isSuccess = true;
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = "Chưa tạo dự trù năm";
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

        public ActionMessage EditItems(int proposalId, List<ItemPropsalInfo> _items)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    List<ItemPropsalInfo> currentItems = ProposalDataLayer.GetInstance().GetPropsalItems(connection, proposalId);

                    var listDeleteItem = new List<ItemPropsalInfo>();
                    var listInsertItems = new List<ItemPropsalInfo>();
                    if (_items == null)
                    {
                        listDeleteItem = currentItems;
                        listInsertItems = new List<ItemPropsalInfo>();
                    }
                    else
                    {
                        listDeleteItem = currentItems.Except(_items).ToList();
                        listInsertItems = _items.Except(currentItems).ToList();
                    }
                    //add new Items
                    foreach (var item in listInsertItems)
                    {
                        ProposalDataLayer.GetInstance().InsertProposalItem(connection, proposalId, item);
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
                        ProposalDataLayer.GetInstance().DeleteProposalItems(connection, autoIds);
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


        public ActionMessage editProposal(int id, ProposalInfo _Proposal, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserLogInfo info = new UserLogInfo();
            info.Action = "Update";
            info.Description = "ngày " + _Proposal.DateIn + " khoa/phòng chịu trách nhiệm ID " + _Proposal.CurDepartmentID + " code " + _Proposal.ProposalCode
               + " khoa phòng đề xuất ID " + _Proposal.DepartmentID + " trạng thái " + _Proposal.Status;
            info.Feature = TableFile.Proposal.ToString();
            info.Time = DateTime.Now;
            info.UserName = _userU;
            info.UserID = 1;
            UserService.GetInstance().TrackUserAction(info);
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                ProposalSeachCriteria _criteria = new ProposalSeachCriteria();
                _criteria.ProposalID = id.ToString();
                var chkProposalInfo = getProposal(_criteria, "");
                if (chkProposalInfo.Count > 0)
                {
                    try
                    {
                        ProposalDataLayer.GetInstance().UpdateProposal(connection, id, _Proposal, _userU);
                        ret.isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = ex.Message;
                    }
                }
            }
            return ret;
        }

        public ActionMessage DeleteProposal(int id , string user, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {

                    ProposalDetailInfo temp = ProposalService.GetInstance().getDetailProposal(id, "");

                    UserLogInfo info = new UserLogInfo();
                    info.Action = "Delete ";

                    if (temp != null)
                    {
                        info.Description = "code : " + temp.ProposalCode + " id : " + id;
                    }
                    else
                    {
                        info.Description =  " id : " + id;
                    }
                    info.Feature = TableFile.Proposal.ToString();
                    info.Time = DateTime.Now;
                    info.UserName = user;
                    info.UserID = 1;
                    UserService.GetInstance().TrackUserAction(info);

                    //delete Items Table 
                    List<ItemPropsalInfo> currentItems = ProposalDataLayer.GetInstance().GetPropsalItems(connection, id);
                    if (currentItems.Count > 0)
                    {
                        string autoIds = "";
                        foreach (var item in currentItems)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        ProposalDataLayer.GetInstance().DeleteProposalItems(connection, autoIds);
                    }


                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Proposal.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.Proposal.ToString(), id);

                    //deleteRelatedProsal
    //             ..   var quoteIds = QuoteDataLayer.GetInstance().GetQuotesByProposalId(connection,id.ToString());
                   // if(quoteIds.Count > 0)
  //                  QuoteService.DeleteMuti(String.Join(", ", quoteIds.ToArray()));

             
                    var surveyIds = SurveyDataLayer.GetInstance().GetSurveyByProposalId(connection, id.ToString());


                    if (surveyIds.Count > 0)
                        SurveyDataLayer.GetInstance().DeleteMuti(connection, String.Join(", ", surveyIds.ToArray()));

                    var explanationServiceIds = ExplanationDataLayer.GetInstance().GetExplanationsByProposalId(connection, id.ToString(), _userID);

                    if (explanationServiceIds.Count > 0)
                        ExplanationDataLayer.GetInstance().DeleteMuti(connection, String.Join(", ", explanationServiceIds.ToArray()));


                    //delete record
                    ProposalDataLayer.GetInstance().DeleteProposal(connection, id);
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
        public ActionMessage DeleteProposals(string ids , string user, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            UserLogInfo info = new UserLogInfo();
            info.Action = "Delete ";
            info.Description = " ids: " + ids;
            info.Feature = TableFile.Proposal.ToString();
            info.Time = DateTime.Now;
            info.UserName = user;
            info.UserID = 1;
            UserService.GetInstance().TrackUserAction(info);
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete Items Table 
                    List<ItemPropsalInfo> currentItems = ProposalDataLayer.GetInstance().GetPropsalItemsByIds(connection, ids);
                    if (currentItems.Count > 0)
                    {
                        string autoIds = "";
                        foreach (var item in currentItems)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        ProposalDataLayer.GetInstance().DeleteProposalItems(connection, autoIds);
                    }

                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.Proposal.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.Proposal.ToString(), ids);
                    //delete records

                  /*  //deleteRelatedProsal
                    var quoteIds = QuoteDataLayer.GetInstance().GetQuotesByProposalId(connection, ids);

                    QuoteService.DeleteMuti(String.Join(", ", quoteIds.ToArray()));*/

                    var surveyIds = SurveyDataLayer.GetInstance().GetSurveyByProposalId(connection, ids);

                    if(surveyIds !=null  && surveyIds.Count  >0)
                    SurveyDataLayer.GetInstance().DeleteMuti(connection, String.Join(", ", surveyIds.ToArray()));

                    var explanationServiceIds = ExplanationDataLayer.GetInstance().GetExplanationsByProposalId(connection, ids, _userID);
                    if (explanationServiceIds != null && explanationServiceIds.Count > 0)
                        ExplanationDataLayer.GetInstance().DeleteMuti(connection, String.Join(", ", explanationServiceIds.ToArray()));


                    ProposalDataLayer.GetInstance().DeleteProposals(connection, ids);
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

      

        public ProposalRelatedData GetRelateData(int id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                ProposalRelatedData ret = new ProposalRelatedData();
                ret = ProposalDataLayer.GetInstance().GetRelateData(connection, id);
                return ret;
            }
        }

        public List<StatusCountReport> GetCountByStatus()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<StatusCountReport> ret = new List<StatusCountReport>();
                ret = ProposalDataLayer.GetInstance().CountByStatus(connection);
                return ret;
            }
        }


        public List<ProposalWithItems> getProposalCanCreateQuote(string proposalCode, string itemName)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalWithItems> ret = new List<ProposalWithItems>();
                ret = ProposalDataLayer.GetInstance().getProposalCanCreateQuote(connection, proposalCode , itemName );
                return ret;
            }
        }

        public List<ProposalSelectItem> GetListProsalHaveQuote(string code)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().GetListProsalHaveQuote(connection, code);
            }
        }
        public List<ProposalSelectItem> GetListProsalCanCreateAcceptance(string code)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().GetListProsalCanCreateAcceptance(connection, code);
            }
        }
     


        public DRFillDetailInfo getDetailsForDR(int id)
        {
            DRFillDetailInfo proposalDetail = new DRFillDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                proposalDetail = ProposalDataLayer.GetInstance().getDetailsForDR(connection, id);
                proposalDetail.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, proposalDetail.QuoteID);
                
                string IDs = "( "; 
                foreach (var item in proposalDetail.Items)
                {
                    IDs += item.QuoteItemID.ToString() + " ,";
                }

                IDs = IDs.Remove(IDs.Length - 1);
                IDs += " )";
                List<DeliveryReceiptItemInfoNew> deliveryReceiptItems = DeliveryReceiptDataLayer.GetInstance().getItemsByQuoteIDs(connection, IDs);

                foreach (var DRitem in deliveryReceiptItems)
                {
                    for (int i = 0; i < proposalDetail.Items.Count;)
                    {
                        if (DRitem.QuoteItemID == proposalDetail.Items[i].QuoteItemID)
                        {
                            proposalDetail.Items[i].Amount = proposalDetail.Items[i].Amount - DRitem.Amount;
                        }
                        if (proposalDetail.Items[i].Amount > 0)
                        {
                            i++;
                        }
                        else
                        {
                            proposalDetail.Items.RemoveAt(i);
                        }
                    }
                }

                //todo
                return proposalDetail;
            }
        }



        public DRFillDetailInfo getDetailsAcceptanceByProposalID(int id)
        {
            DRFillDetailInfo proposalDetail = new DRFillDetailInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                proposalDetail = ProposalDataLayer.GetInstance().getDetailsForDR(connection, id);
                proposalDetail.Items = QuoteDataLayer.GetInstance().getSelectedItemsQuote(connection, proposalDetail.QuoteID);
                proposalDetail.DeliveryReceiptItems =  DeliveryReceiptDataLayer.GetInstance().getDetailsAcceptanceByProposalID(connection, id);

                return proposalDetail;
            }
        }

        public QuoteRelation getQuoteRelation(int quoteID)
        {
            QuoteRelation ret = new QuoteRelation();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                ret = ProposalDataLayer.GetInstance().getQuoteRelation(connection, quoteID);              
                return ret;
            }
        }


        public List<ProposalSelectItem> getListProsalCanCreateDR(string code)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().getListProsalCanCreateDR(connection, code);
            }
        }
        //Nguyen Minh Hoang
        public List<ProposalInfo> GetAllOutdateProposal(string userI)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().GetAllOutdateProposal(connection, userI);
            }
        }
        public List<ProposalsByDepartment> GetByDepartment()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().CountByDepartment(connection);
            }
        }
        public List<ProposalInfo> GetAllExceedReserveProposal()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().GetAllExceedReserveProposal(connection);
            }
        }

     
        public List<ProposalRelatedData> GetProposalProccess(int proposalID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return ProposalDataLayer.GetInstance().GetProposalProccess(connection , proposalID);
            }
        }

        public TimeLineRecord GetProposalProccess2(int proposalID, string _userI, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            TimeLineRecord ret = new TimeLineRecord();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {               
                ProcessInfo info =  ProposalDataLayer.GetInstance().GetProposalProccessID(connection, proposalID);
                if(info.ProposalID !=0)
                    ret.ProposalDetailInfo = ProposalService.GetInstance().getDetailProposal(info.ProposalID, _userI);
                if (info.SurveyID != 0)
                    ret.SurveyDetailInfo = SurveyService.GetInstance().GetDetailSurvey(info.SurveyID,_userID);
                if (info.ExplanationID != 0)
                    ret.ExplanationDetailInfo = ExplanationService.GetInstance().getDetailExplanation(info.ExplanationID, _userID);
                if (info.QuoteID != 0)
                    ret.QuoteInfo = QuoteService.GetInstance().getQuote(info.QuoteID, _userID);
                if (info.AuditID != 0)
                    ret.AuditDetailInfo = AuditService.GetInstance().getAuditInfo(info.AuditID,_userID);
                if (info.BidPlanID != 0)
                    ret.BidPlanInfo = BidPlanService.GetInstance().getBidPlan(info.BidPlanID, _userID);
                if (info.NegotiationID != 0)
                    ret.NegotiationInfo = NegotiationService.GetInstance().GetNegotiation(info.NegotiationID, _userID);
                if (info.DecisionID != 0)
                    ret.DecisionInfo = DecisionService.GetInstance().GetDecision(info.DecisionID, _userID);
                if (info.ContractID != 0)
                    ret.ContractInfo = ContractService.GetInstance().getContractNew(info.ContractID,_userID);
                if (info.DeliveryReceiptID != 0)
                    ret.DeliveryReceiptInfo = DeliveryReceiptServices.GetInstance().GetDetail(info.DeliveryReceiptID, _userID);
                if (info.AcceptanceID != 0)
                    ret.AcceptanceInfo = AcceptanceServices.GetInstance().GetDetail(info.AcceptanceID,_userID);
            }
            return ret;
        }
    }
}