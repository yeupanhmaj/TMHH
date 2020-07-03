using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class DeliveryReceiptInfo
    {
        public int DeliveryReceiptID { get; set; }
        public int DeliveryReceiptType { get; set; }
        public string DeliveryReceiptCode { get; set; }
        public int ContractID { get; set; }
        public string ContractCode { get; set; }
        public DateTime DeliveryReceiptDate { get; set; }
        public int DeliveryReceiptNumber { get; set; }
        public int ProposalID { get; set; }
        public int QuoteID { get; set; }
        public string ProposalCode { get; set; }
        public DateTime ProposalTime { get; set; }
        public bool IsVAT { get; set; }
        public double VATNumber { get; set; }
        public string DepartmentCode { get; set; }
        public string CurDepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string CurDepartmentName { get; set; }
        public string DeliveryReceiptPlace { get; set; }

        public string QuoteCode { get; set; }

        public string UserU { get; set; }
    
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public List<CommentInfo> ListComment { get; set; }
        public List<DocumentInfo> ListDocument { get; set; }
        public List<DeliveryReceiptItemInfoNew> Items { get; set; }


        public List<DeliveryReceiptEmployeeInfo> Employees { get; set; }
    }
    public class DeliveryReceiptCriteria
    {
        public string proposalCode { get; set; }
        public int departmentID { get; set; }
        public DateTime fromDate { get; set; } 
        public DateTime toDate { get; set; } 
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
    public class DeliveryReceiptItemInfo
    {
        public int AutoID { get; set; }
        public int QuoteItemID { get; set; }

        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public double TotalPrice { get; set; }
        public string ItemName { get; set; }
        public string ItemManufactureCountry { get; set; }
        public string ItemUnit { get; set; }
        public double Amount { get; set; }
        public double ItemPrice { get; set; }
        public double ItemTotalPrice { get; set; }
        public double ShipCost { get; set; }
        public double TestCost { get; set; }
        public string ItemDocument { get; set; }
        public bool IsSub { get; set; }
        public int DeliveryReceiptID { get; set; }
        public int ManufactureYear { get; set; }
        public int StartUseYear { get; set; }
        public string DeliveryNote { get; set; }
        public string UserU { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }


    public class DeliveryReceiptItemInfoNew
    {
        public int AutoID { get; set; }
        public int QuoteItemID { get; set; }
        public int ItemID { get; set; }
        public String ItemCode { get; set; }
        public int DeliveryReceiptID { get; set; }
        public string ItemName { get; set; }
        public string  Description { get; set; }
        public string  ItemUnit { get; set; }
        public Double ItemPrice { get; set; }
        public Double  Amount { get; set; }
        public Double  TotalPrice { get; set; }
        public string UserU { get; set; }
        public bool  AcceptanceResult { get; set; }
    }

    public class DeliveryReceiptWithDepartment
    {
        public int CustomerID { get; set; }
        public int DepartmentID { get; set; }

        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string UserIContract { get; set; }

    }

}
