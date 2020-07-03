using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersGroupController : BaseController
    {
        // GET: api/UsersGroup
        [HttpGet]
        public ListResponeMessage<UserGroupInfo> GetList([FromQuery]UserGroupCriteria criteria)
        {
            ListResponeMessage<UserGroupInfo> ret = new ListResponeMessage<UserGroupInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = UserGroupService.GetInstance().GetList(criteria);
                ret.totalRecords = UserGroupService.GetInstance().getTotalRecords(criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/UsersGroup/5
        [HttpGet("{id}")]
        public SingleResponeMessage<UserGroupInfo> Get(int id)
        {
            SingleResponeMessage<UserGroupInfo> ret = new SingleResponeMessage<UserGroupInfo>();
            try
            {
                UserGroupInfo item = UserGroupService.GetInstance().GetDetail(id);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no  User found";
                    return ret;
                }
                ret.item = item;
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/UsersGroup
        [HttpPost]
        public ActionMessage Post([FromBody] UserGroupInfo _user)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserGroupService.GetInstance().CreateUserGroup(_user);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/UsersGroup/5
        [HttpPut("{id}")]
        public ActionMessage Put(string id, [FromBody] UserGroupInfo  userGroup)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserGroupService.GetInstance().UpdateUserGroup(id , userGroup);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/UsersGroup/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(string id)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserGroupService.GetInstance().DeleteUserGroup(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/UsersGroup/listgroup
        [HttpGet("listgroup")]
        public ListResponeMessage<UserGroupInfo> GetListGroupsOfUser(string userID)
        {
            ListResponeMessage<UserGroupInfo> ret = new ListResponeMessage<UserGroupInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = UserGroupService.GetInstance().GetListGroupOfUser(userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/UsersGroup/updatelistgroup
        [HttpPost("updatelistgroup")]
        public ActionMessage  UpdateListGroupsOfUser(string userID, [FromBody] List<UserGroupUser> lstGroup)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserGroupService.GetInstance().UpdateGroupOfUser(userID, lstGroup);
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
