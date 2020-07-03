using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class CommentInfo
    {
        public int AutoID { get; set; }
        public string TableName { get; set; }
        public string PreferId { get; set; }
        public string Comment { get; set; }
        public string UserI { get; set; }
        public string UserName { get; set; }
        public DateTime? Intime { get; set; }
        public List<DocumentInfo> ListDocument { get; set; }
}
    public class CommentSeachCriteria
    {
        public string TableName { get; set; }
        public string PreferId { get; set; }
    }
}
