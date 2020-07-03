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
    public class BranchController : BaseController
    {
        [HttpGet("search")]
        public ListResponeMessage<BranchInfo> Search(string query, int page = 1, int size = 10)
        {
            ListResponeMessage<BranchInfo> ret = new ListResponeMessage<BranchInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = BranchService.GetInstance().search(query,page,size);
                ret.totalRecords = BranchService.GetInstance().totalRecordSearch(query);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("{id}")]
        public SingleResponeMessage<BranchInfo> getBranchById(int id)
        {
            SingleResponeMessage<BranchInfo> ret = new SingleResponeMessage<BranchInfo>();
            try
            {
                ret.isSuccess = true;
                ret.item = BranchService.GetInstance().getBranchById(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        [HttpGet]
        public ListResponeMessage<BranchInfo> GetList(int page = 1, int size = 10)
        {
            ListResponeMessage<BranchInfo> ret = new ListResponeMessage<BranchInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = BranchService.GetInstance().getAllBranch(page,size);
                ret.totalRecords = BranchService.GetInstance().GetTotalRecordBranch();
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpPost]
        public ActionMessage createBranch([FromBody]BranchInfo branch)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BranchService.GetInstance().createBranch(branch);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpPut]
        public SingleResponeMessage<BranchInfo> editBranch([FromBody]BranchInfo branch)
        {
            SingleResponeMessage<BranchInfo> ret = new SingleResponeMessage<BranchInfo>();
            try
            {
                ret.isSuccess = true;
                BranchService.GetInstance().editBranch(branch);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpDelete("{id}")]
        public ActionMessage deleteBranch(int id)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BranchService.GetInstance().deleteBranch(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpDelete]
        public ActionMessage deleteManyBranch(string ids)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BranchService.GetInstance().deleteManyBranch(ids);
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