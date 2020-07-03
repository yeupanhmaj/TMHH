using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class DecisionInfo
    {
        public int DecisionID { get; set; }

        public string DecisionCode { get; set; }


        public int CapitalID { get; set; }

        public string CapitalName { get; set; }
        public int BidPlanID { get; set; }
        public int BidMethod { get; set; }
        public string BidPlanCode { get; set; }
        public string BidType { get; set; }
        public int BidExpirated { get; set; }
        public string BidExpiratedUnit { get; set; }
        public int QuoteID { get; set; }

        public string QuoteCode { get; set; }

        

        public bool IsVAT { get; set; }

        public double VATNumber { get; set; }

        public string Comment { get; set; }

        public DateTime DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<ItemInfo> Items { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public DateTime BidPlanTime { get; set; }

        //public DateTime NegotiationTime { get; set; }

        public DateTime QuoteTime { get; set; }

   

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string AuditCode { get; set; }
        public DateTime AuditTime { get; set; }

        public string NegotiationCode { get; set; }
        public DateTime NegotiationTime { get; set; }

        public string DepartmentNames { get; set; }
    }



    public class DecisionSeachCriteria
    {
        public string DecisionCode { get; set; }
        public string QuoteCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int CustomerID { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

    }
}
