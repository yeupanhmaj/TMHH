using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;

namespace AdminPortal.Entities
{
    public class DocumentInfo
    {
        public int AutoID { get; set; }
        public string TableName { get; set; }
        public string PreferId { get; set; }
        public string Link { get; set; }
        public string FileName { get; set; }
        public string Length { get; set; }
        public string Type { get; set; }
        public string UserI { get; set; }
        public DateTime? InTime { get; set; }
    }
    public class DocumentSeachCriteria
    {
        public string TableName { get; set; }
        public string PreferId { get; set; }
    }
}
