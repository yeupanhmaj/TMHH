using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class ItemInfo
    {
        public int QuoteItemID { get; set; }
        public int ItemID { get; set; }

        public int QuoteID { get; set; }

        public string ItemCode { get; set; }

        public double TotalPrice { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public string ItemUnit { get; set; }

        public double Amount { get; set; }

        public int WarrantyYears { get; set; }

        public double ItemPrice { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string QuoteNote { get; set; }

        public int ClassID { get; set; }

        public string ClassName { get; set; }

        public int ItemType { get; set; }

        public string TypeName { get; set; }

        public bool IsUse { get; set; }

        public int ProposalQuantity { get; set; }

        public int QuoteQuantity { get; set; }

        public float QuoteCost { get; set; }

        public int ProposalID { get; set; }
    }

    public class ItemPropsalInfo
    {
        public int AutoID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public bool IsExceedReserve { get; set; }
        public double NumExceedReserve { get; set; }
        public bool IsReservered { get; set; }
        public string SurveyNote { get; set; }
        public string ExplanationNote { get; set; }
        public string AcceptanceNote { get; set; }
        public bool AcceptanceResult { get; set; }

        public bool IsFinalCost { get; set; }

        public int ProposalID { get; set; }
    }


    public class ItemSurveyInfo
    {
        public int AutoID { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public double ItemAmount { get; set; }
        public string Note { get; set; }
     
    }

    public class ItemRequest{
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
    }


    public class ItemReserve
    {
        public bool IsExceedReserve { get; set; }
        public double NumExceedReserve { get; set; }
        public bool IsReservered { get; set; }
    }


    public class ItemQuoteAudit
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }

        public string Amount { get; set; }

        public string ItemPrice { get; set; }
    }
}
