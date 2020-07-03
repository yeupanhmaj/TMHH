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
    public class EmployeeController : BaseController
    {
        // GET: api/Employee
        [HttpGet]
        public ListResponeMessage<EmployeeInfo> GetAll()
        {
            ListResponeMessage<EmployeeInfo> ret = new ListResponeMessage<EmployeeInfo>();
            ret.data = EmployeeServices.GetInstance().getAllEmployee();
            ret.isSuccess = true;

            return ret;
        }

        [HttpGet("GetListEmployee")]
        public ListResponeMessage<EmployeeInfo> GetListEmployee(string name = "")
        {
            ListResponeMessage<EmployeeInfo> ret = new ListResponeMessage<EmployeeInfo>();
            try
            {
                ret.data = EmployeeServices.GetInstance().GetEmployeesByCondition(name);
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

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public SingleResponeMessage<EmployeeInfo> Get(int id)
        {
            SingleResponeMessage<EmployeeInfo> ret = new SingleResponeMessage<EmployeeInfo>();
            ret.item = EmployeeServices.GetInstance().getEmployeebyId(id);
            ret.isSuccess = true;

            return ret;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionMessage Post([FromBody] EmployeeInfo _employee)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = EmployeeServices.GetInstance().createEmployee(_employee, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] EmployeeInfo _employee)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                EmployeeServices.GetInstance().editEmloyee(id, _employee, GetUserId());
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
                EmployeeServices.GetInstance().deleteEmployee(id);
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
