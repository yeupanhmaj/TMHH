using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AdminPortal.Entities
{
    public class DepartmentInfo
    {
        public int DepartmentID { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public int SourceID { get; set; }
    }

    public class DepartmentSeachCriteria
    {
        public string DepartmentID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
    }
}
