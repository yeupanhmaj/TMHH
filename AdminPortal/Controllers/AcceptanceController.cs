using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptanceController : BaseController
    {
        // GET: api/Acceptance
        [HttpGet]
        public ListResponeMessage<AcceptanceInfo> GetList([FromQuery]AcceptanceCriteria criteria, string _userID)
        {
            ListResponeMessage<AcceptanceInfo> ret = new ListResponeMessage<AcceptanceInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = AcceptanceServices.GetInstance().GetList(criteria,_userID);
                ret.totalRecords = AcceptanceServices.GetInstance().getTotalRecords(criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Acceptance/5
        [HttpGet("{id}")]
        public SingleResponeMessage<AcceptanceInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<AcceptanceInfo> ret = new SingleResponeMessage<AcceptanceInfo>();
            try
            {
                AcceptanceInfo item = AcceptanceServices.GetInstance().GetDetail(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no  Acceptance found";
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


        [HttpPost]
        //POST: api/Acceptance
        [RequestSizeLimit(5242880)]
        public async Task<ActionMessage> PostWithAttFile([FromForm] AcceptanceInfo obj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await AcceptanceServices.GetInstance().Create(obj, files, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Acceptance/5
        [HttpPut()]
        public async Task<ActionMessage> Put([FromForm] AcceptanceInfo obj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await AcceptanceServices.GetInstance().Update(obj, files, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Acceptance/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AcceptanceServices.GetInstance().Delete(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Acceptance/deleteall?ids= 
        [HttpDelete()]
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AcceptanceServices.GetInstance().DeleteMuti(ids);
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
