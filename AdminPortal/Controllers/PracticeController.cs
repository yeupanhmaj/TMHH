using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PracticeController : ControllerBase
    {
        // GET: api/Practice
        [HttpGet]
        public ListResponeMessage<PracticeInfo> GetList([FromQuery]PracticeCriteria criteria)
        {
            ListResponeMessage<PracticeInfo> ret = new ListResponeMessage<PracticeInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = PracticeService.GetInstance().GetList(criteria);
                ret.totalRecords = PracticeService.GetInstance().getTotalRecords(criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
      
        // GET: api/Practice/313
        [HttpGet("{id}")]
        public SingleResponeMessage<PracticeInfo> Get(string id)
        {
            SingleResponeMessage<PracticeInfo> ret = new SingleResponeMessage<PracticeInfo>();
            try
            {
                PracticeInfo item = PracticeService.GetInstance().GetDetail(id);
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


        // POST: api/Practice
        [HttpPost]
        public ActionMessage Post([FromBody] PracticeInfo value)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                PracticeService.GetInstance().CreateUser(value);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Practice/5
        [HttpPut()]
        public ActionMessage Put([FromBody] PracticeInfo model)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                PracticeService.GetInstance().UpdateUser(model);
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
                PracticeService.GetInstance().DeleteUser(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpDelete()]
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                PracticeService.GetInstance().DeleteAllUser(ids);
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
