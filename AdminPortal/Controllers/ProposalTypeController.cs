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
    public class ProposalTypeController : BaseController
    {
        // GET: api/ProposalType
        [HttpGet]
        public ListResponeMessage<ProposalTypeInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<ProposalTypeInfo> ret = new ListResponeMessage<ProposalTypeInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = ProposalTypeService.GetInstance().getAllProposalType(pageSize, pageIndex);
                ret.totalRecords = ProposalTypeService.GetInstance().getTotalRecords(ret.data);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/ProposalType/5
        [HttpGet("{id}", Name = "Get")]
        public SingleResponeMessage<ProposalTypeInfo> Get(int id)
        {
            SingleResponeMessage<ProposalTypeInfo> ret = new SingleResponeMessage<ProposalTypeInfo>();
            ret.isSuccess = true;
            ret.item = ProposalTypeService.GetInstance().getProposalTypebyId(id);
            return ret;
        }

        // POST: api/ProposalType
        [HttpPost]

        public ActionMessage Post([FromBody] ProposalTypeInfo _proposalType)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                
                ret = ProposalTypeService.GetInstance().createProposalType(_proposalType, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        // PUT: api/ProposalType/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] ProposalTypeInfo _department)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ProposalTypeService.GetInstance().editProposalType(id, _department, GetUserId());
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
                ret = ProposalTypeService.GetInstance().deleteProposalType(id);
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
