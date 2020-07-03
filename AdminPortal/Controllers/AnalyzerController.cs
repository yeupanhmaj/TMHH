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
    public class AnalyzerController :  BaseController
    {
        // GET: api/Analyzer
        [HttpGet]
        public ListResponeMessage<AnalyzerInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<AnalyzerInfo> ret = GetListWithCondition(
                    "",
                    "",
                    "",
                    Helpers.Utils.StartOfMonth(DateTime.Now),
                    Helpers.Utils.EndOfDate(DateTime.Now),
                     pageSize,
                     pageIndex);
            return ret;
        }

        // GET: api/Analyzer/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<AnalyzerInfo> GetListWithCondition(
            string analyzerCode, string departmentID, string customerID,
            DateTime fromDate, DateTime toDate, int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<AnalyzerInfo> ret = new ListResponeMessage<AnalyzerInfo>();
            try
            {
                AnalyzerSeachCriteria _criteria = new AnalyzerSeachCriteria();
                _criteria.AnalyzerCode = analyzerCode;
                _criteria.DepartmentID = departmentID;
                _criteria.CustomerID = customerID;
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;

            
                ret.data = AnalyzerService.GetInstance().getAllAnalyzer(pageSize, pageIndex, _criteria);
                ret.totalRecords = AnalyzerService.GetInstance().getTotalRecords(_criteria);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Analyzer/5
        [HttpGet("{id}")]
        public SingleResponeMessage<AnalyzerInfo> Get(int id)
        {
            SingleResponeMessage<AnalyzerInfo> ret = new SingleResponeMessage<AnalyzerInfo>();
            try
            {
                AnalyzerInfo item = AnalyzerService.GetInstance().getAnalyzer(id);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Analyzer found";
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
        [HttpGet("analyzerByDate")]
        public ListResponeMessage<AnalyzerInfo> GetAnalyzerByDate([FromQuery] AnalyzerSeachCriteria _criteria)
        {
            ListResponeMessage<AnalyzerInfo> ret = new ListResponeMessage<AnalyzerInfo>();
            try
            {
                List<AnalyzerInfo> item = AnalyzerService.GetInstance().GetAnalyzerByDate(_criteria);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Analyzer found";
                    return ret;
                }
                ret.data = item;
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
        // GET: api/Analyzer/getListAuditCode
        [HttpGet("getListAnalyzerCode")]
        public ListResponeMessage<string> getListAnalyzerCode(string AnalyzerCode = "")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                ret.isSuccess = true;
                ret.data = AnalyzerService.GetInstance().getListAnalyzerCode(AnalyzerCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/Analyzer
        [HttpPost]
        public ActionMessage Post([FromBody] AnalyzerInfo _Analyzer)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AnalyzerService.GetInstance().createAnalyzer(_Analyzer, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Analyzer/5
        [HttpPut("{id}")]
         public async Task<ActionMessage> Put(int id, [FromBody] AnalyzerInfo _Analyzer)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await AnalyzerService.GetInstance().editAnalyzer(id, _Analyzer, GetUserId());
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
        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AnalyzerService.GetInstance().Delete(id);
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
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string AnalyzerIDs)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AnalyzerService.GetInstance().DeleteMuti(AnalyzerIDs);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Analyzer/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<AnalyzerInfo> GetByCode(string code)
        {
            SingleResponeMessage<AnalyzerInfo> ret = new SingleResponeMessage<AnalyzerInfo>();
            ret.isSuccess = true;
            ret.item = AnalyzerService.GetInstance().GetAnalyzerByCode(code);
            return ret;
        }
    }
}
