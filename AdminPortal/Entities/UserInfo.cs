using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AdminPortal.DataLayer
{
    public class UserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int GroupID { get; set; }
        public string Email { get; set; }

        public Boolean Disable { get; set; }
    }

    public class UserLogInfo
    {
        public int id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public DateTime Time { get; set; }
    }


    public class UserCriteria
    {
        public String UserID { get; set; }
        public String UserName { get; set; }
      
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

    }
}
