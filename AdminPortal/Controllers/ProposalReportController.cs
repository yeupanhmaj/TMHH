using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalReportController : BaseController
    {
        // GET: api/<ProposalReportController>
        [HttpGet]
        public ListResponeMessage<ProposalInfo> GetList([FromQuery] ProposalSeachCriteria criteria, [FromQuery] string reportType)
        {
            ListResponeMessage<ProposalInfo> ret = new ListResponeMessage<ProposalInfo>();
            try
            {
                string user = GetUserId();
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().GetAllOutdateProposal(user);//thống kê các đề xuất quá hạn
                ret.totalRecords = ProposalService.GetInstance().getTotalRecords(criteria, user);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("byDepartment")]
        public ListResponeMessage<ProposalsByDepartment> GetListByDepartment([FromQuery] ProposalSeachCriteria criteria)
        {
            ListResponeMessage<ProposalsByDepartment> ret = new ListResponeMessage<ProposalsByDepartment>();
            try
            {
                string _userID = GetUserId();
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().GetByDepartment();//thống kê các đề xuất theo phòng 
                ret.totalRecords = ProposalService.GetInstance().getTotalRecords(criteria, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        [HttpGet("isExceedReserve")]
        public ListResponeMessage<ProposalInfo> GetListExceedReserve([FromQuery] ProposalSeachCriteria criteria)
        {
            ListResponeMessage<ProposalInfo> ret = new ListResponeMessage<ProposalInfo>();
            try
            {
                string _userID = GetUserId();
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().GetAllExceedReserveProposal();//thống kê các đề xuất đã dự trù
                ret.totalRecords = ProposalService.GetInstance().getTotalRecords(criteria, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        /*[HttpGet("proccess")]
        public ListResponeMessage<ProposalRelatedData> GetProposalProccess([FromQuery] int proposalID)
        {
            ListResponeMessage<ProposalRelatedData> ret = new ListResponeMessage<ProposalRelatedData>();
            try
            {
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().GetProposalProccess(proposalID);//thống kê các đề xuất đã dự trù
             //   ret.totalRecords = ProposalService.GetInstance().getTotalRecords(criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }*/

        [HttpGet("proccess")]
        public SingleResponeMessage<TimeLineRecord> GetProposalProccess2([FromQuery] int proposalID,string _userID)
        {
            SingleResponeMessage<TimeLineRecord> ret = new SingleResponeMessage<TimeLineRecord>();
            try
            {
                string _userI = GetUserId();
                ret.isSuccess = true;
                ret.item = ProposalService.GetInstance().GetProposalProccess2(proposalID, _userI, _userID);//thống kê các đề xuất đã dự trù
                                                                            //   ret.totalRecords = ProposalService.GetInstance().getTotalRecords(criteria);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        // GET api/<ProposalReportController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/<ProposalReportController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProposalReportController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProposalReportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
