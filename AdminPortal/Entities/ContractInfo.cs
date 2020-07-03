using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class ContractInfo
    {
        public int ContractID { get; set; }

        public string ContractCode { get; set; }

        public  string CustomerName { get; set; }

        public string Code { get; set; }

        public int DecisionID { get; set; }

        public string DecisionCode { get; set; }

        public int NegotiationID { get; set; }

        public string NegotiationCode { get; set; }

        public int BidPlanID { get; set; }

        public string BidPlanCode { get; set; }

        public int AuditID { get; set; }

        public string AuditCode { get; set; }

        public int QuoteID { get; set; }

        public string QuoteCode { get; set; }

        public bool IsVAT { get; set; }

        public double VATNumber { get; set; }

        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public string DepartmentName { get; set; }

        public string Comment { get; set; }

        public DateTime? DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public int DeliveryReceiptType { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<ItemInfo> Items { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public string ASide { get; set; }

        public string ALocation { get; set; }

        public string APhone { get; set; }

        public string AFax { get; set; }

        public string ABankID { get; set; }

        public string ATaxCode { get; set; }

        public string ARepresent { get; set; }

        public string APosition { get; set; }

        public string BSide { get; set; }

        public string BLocation { get; set; }

        public string BPhone { get; set; }

        public string BFax { get; set; }

        public string BBankID { get; set; }

        public string BTaxCode { get; set; }

        public string BRepresent { get; set; }

        public string BPosition { get; set; }

        public DateTime? NegotiationTime { get; set; }

        public DateTime? DecisionTime { get; set; }

        public double QuoteTotalCost { get; set; }

    }


    public class NewContractInfo
    {
        public int ContractID { get; set; }

        public string ContractCode { get; set; }

        public int NegotiationID { get; set; }

        public DateTime DateIn { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public NegotiationInfo negotiation { get; set; }
    }

    public class ContractSeachCriteria
    {
        public string ContractCode { get; set; }
        public string QuoteCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int CustomerID { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
}


    public class ContractPrintModel
    {

        public string DecisionCode { get; set; }

        public DateTime DecisionTime { get; set; }

        public string ContractCode { get; set; }

        public DateTime NegotiationTime { get; set; }
  

        public string NegotiationCode { get; set; }

        public string Code { get; set; }

        public int BidPlanID { get; set; }

        public string BidPlanCode { get; set; }

        public int AuditID { get; set; }

        public string AuditCode { get; set; }

        public int QuoteID { get; set; }

        public string QuoteCode { get; set; }

        public double QuoteTotalCost { get; set; }
        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }
        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public string DepartmentName { get; set; }

        public string ASide { get; set; }

        public string ALocation { get; set; }

        public string APhone { get; set; }

        public string AFax { get; set; }

        public string ABankID { get; set; }

        public string ATaxCode { get; set; }

        public string ARepresent { get; set; }

        public string APosition { get; set; }

        public string BSide { get; set; }

        public string BLocation { get; set; }

        public string BPhone { get; set; }

        public string BFax { get; set; }

        public string BBankID { get; set; }

        public string BTaxCode { get; set; }

        public string BRepresent { get; set; }

        public string BPosition { get; set; }

        public string BidType { get; set; }

        public int BidExpirated { get; set; }
        public string BidExpiratedUnit { get; set; }

        public int Term { get; set; }

        public DateTime DateIn { get; set; }

        public List<ItemInfo> Items { get; set; }
        public DateTime AuditTime { get; set; }

        public DateTime BidPlanTime { get; set; }

    }
    public class ContractSelectItem
    {
        public int ContractID { get; set; }

        public string ContractCode { get; set; }
    }

  }
