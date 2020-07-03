using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class AuditMemberInfo
    {
        public int MemberID { get; set; }

        public int AuditID { get; set; }

        public string MemberName { get; set; }

        public string Comment { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public class AuditMemberSeachCriteria
    {
        public string AuditID { get; set; }
        public string MemberID { get; set; }
    }
}
