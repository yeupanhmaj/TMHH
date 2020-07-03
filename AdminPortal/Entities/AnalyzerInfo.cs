using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class AnalyzerInfo
    {
        public int AnalyzerID { get; set; }

        public string AnalyzerCode { get; set; }

        public string AnalyzerAccountantCode { get; set; }

        public string AnalyzerName { get; set; }

        public int AnalyzerType { get; set; }

        public int AnalyzerGroupID { get; set; }

        public string AnalyzerGroupCode { get; set; }

        public string AnalyzerGroupName { get; set; }

        public int QuoteItemID { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

        public double ItemPrice { get; set; }

        public double TotalPrice { get; set; }

        public int DepartmentRootID { get; set; }

        public string DepartmentRootCode { get; set; }

        public string DepartmentRootName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string ContractCode { get; set; }

        public string UserIContract { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string Serial { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime DateIn { get; set; }

        public int DeliveryReceiptID { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime DeliveryReceiptDate { get; set; }

        public int Number { get; set; }
    }

    public class AnalyzerSeachCriteria
    {

        public string AnalyzerCode { get; set; }

        public string AnalyzerName { get; set; }

        public string DepartmentID { get; set; }

        public string CustomerID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int pageSize { get; set; }
        public int pageIndex { get; set; }

        public string DepartmentName { get; set; }
        public string Price { get; set; }
        public string CustomerName { get; set; }
    }

    //public class AnalyzerPrint
    //{
    //    public int AnalyzerID { get; set; }
    //    public string AnalyzerCode { get; set; }
    //}
}
