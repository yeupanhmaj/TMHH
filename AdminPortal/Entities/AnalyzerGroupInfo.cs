using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AdminPortal.Entities
{
    public class AnalyzerGroupInfo
    {
        public int AnalyzerGroupID { get; set; }

        public string AnalyzerGroupCode { get; set; }

        public string AnalyzerGroupName { get; set; }

        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }


    }

    public class AnalyzerGroupSeachCriteria
    {
        public string AnalyzerGroupCode { get; set; }
        public string AnalyzerGroupName { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}

