using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models.Common
{
    public class ActionMessage
    {
        public bool isSuccess { get; set; }

        public int id { get; set; }
        public ActionMessage()
        {
            err = new ErorrMssage();
        }
        public ErorrMssage err { get; set; }
    }
}
