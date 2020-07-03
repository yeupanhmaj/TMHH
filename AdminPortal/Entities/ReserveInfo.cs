using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class ReserveInfo
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public double Packet { get; set; }
        public bool SpecialItem { get; set; }
        public string CategoryName { get; set; }
        public string ClassName { get; set; }
        public int? ReserveDetailSystemId { get; set; }
        public int ReserveID { get; set; }
        public string ReserveCode { get; set; }
        public DateTime? DateReserve { get; set; }
        public double ReserveUnit { get; set; }
        public double ReserveTest { get; set; }
        public double Cost { get; set; }
        public double SubTotal { get; set; }
        public bool Checked { get; set; }
        public bool IsReserveTotal { get; set; }

    }
    public class AllocationReserve
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string ClassName { get; set; }
        public int DepartmentID { get; set; }
        public int? ReserveDetailSystemId { get; set; }
        public int ReserveID { get; set; }
        public string ReserveCode { get; set; }
        public DateTime? DateReserve { get; set; }
        public double ReserveUnit { get; set; }
        public string Comment { get; set; }
        public string OriComment { get; set; }
        public int Available { get; set; }
        public int Using { get; set; }
        public int Repairing { get; set; }
        public int Substitute { get; set; }
    }

    public class ReserveCodeInfo
    {
        public int ReserveID { get; set; }
        public string ReserveCode { get; set; }
        public DateTime ReserveDate { get; set; }
    }

    //public class AuditSeachCriteria
    //{
    //    public string AuditID { get; set; }
    //    public string AuditCode { get; set; }
    //    public string ProposalID { get; set; }
    //    public string ProposalCode { get; set; }
    //    public DateTime? FromDate { get; set; }
    //    public DateTime? ToDate { get; set; }
    //    public string DepartmentID { get; set; }
    //    public string CustomerID { get; set; }
    //}
}
