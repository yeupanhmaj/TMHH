using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class QuoteInfo
    {
        public int QuoteID { get; set; }

        public string proposalCodes { get; set; }

        public string QuoteCode { get; set; }

        // public string DepartmentName { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string CustomerCode { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public string TaxCode { get; set; }

        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string Surrogate { get; set; }
        public string Position { get; set; }
        public string Fax { get; set; }
        public int QuoteType { get; set; }
        public string AccountantCode { get; set; }
        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }

        public double Cost { get; set; }

        public double TotalCost { get; set; }

        public string Comment { get; set; }

        public string DeliveryLocation { get; set; }
        public string DeliveryTermAndCondition { get; set; }
        public string WarrantyTermAndCondition { get; set; }
        public int DeliveryTime { get; set; }
        public int ValidTime { get; set; }

        public DateTime DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<ItemInfo> Items { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }


        public List<QuotCustomerInfo> Quotes { get; set; }


        public List<QuoteSimpleProposalInfo> lstProposal { get; set; }
        public QuoteInfo()
        {
            Items = new List<ItemInfo>();
            ListDocument = new List<DocumentInfo>();
            ListComment = new List<CommentInfo>();
            Quotes = new List<QuotCustomerInfo>();
            lstProposal = new List<QuoteSimpleProposalInfo>();
        }
    }

    public class QuotCustomerInfo
    {

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }

        public bool IsChoosed { get; set; }

    }

    public class QuoteSimpleProposalInfo
    {
        public QuoteSimpleProposalInfo()
        {
            items = new List<ItemPropsalInfo>();
        }

        public int ProposalId { get; set; }
        public string ProposalCode { get; set; }

        public string DepartmentName { get; set; }
        public DateTime inTtime { get; set; }


        public List<ItemPropsalInfo> items { get; set; }
    }


    public class QuoteSeachCriteria
    {
        public string QuoteCode { get; set; }
        public string ProposalCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int CustomerID { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }




    public class QuoteCreateRequest
    {
        public int QuoteID { get; set; }


        public int[] CustomerList { get; set; }

        public int[] ProposalList { get; set; }
    }

    public class QuoteUpdateRequest
    {
        public string QuoteCode { get; set; }


        public bool IsVAT { get; set; }

        public float VATNumber { get; set; }
    }

    public class SelectQuoteRequest
    {
        public int quoteID { get; set; }


        public int customerID { get; set; }
    }


    public class searchQuoteRespone
    {
        public int QuoteID { get; set; }
        public string QuoteCode { get; set; }

        public string ProposalCodes { get; set; }

        public string ItemNames { get; set; }

        public int QuoteItemID { get; set; }

        public double Amount { get; set; }

        public double AvaAmount { get; set; }

        public string CustomerNames { get; set; }
    }

    public class SelectedQuoteInfo
    {

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }
        public string BidType { get; set; }
        public int BidExpirated { get; set; }
        public string BidExpiratedUnit { get; set; }
        public int BidMethod { get; set; }
        public List<ItemInfo> items { get; set; }
    }


    public class ProposalRelation
    {
        public int ProposalID { get; set; }
        public string ProposalCode { get; set; }
        public DateTime ProposalTime { get; set; }

        public int SurveyID { get; set; }
        public string SurveyCode { get; set; }
        public DateTime SurveyTime { get; set; }

        public int ExplanationID { get; set; }
        public string ExplanationCode { get; set; }
        public DateTime ExplanationTime { get; set; }

        public int QuoteID { get; set; }
        public string QuoteCode { get; set; }
        public DateTime QuoteTime { get; set; }

        public int AuditID { get; set; }
        public string AuditCode { get; set; }
        public DateTime AuditTime { get; set; }

        public int BidPlanID { get; set; }
        public string BidPlanCode { get; set; }
        public DateTime BidPlanTime { get; set; }

        public int NegotiationID { get; set; }
        public string NegotiationCode { get; set; }
        public DateTime NegotiationTime { get; set; }

        public int DecisionID { get; set; }
        public string DecisionCode { get; set; }
        public DateTime DecisionTime { get; set; }

        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public DateTime ContractTime { get; set; }

        public int DeliveryReceiptID { get; set; }
        public string DeliveryReceiptCode { get; set; }
        public DateTime DeliveryReceiptTime { get; set; }

        public int AcceptanceID { get; set; }
        public string AcceptanceCode { get; set; }
        public DateTime AcceptanceTime { get; set; }

        public String status { get; set; }
        public String step { get; set; }
    }


    public class QuoteRelation
    {
        public int QuoteID { get; set; }
        public string QuoteCode { get; set; }
        public DateTime QuoteTime { get; set; }

        public int AuditID { get; set; }
        public string AuditCode { get; set; }
        public DateTime AuditTime { get; set; }

        public int BidPlanID { get; set; }
        public string BidPlanCode { get; set; }
        public DateTime BidPlanTime { get; set; }

        public int NegotiationID { get; set; }
        public string NegotiationCode { get; set; }
        public DateTime NegotiationTime { get; set; }

        public int DecisionID { get; set; }
        public string DecisionCode { get; set; }
        public DateTime DecisionTime { get; set; }

        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public DateTime ContractTime { get; set; }

        public String status { get; set; }
        public String step { get; set; }
    }
}
