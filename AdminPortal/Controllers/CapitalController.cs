using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CapitalController : ControllerBase
    {
        // GET: api/Capital
        [HttpGet]
        public ListResponeMessage<CapitalInfo> GetAll()
        {
            ListResponeMessage<CapitalInfo> ret = new ListResponeMessage<CapitalInfo>();
            ret.data = CapitalServices.GetInstance().getAllCapital();
            ret.isSuccess = true;

            return ret;
        }

        // GET: api/Capital/getbyname?name=
        [HttpGet("getbyname")]

        public ListResponeMessage<CapitalInfo> GetByName(string name)
        {
            ListResponeMessage<CapitalInfo> ret = new ListResponeMessage<CapitalInfo>();
            ret.data = CapitalServices.GetInstance().GetByName(name);
            ret.isSuccess = true;

            return ret;
        }

        // GET: api/Capital/5
        [HttpGet("{id}")]
        public SingleResponeMessage<CapitalInfo> Get(int id)
        {
            SingleResponeMessage<CapitalInfo> ret = new SingleResponeMessage<CapitalInfo>();
            ret.item = CapitalServices.GetInstance().getCapitalbyId(id);
            ret.isSuccess = true;

            return ret;
        }

        // POST: api/Capital
        [HttpPost]
        public ActionMessage Post([FromBody] CapitalInfo _capital)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = CapitalServices.GetInstance().createCapital(_capital);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Capital/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] CapitalInfo _capital)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                CapitalServices.GetInstance().editEmloyee(id, _capital);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                CapitalServices.GetInstance().deleteCapital(id);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
    }
}
