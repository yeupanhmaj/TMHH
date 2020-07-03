using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class SurveyInfo
    {
        public int SurveyID { get; set; }

        public string SurveyCode { get; set; }

        public string SurveyName { get; set; }

        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public int ProposalType { get; set; }

        public string ProposalTypeName { get; set; }

        public string TypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public int SurveyDepartmentID { get; set; }

        public string SurveyDepartmentName { get; set; }

        public string Comment { get; set; }

        public int Solution { get; set; }
        public string ProductsName { get; set; }

        public string DepartmentComment { get; set; }

        public string Status { get; set; }
        public string SolutionText { get; set; }

        public bool IsSample { get; set; }

        public bool Valid { get; set; }

        public string ValidText { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public DateTime? DateIn { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public List<CommentInfo> ListComment { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }


        public List<ItemPropsalInfo> Items { get; set; }

        public List<ItemSurveyInfo> SurveyItems { get; set; }
    }

    public class SurveyDetailInfo
    {
        public int SurveyID { get; set; }

        public string SurveyCode { get; set; }

        public string SurveyName { get; set; }

        public int ProposalID { get; set; }

        public string ProposalCode { get; set; }

        public int ProposalType { get; set; }
        public string ProposalTypeName { get; set; }

        public string TypeName { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public int SurveyDepartmentID { get; set; }

        public string SurveyDepartmentName { get; set; }
        public string DepartmentComment { get; set; }

        public string Comment { get; set; }

        public int Solution { get; set; }

        public string Status { get; set; }

        public string SolutionText { get; set; }

        public bool IsSample { get; set; }

        public bool Valid { get; set; }

        public string ValidText { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public DateTime? DateIn { get; set; }

        public string UserU { get; set; }

        public string ProductsName { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime ProposalDate { get; set; }
        
        public List<CommentInfo> ListComment { get; set; }

        public List<DocumentInfo> ListDocument { get; set; }

        public List<ItemPropsalInfo> Items { get; set; }

        public List<ItemSurveyInfo> SurveyItems { get; set; }
    }

    public class SurveySeachCriteria
    {
        public int departmentID { get; set; }
        public string proposalCode { get; set; }
        public string SurveyID { get; set; }
        public DateTime? fromDate { get; set; } = DateTime.Parse("2000-01-01");
        public DateTime? toDate { get; set; } = DateTime.Now.AddMonths(1);
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}
