using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class ExplanationInfo
    {
        public int ExplanationID { get; set; }

        public string ExplanationName { get; set; }

        public string ExplanationCode { get; set; }

        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public int ProposalType { get; set; }

        public string TypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }
        public string ProductsName { get; set; }
        public bool Necess { get; set; }

        public bool Suitable { get; set; }

        public string NBNum { get; set; }

        public string XNNum { get; set; }

        public string Available { get; set; }

        public bool IsAvailable { get; set; }

        public string Comment { get; set; }

        public string TNCB { get; set; }

        public string DBLTCN { get; set; }

        public string NVHTTB { get; set; }

        public string DTNL { get; set; }

        public string NQL { get; set; }

        public string HQKTXH { get; set; }
        public string Status { get; set; }
        public string UserI { get; set; }
        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<ItemPropsalInfo> Items { get; set; }
    }

    public class ExplanationDetailInfo
    {
        public int ExplanationID { get; set; }

        public string ExplanationName { get; set; }

        public string ExplanationCode { get; set; }

        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public int ProposalType { get; set; }

        public string TypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public bool Necess { get; set; }

        public bool Suitable { get; set; }

        public string NBNum { get; set; }

        public string XNNum { get; set; }

        public string Available { get; set; }

        public string Status { get; set; }

        public bool IsAvailable { get; set; }

        public string Comment { get; set; }

        public string TNCB { get; set; }

        public string DBLTCN { get; set; }

        public string NVHTTB { get; set; }

        public string DTNL { get; set; }

        public string NQL { get; set; }

        public string HQKTXH { get; set; }
        public string ProductsName { get; set; }
        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<ItemPropsalInfo> Items { get; set; }


    }

    public class ExplanationSeachCriteria
    {
        public string proposalCode { get; set; }
        public int departmentID { get; set; }
        public DateTime fromDate { get; set; } = DateTime.Parse("2000-01-01");
        public DateTime toDate { get; set; } = DateTime.Now.AddMonths(1);
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public string ExplanationID { get; set; }
    }
}
