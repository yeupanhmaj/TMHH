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
    public class UsersController : BaseController
    {
        // GET: api/Users
        [HttpGet]
        public ListResponeMessage<UserInfo> GetList([FromQuery]UserCriteria criteria)
        {
            ListResponeMessage<UserInfo> ret = new ListResponeMessage<UserInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = UserService.GetInstance().GetList(criteria);
                ret.totalRecords = UserService.GetInstance().getTotalRecords(criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public SingleResponeMessage<UserInfo> Get(string id)
        {
            SingleResponeMessage<UserInfo> ret = new SingleResponeMessage<UserInfo>();
            try
            {
                UserInfo item = UserService.GetInstance().GetDetail(id);
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

        // POST: api/Users
        [HttpPost]
        public ActionMessage Post([FromBody] UserInfo _user)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                  UserService.GetInstance().CreateUser(_user);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionMessage Put(string id, [FromBody] UserInfo _user)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserService.GetInstance().UpdateUser(id , _user);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(string id)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                 UserService.GetInstance().DeleteUser(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/ApiWithActions/5  [HttpDelete()]
        [HttpDelete()]
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserService.GetInstance().DeleteUsers(ids);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/Users
        [HttpPost("changepass")]
        public ActionMessage ChangePass([FromBody] string newPassword)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                UserService.GetInstance().ChangePassword(this.GetUserId(), newPassword);
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
