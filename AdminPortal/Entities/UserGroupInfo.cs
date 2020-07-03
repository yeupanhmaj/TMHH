using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AdminPortal.DataLayer
{
    public class UserGroupInfo
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
    public class UserGroupCriteria
    {
        public String GroupName { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
    public class UserGroupPermission
    {
        public int GroupID { get; set; }     
        public int Feature { get; set; }
    }

    public class UserGroupUser
    {
        public String UserID { get; set; }
        public int GroupID { get; set; }
    }
}
