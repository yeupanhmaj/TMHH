using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class AcceptanceInfo
    {
        public int AcceptanceID { get; set; }
        public int DeliveryReceiptID { get; set; }
        public string AcceptanceCode { get; set; }
        public string AcceptanceNote { get; set; }
        public string DeliveryReceiptCode { get; set; }
        public int ProposalID { get; set; }
        public string ProposalCode { get; set; }
        public int AcceptanceResult { get; set; }
        public string DepartmentName { get; set; }
        public string CurDepartmentName { get; set; }
        public string UserU { get; set; }
        public int AcceptanceType { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public List<CommentInfo> ListComment { get; set; }
        public List<DocumentInfo> ListDocument { get; set; }
        public List<DeliveryReceiptItemInfoNew> Items { get; set; }
    }


    public class AcceptanceCriteria
    {
        public string proposalCode { get; set; }
        public int departmentID { get; set; }
        public DateTime fromDate { get; set; } = DateTime.Parse("2000-01-01");
        public DateTime toDate { get; set; } = DateTime.Now.AddMonths(1);
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    
    }
}