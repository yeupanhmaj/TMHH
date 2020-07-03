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
    public class EmployeeRoleController : ControllerBase
    {
        // GET: api/EmployeeRole
        [HttpGet]
        public ListResponeMessage<EmployeeRoleInfo> GetAll()
        {
            ListResponeMessage<EmployeeRoleInfo> ret = new ListResponeMessage<EmployeeRoleInfo>();
            ret.data = EmployeeRoleServices.GetInstance().getAllEmployeeRole();
            ret.isSuccess = true;

            return ret;
        }

        // GET: api/EmployeeRole/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/EmployeeRole
        [HttpPost]
        public ActionMessage Post([FromBody] EmployeeRoleInfo _employeeRole)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = EmployeeRoleServices.GetInstance().createEmployeeRole(_employeeRole);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/EmployeeRole/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] EmployeeRoleInfo _employeeRole)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                EmployeeRoleServices.GetInstance().editEmployeeRole(id, _employeeRole);
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
                EmployeeRoleServices.GetInstance().deleteEmployeeRole(id);
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
