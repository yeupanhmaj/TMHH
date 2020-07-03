using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExplanationController : BaseController
    {
        // GET: api/Explanation
        [HttpGet]
        public ListResponeMessage<ExplanationInfo> GetList(int pageSize = 10, int pageIndex = 0, string _userID="")
        {
            ListResponeMessage<ExplanationInfo> ret = new ListResponeMessage<ExplanationInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = ExplanationService.GetInstance().getAllExplanation(pageSize, pageIndex,_userID);
                ret.totalRecords = ExplanationService.GetInstance().getTotalRecords(null,_userID) ;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Explanation/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<ExplanationInfo> GetListWithCondition([FromQuery] ExplanationSeachCriteria _criteria, string _userID)
        {
            ListResponeMessage<ExplanationInfo> ret = new ListResponeMessage<ExplanationInfo>();
            try
            {
              
                ret.isSuccess = true;
                ret.data = ExplanationService.GetInstance().getAllExplanationWwithCondition(_criteria,_userID);
                ret.totalRecords = ExplanationService.GetInstance().getTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Explanation/5
        [HttpGet("{id}")]
        public SingleResponeMessage<ExplanationDetailInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<ExplanationDetailInfo> ret = new SingleResponeMessage<ExplanationDetailInfo>();
            try
            {
                ExplanationDetailInfo item = ExplanationService.GetInstance().getDetailExplanation(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Explanation found";
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

        // POST: api/Explanation
        [HttpPost]
        public ActionMessage Post([FromBody] ExplanationInfo _explanation)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ExplanationService.GetInstance().createExplanation(_explanation, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Explanation/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] ExplanationInfo explanationObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await ExplanationService.GetInstance().CreateExplanation2(explanationObj, GetUserId(), files);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Explanation/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] ExplanationInfo _explanation)
        {
            ActionMessage ret = new ActionMessage();
           /* try
            {
                ret = ExplanationService.GetInstance().editExplanation(id, _explanation, getUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }*/
            return ret;
        }
        //PUT: api/Explanation/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] ExplanationInfo _explanation, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await ExplanationService.GetInstance().EditExplanation(id, _explanation, GetUserId(), files);
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
        public ActionMessage Delete(int id,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ExplanationService.GetInstance().DeleteExplanation(id, GetUserId(), _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Explanation?explanationIDs=
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ExplanationService.GetInstance().DeleteAll(ids);
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
