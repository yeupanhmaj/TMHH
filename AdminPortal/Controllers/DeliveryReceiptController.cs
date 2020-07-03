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
    public class DeliveryReceiptController : BaseController
    {
        // GET: api/DeliveryReceipt
        [HttpGet]
        public ListResponeMessage<DeliveryReceiptInfo> GetList([FromQuery]DeliveryReceiptCriteria criteria, string _userID)
        {
            ListResponeMessage<DeliveryReceiptInfo> ret = new ListResponeMessage<DeliveryReceiptInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = DeliveryReceiptServices.GetInstance().GetList(criteria,_userID);
                ret.totalRecords = DeliveryReceiptServices.GetInstance().getTotalRecords(criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/DeliveryReceipt/5
        [HttpGet("{id}")]
        public SingleResponeMessage<DeliveryReceiptInfo> Get(int id,string _userID)
        {
            SingleResponeMessage<DeliveryReceiptInfo> ret = new SingleResponeMessage<DeliveryReceiptInfo>();
            try
            {
                DeliveryReceiptInfo item = DeliveryReceiptServices.GetInstance().GetDetail(id, _userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "không tìm thấy biên bản giao nhận";
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
        // POST: api/DeliveryReceipt
        [HttpPost]
        [RequestSizeLimit(5242880)]
        public async Task<ActionMessage> PostWithAttFile([FromForm] DeliveryReceiptInfo obj, [FromForm] List<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await DeliveryReceiptServices.GetInstance().Create(obj, files, GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/DeliveryReceipt/5
        [HttpPut()]
        public async Task<ActionMessage> Put([FromForm] DeliveryReceiptInfo obj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await DeliveryReceiptServices.GetInstance().Update(obj, files, GetUserId());
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
        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DeliveryReceiptServices.GetInstance().Delete(id,_userID);
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
        public ActionMessage DeleteAll(string ids, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DeliveryReceiptServices.GetInstance().DeleteMuti(ids,_userID);
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
