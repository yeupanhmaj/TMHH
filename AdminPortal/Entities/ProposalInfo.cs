using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace AdminPortal.Entities
{
    public class ProposalInfo
    {
        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public string InCode { get; set; }

        public int ProposalType { get; set; }

        public string ProposalTypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string UserAssigned { get; set; }

        public string ContractCode { get; set; }

        public int DateDiff { get; set; }

        public string IsExceedReserve { get; set; }
        public string ItemsName { get; set; }
        public int CategoryID { get; set; }

        [DefaultValue(1)]
        public int Status { get; set; }

        public int CurDepartmentID { get; set; }

        public string CurDepartmentCode { get; set; }

        public string CurDepartmentName { get; set; }

        public string FollowComment { get; set; }

        public string Comment { get; set; }

        public string Opinion { get; set; }
        public DateTime DueDate { get; set; }
        public string UserI { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<ItemPropsalInfo> Items { get; set; }

        public List<DocumentInfo> Documents { get; set; }
    }
    public class SeachCode
    {
        public int ProposalID { get; set; }

    }
    public class ProposalDetailInfo : ProposalDetailBase
    {
        public ProposalDetailInfo()
        {
            this.ListComment = new List<CommentInfo>();
            this.ListDocument = new List<DocumentInfo>();
            this.Items = new List<ItemPropsalInfo>();
        }
        public ProposalDetailInfo(ProposalDetailBase parent)
        {
            this.ProposalID = parent.ProposalID;
            this.ProposalCode = parent.ProposalCode;
            this.InCode = parent.InCode;

            this.ProposalType = parent.ProposalType;

            this.ProposalTypeName = parent.ProposalTypeName;

            this.DepartmentID = parent.DepartmentID;

            this.DepartmentName = parent.DepartmentName;

            this.UserAssigned = parent.UserAssigned;

            this.ContractCode = parent.ContractCode;

            this.Status = parent.Status;

            this.CurDepartmentID = parent.CurDepartmentID;
            this.CurDepartmentName = parent.CurDepartmentName;
            this.FollowComment = parent.FollowComment;

            this.Opinion = parent.Opinion;
            this.Comment = parent.Comment;

            this.UserI = parent.UserI;

            this.DateIn = parent.DateIn;

            this.InTime = parent.InTime;

            this.UserU = parent.UserU;

            this.UpdateTime = parent.UpdateTime;

            this.ListComment = new List<CommentInfo>();
            this.ListDocument = new List<DocumentInfo>();
            this.Items = new List<ItemPropsalInfo>();
        }
    
        public List<CommentInfo> ListComment { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<ItemPropsalInfo> Items { get; set; }
    }


    public class ProposalDetailBase {
        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public string InCode { get; set; }

        public int ProposalType { get; set; }

        public string ProposalTypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string UserAssigned { get; set; }

        public string ContractCode { get; set; }

        [DefaultValue(1)]
        public int Status { get; set; }

        public int CurDepartmentID { get; set; }

        public string CurDepartmentCode { get; set; }

        public string CurDepartmentName { get; set; }

        public string FollowComment { get; set; }

        public string Opinion { get; set; }

        public string Comment { get; set; }

        public string UserI { get; set; }

        public DateTime DateIn { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

    }
    public class ProposalDetailWithContactItemsInfo: ProposalDetailBase
    {
        public ProposalDetailWithContactItemsInfo()
        {

        }
        public ProposalDetailWithContactItemsInfo(ProposalDetailBase parent)
        {
            this.ProposalID = parent.ProposalID;
            this.ProposalCode = parent.ProposalCode;
            this.InCode = parent.InCode;

            this.ProposalType = parent.ProposalType;

            this.ProposalTypeName = parent.ProposalTypeName;

            this.DepartmentID = parent.DepartmentID;

            this.DepartmentCode = parent.DepartmentCode;

            this.DepartmentName = parent.DepartmentName;

            this.UserAssigned = parent.UserAssigned;

            this.ContractCode = parent.ContractCode;

            this.Status = parent.Status;

            this.CurDepartmentID = parent.CurDepartmentID;
            this.CurDepartmentCode = parent.CurDepartmentCode;
            this.CurDepartmentName = parent.CurDepartmentName;
            this.FollowComment = parent.FollowComment;
            this.Opinion = parent.Opinion;
            this.Comment = parent.Comment;

            this.UserI = parent.UserI;

            this.DateIn = parent.DateIn;

            this.InTime = parent.InTime;

            this.UserU = parent.UserU;

            this.UpdateTime = parent.UpdateTime;

            this.Items = new List<ItemInfo>();
        }
        public List<ItemInfo> Items { get; set; }

        public int DeliveryReceiptType { get; set; }
    }


    public class ProposalSeachCriteria
    {
       
        public string ProposalID { get; set; }
        public string ProposalCode { get; set; }
        public int DepartmentID { get; set; }
        public string ProposalType { get; set; }
        public string Status { get; set; }
        //public string ContractCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public string Item { get; set; }
    }

    public class ProposalReqObjWithAttFile
    {
        public ProposalInfo propsalObj { get; set; }
        public List<IFormFile> files { get; set; }
    }

    public class ProposalRelatedData
    {
        public ProposalRelatedData()
        {
            lstExplanation = new List<SingleDetailsData>();
            lstSurvey = new List<SingleDetailsData>();
            lstQuote = new List<SingleDetailsData>();
            lstAudit = new List<SingleDetailsData>();
            lstBidPlan = new List<SingleDetailsData>();
            lstNegotiation = new List<SingleDetailsData>();
            lstDecision = new List<SingleDetailsData>();
            lstContract = new List<SingleDetailsData>();
            lstDeliveryReceipt = new List<SingleDetailsData>();
            lstAcceptance = new List<SingleDetailsData>();
            lstProccess = new List<SingleDetailsData>();
        }
        public int ProposalID { get; set; }
        public string ProposalCode { get; set; }
        public string CurrentFeature { get; set; }
        public List<SingleDetailsData> lstProccess { get; set; }
        public List<SingleDetailsData> lstExplanation { get; set; } 
        public List<SingleDetailsData> lstSurvey { get; set; }
        public List<SingleDetailsData> lstQuote { get; set; }
        public List<SingleDetailsData> lstAudit { get; set; }
        public List<SingleDetailsData> lstBidPlan { get; set; }
        public List<SingleDetailsData> lstNegotiation { get; set; }
        public List<SingleDetailsData> lstDecision { get; set; }
        public List<SingleDetailsData> lstContract { get; set; }
        public List<SingleDetailsData> lstDeliveryReceipt { get; set; }
        public List<SingleDetailsData> lstAcceptance { get; set; }

    }
    public class SingleDetailsData
    {
        public string name { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public DateTime date { get; set; }
    }

    public class StatusCountReport
    {
        public string label { get; set; }
        public string value { get; set; }
    }
    public class ProposalsByDepartment
    {
        public string label { get; set; }
        public string value { get; set; }
    }


    public class ProposalWithItems
    {
        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }



        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public string ItemUnit { get; set; }

        public Double ItemAmount { get; set; }
    }

    public class ProposalSelectItem
    {
        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }
    }

    public class DRFillDetailInfo
    {
        public int QuoteID { get; set; }
        public int ContractID{ get; set; }

        public int ProposalID { get; set; }
        public string CurDepartmentName { get; set; }
        public string DepartmentName { get; set; }
        public string ContractCode{ get; set; }
        public string DecisionCode{ get; set; }
        public int CapitalID { get; set; }
        public string QuoteCode { get; set; }     
        public List<ItemInfo> Items { get; set; }
        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }
        public int ProposalType { get; set; }

        public List<DeliveryReceiptItemInfoNew> DeliveryReceiptItems { get; set; }

    }


    public class searchProposalItemRespone
    {
        public int DeliveryReceiptID { get; set; }
        public int QuoteItemID { get; set; }
        public string ItemNames { get; set; }
        public string ProposalCodes { get; set; }
        public double Amount { get; set; }
        public double AvaAmount { get; set; }
    }


    public class TimeLineRecord
    {
        public TimeLineRecord()
        {
            ProposalDetailInfo = new ProposalDetailInfo();
            ExplanationDetailInfo = new ExplanationDetailInfo();
            SurveyDetailInfo = new SurveyDetailInfo();
            QuoteInfo = new QuoteInfo();
            AuditDetailInfo = new AuditDetailInfo();
            BidPlanInfo = new BidPlanInfo();
            NegotiationInfo = new NegotiationInfo();
            DecisionInfo = new DecisionInfo();
            ContractInfo = new NewContractInfo();
            DeliveryReceiptInfo = new DeliveryReceiptInfo();
            AcceptanceInfo = new AcceptanceInfo();
        }
        public ProposalDetailInfo ProposalDetailInfo { get; set; }
        public ExplanationDetailInfo ExplanationDetailInfo { get; set; }
        public SurveyDetailInfo SurveyDetailInfo { get; set; }
        public QuoteInfo QuoteInfo { get; set; }
        public AuditDetailInfo AuditDetailInfo { get; set; }
        public BidPlanInfo BidPlanInfo { get; set; }
        public NegotiationInfo NegotiationInfo { get; set; }
        public DecisionInfo DecisionInfo { get; set; }
        public NewContractInfo ContractInfo { get; set; }
        public DeliveryReceiptInfo DeliveryReceiptInfo { get; set; }
        public AcceptanceInfo AcceptanceInfo { get; set; }

    }



    public class ProcessInfo
    {

        public int ProposalID  { get; set; }
        public int ExplanationID  { get; set; }
        public int SurveyID  { get; set; }
        public int QuoteID  { get; set; }
        public int AuditID  { get; set; }
        public  int BidPlanID { get; set; }
        public int NegotiationID { get; set; }
        public int DecisionID { get; set; }
        public int ContractID { get; set; }
        public  int DeliveryReceiptID { get; set; }
        public  int AcceptanceID { get; set; }

    }
}
