using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class BidPlanInfo
    {
        public int BidPlanID { get; set; }

        public string BidPlanCode { get; set; }

        public DateTime QuoteCreateTime { get; set; }

        public string CapitalName { get; set; }
        public int CapitalID { get; set; }

        public int QuoteID { get; set; }

        public string QuoteCode { get; set; }

        public string AuditCode { get; set; }
        public DateTime AuditTime { get; set; }

        public string Bid { get; set; }

        public string BidName { get; set; }
      
        public string BidTime { get; set; }

        public string BidLocation { get; set; }

        public int BidMethod { get; set; }

        public string BidType { get; set; }

        public int BidExpirated { get; set; }
        public string BidExpiratedUnit { get; set; }
        public bool IsVAT { get; set; }

        public double VATNumber { get; set; }

        public DateTime DateIn { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<ItemInfo> Items { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<CommentInfo> ListComment { get; set; }

    }

    public class BidPlanSeachCriteria
    {
      
        public string BidPlanCode { get; set; }
       
        public string QuoteCode { get; set; }
      
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
  
        public int CustomerID { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }

    public class BidPlanPrint
    {
        public int BidPlanID { get; set; }
        public string BidPlanCode { get; set; }
        public string BidName { get; set; }
        public string BidTime { get; set; }
        public string BidLocation { get; set; }
        public int BidMethod { get; set; }
        public string BidType { get; set; }
        public int BidExpirated { get; set; }
        public string BidExpiratedUnit { get; set; }

        public int CapitalID { get; set; }
        public string CapitalName { get; set; }

    }
}
