using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class AuditInfo
    {
        public int AuditID { get; set; }
        public string AuditCode { get; set; }

        public string QuoteCode { get; set; }

       // public string ProposalCode { get; set; }
        public string CurDepartmentName { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public int Preside { get; set; }
        public string PresideName { get; set; }
        public string PresideTitle { get; set; }
        public string PresideRoleName { get; set; }
        public int Secretary { get; set; }
        public string SecretaryName { get; set; }
        public string SecretaryTitle { get; set; }
        public string SecretaryRoleName { get; set; }

        public string Members { get; set; }

        public string Comment { get; set; }

        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<AuditEmployeeInfo> Employees { get; set; }

        public List<int> QuoteIDs { get; set; }

        public List<DocumentInfo> Documents { get; set; }
    }

    public class AuditDetailInfo
    {
        public int AuditID { get; set; }

        public string AuditCode { get; set; }
   
        public string Location { get; set; }

        public int Preside { get; set; }
        public string PresideName { get; set; }
        public string PresideTitle { get; set; }
        public string PresideRoleName { get; set; }
        public int Secretary { get; set; }
        public string SecretaryName { get; set; }
        public string SecretaryTitle { get; set; }
        public string SecretaryRoleName { get; set; }

        public string Members { get; set; }

        public string Comment { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public List<AuditEmployeeInfo> Employees { get; set; }

        public List<QuoteAuditInfo> Quotes { get; set; }


    }

    public class QuoteAuditInfo
    {
        public List<ItemInfo> Items { get; set; }
        public List<string> ProposalCodes { get; set; }

        public bool IsVAT { get; set; }

        public double VATNumber { get; set; }
        public int QuoteID { get; set; }
        public string QuoteCode { get; set; }
        public string CustomerName { get; set; }
        public int CustomerID { get; set; }
        public DateTime InTime { get; set; }
    }

    public class AuditSeachCriteria
    {
      
        public string AuditCode { get; set; }
 
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int CustomerID { get; set; }


        public string QuoteCode { get; set; }


        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }

    public class SearchAuditInfo
    {
        public int AuditID { get; set; }
        public string AuditCode { get; set; }
        public string QuoteCodes { get; set; }
        public string ProposalCodes { get; set; }
        public DateTime InTime { get; set; }
    }

    public class AuditWord
    {
        public string AuditCode { get; set; }

        public DateTime datein { get; set; }
    }

}



