using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPermissionController : ControllerBase
    {
        // GET: api/GroupPermission
        [HttpGet]
        public ListResponeMessage<UserGroupPermission>  Get(int groupID)
        {
            ListResponeMessage<UserGroupPermission> ret = new ListResponeMessage<UserGroupPermission>();
            try
            {
                ret.isSuccess = true;
                ret.data = UserGroupService.GetInstance().GetListGroupPermission(groupID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

       

        // POST: api/GroupPermission
        [HttpPost]
        public ActionMessage Patch(int groupID, [FromBody] List<UserGroupPermission> groupPermissions)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserGroupService.GetInstance().UpdateListPermission(groupID, groupPermissions);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


     }
}
