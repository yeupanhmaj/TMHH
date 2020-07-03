using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class PracticeInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }
    public class PracticeLogInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }


    public class PracticeCriteria
    {
        public String UserID { get; set; }
        public String UserName { get; set; }

        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}
