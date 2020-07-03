using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Entities;
using AdminPortal.Services;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        // GET: api/Department
        [HttpGet]
        public ListResponeMessage<DepartmentInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<DepartmentInfo> ret = new ListResponeMessage<DepartmentInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = DepartmentService.GetInstance().getAllDepartment(pageSize, pageIndex);
                ret.totalRecords = DepartmentService.GetInstance().getTotalRecords(ret.data);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public SingleResponeMessage<DepartmentInfo> Get(int id)
        {
            SingleResponeMessage<DepartmentInfo> ret = new SingleResponeMessage<DepartmentInfo>();
            ret.isSuccess= true;
            ret.item = DepartmentService.GetInstance().getDepartmentbyId(id);
            return ret;
        }

        //// POST: api/Department
        //[HttpPost]
        //public ActionMessage Post([FromBody] DepartmentInfo _department)
        //{
        //    ActionMessage ret = new ActionMessage();
        //    try
        //    {
        //        ret = DepartmentService.GetInstance().createDepartment(_department);
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.isSuccess = false;
        //        ret.err.msgCode = "001";
        //        ret.err.msgString = ex.ToString();
        //    }
        //    return ret;
        //}

        //// PUT: api/Department/5
        //[HttpPut("{id}")]
        //public ActionMessage Put(int id, [FromBody] DepartmentInfo _department)
        //{
        //    ActionMessage ret = new ActionMessage();
        //    try
        //    {
        //        DepartmentService.GetInstance().editDepartment(id, _department);
        //        ret.isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.isSuccess = false;
        //        ret.err.msgCode = "001";
        //        ret.err.msgString = ex.ToString();
        //    }
        //    return ret;
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public ActionMessage Delete(int id)
        //{
        //    ActionMessage ret = new ActionMessage();
        //    try
        //    {
        //        DepartmentService.GetInstance().deleteDepartment(id);
        //        ret.isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.isSuccess = false;
        //        ret.err.msgCode = "001";
        //        ret.err.msgString = ex.ToString();
        //    }
        //    return ret;
        //}
    }
}
