using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AdminPortal.Entities
{
    public class ProposalTypeInfo
    {
        public int TypeID { get; set; }

        public string TypeCode { get; set; }

        public string TypeName { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public class ProposalTypeSeachCriteria
    {
        public string TypeID { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
    }
}
