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
    public class SurveyController : BaseController
    {
        // GET: api/Survey
        [HttpGet]
        public ListResponeMessage<SurveyInfo> GetList(int pageSize = 10, int pageIndex = 0, string _userID="")
        {
            ListResponeMessage<SurveyInfo> ret = new ListResponeMessage<SurveyInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = SurveyService.GetInstance().GetAllSurvey(pageSize, pageIndex,_userID);
                ret.totalRecords = SurveyService.GetInstance().GetTotalRecords(null,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Survey/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<SurveyInfo> GetListWithCondition([FromQuery] SurveySeachCriteria _criteria, string _userID)
        {
            ListResponeMessage<SurveyInfo> ret = new ListResponeMessage<SurveyInfo>();
            try
            {
               
                ret.isSuccess = true;
                ret.data = SurveyService.GetInstance().GetAllSurveyWithCondition(_criteria,_userID);
                ret.totalRecords = SurveyService.GetInstance().GetTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public SingleResponeMessage<SurveyDetailInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<SurveyDetailInfo> ret = new SingleResponeMessage<SurveyDetailInfo>();
            try
            {
                SurveyDetailInfo item = SurveyService.GetInstance().GetDetailSurvey(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Survey found";
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

        // POST: api/Survey
        [HttpPost]
        public ActionMessage Post([FromBody] SurveyInfo _survey)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = SurveyService.GetInstance().createSurvey(_survey, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Survey/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] SurveyInfo SurveyObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret =  await SurveyService.GetInstance().createSurvey2(SurveyObj, GetUserId() , files);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Survey/5
        [HttpPut]
        public ActionMessage Put(int id, [FromBody] SurveyInfo _survey)
        {
            ActionMessage ret = new ActionMessage();
          /*  try
            {
                ret = SurveyService.GetInstance().editSurvey(id, _survey, getUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }*/
            return ret;
        }

        //PUT: api/Survey/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] SurveyInfo _Survey, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await SurveyService.GetInstance().editSurvey(id, _Survey, GetUserId(), files);
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
                ret = SurveyService.GetInstance().DeleteSurvey(id,GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Survey?surveyIDs=
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string ids)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = SurveyService.GetInstance().DeleteAll(ids);
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
