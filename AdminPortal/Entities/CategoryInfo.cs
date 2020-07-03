using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class CategoryInfo
    {
       public int CategoryID { get; set; }
      public string CategoryCode { get; set; }
        public string CategoryQuickCode { get; set; }
        public string CategoryName { get; set; }
        public string AnalyzerCorp { get; set; }
        public double OrderFrequence { get; set; }
        public double ReceiveFrequence { get; set; }
        public double Overdue { get; set; }
        public string TransportType { get; set; }
        public string UserI { get; set; }
        public DateTime Intime { get; set; }
        public string UserU { get; set; }
        public DateTime Updatetime { get; set; }
        public int ClassID { get; set; }
    }
}
