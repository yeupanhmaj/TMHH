using System;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET: api/Customer
        [HttpGet]
        public ListResponeMessage<CustomerInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<CustomerInfo> ret = new ListResponeMessage<CustomerInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = CustomerService.GetInstance().getAllCustomer(pageSize, pageIndex);
                ret.totalRecords = CustomerService.GetInstance().getTotalRecords(ret.data);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public SingleResponeMessage<CustomerInfo> Get(int id)
        {
            SingleResponeMessage<CustomerInfo> ret = new SingleResponeMessage<CustomerInfo>();
            ret.isSuccess = true;
            ret.item = CustomerService.GetInstance().getCustomerbyId(id);
            return ret;
        }

        // GET: api/Customer/bybidplancode?bidplancode=
        [HttpGet("bybidplancode")]
        public SingleResponeMessage<CustomerInfo> GetByBidPlanCode(string bidPlanCode)
        {
            SingleResponeMessage<CustomerInfo> ret = new SingleResponeMessage<CustomerInfo>();
            ret.isSuccess = true;
            ret.item = CustomerService.GetInstance().getCustomerbyId(bidPlanCode);
            return ret;
        }

        //[HttpPost]
        //// POST: api/Customer
        //public void Post([FromBody] string value)
        //{

        //}

        //// PUT: api/Customer/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
