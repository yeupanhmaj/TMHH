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
    public class AnalyzerGroupController : BaseController
    {
        // GET: api/AnalyzerGroup
        [HttpGet]
        public ListResponeMessage<AnalyzerGroupInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<AnalyzerGroupInfo> ret = GetListWithCondition(
                    "",
                    "",
                    "",
                    Helpers.Utils.StartOfDate(DateTime.Now),
                    Helpers.Utils.EndOfDate(DateTime.Now),
                     10,
                     0);
            return ret;
        }

        // GET: api/AnalyzerGroup/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<AnalyzerGroupInfo> GetListWithCondition(
            string AnalyzerGroupCode,
            string quoteCode,
            string contractCode,
            DateTime fromDate, DateTime toDate, int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<AnalyzerGroupInfo> ret = new ListResponeMessage<AnalyzerGroupInfo>();
            try
            {
                AnalyzerGroupSeachCriteria _criteria = new AnalyzerGroupSeachCriteria();
                _criteria.AnalyzerGroupCode = AnalyzerGroupCode;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;

                ret.isSuccess = true;
                ret.data = AnalyzerGroupService.GetInstance().getAllAnalyzerGroup(_criteria);
                ret.totalRecords = AnalyzerGroupService.GetInstance().getTotalRecords(_criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/AnalyzerGroup/5
        [HttpGet("{id}")]
        public SingleResponeMessage<AnalyzerGroupInfo> Get(int id)
        {
            SingleResponeMessage<AnalyzerGroupInfo> ret = new SingleResponeMessage<AnalyzerGroupInfo>();
            try
            {
                AnalyzerGroupInfo item = AnalyzerGroupService.GetInstance().getAnalyzerGroup(id);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no AnalyzerGroup found";
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

        // GET: api/AnalyzerGroup/getListAuditCode
        [HttpGet("getListAnalyzerGroupCode")]
        public ListResponeMessage<string> getListAnalyzerGroupCode(string AnalyzerGroupCode = "")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                ret.isSuccess = true;
                ret.data = AnalyzerGroupService.GetInstance().getListAnalyzerGroupCode(AnalyzerGroupCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/AnalyzerGroup
        [HttpPost]
        public ActionMessage Post([FromBody] AnalyzerGroupInfo _AnalyzerGroup)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AnalyzerGroupService.GetInstance().createAnalyzerGroup(_AnalyzerGroup, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/AnalyzerGroup/5
        [HttpPut("{id}")]
        public async Task<ActionMessage> Put(int id, [FromBody] AnalyzerGroupInfo _AnalyzerGroup)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await AnalyzerGroupService.GetInstance().editAnalyzerGroup(id, _AnalyzerGroup, GetUserId());
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
                ret = AnalyzerGroupService.GetInstance().Delete(id);
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
        public ActionMessage DeleteAll(string AnalyzerGroupIDs)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AnalyzerGroupService.GetInstance().DeleteMuti(AnalyzerGroupIDs);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/AnalyzerGroup/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<AnalyzerGroupInfo> GetByCode(string code)
        {
            SingleResponeMessage<AnalyzerGroupInfo> ret = new SingleResponeMessage<AnalyzerGroupInfo>();
            ret.isSuccess = true;
            ret.item = AnalyzerGroupService.GetInstance().GetAnalyzerGroupByCode(code);
            return ret;
        }
    }
}
