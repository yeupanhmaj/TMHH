using AdminPortal.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class AuthenticateModelRespone : ActionMessage
    {
        public string token  { get; set; }
        public string name  { get; set; }
        public string userID { get; set; }

        public string role { get; set; }
    }
}
