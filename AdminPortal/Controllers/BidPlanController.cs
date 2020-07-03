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
    public class BidPlanController : BaseController
    {
        // GET: api/BidPlan
        [HttpGet]
        public ListResponeMessage<BidPlanInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<BidPlanInfo> ret = GetListWithCondition(                 
                    "",
                    "",
                    0,
                    Helpers.Utils.StartOfDate(DateTime.Now),
                    Helpers.Utils.EndOfDate(DateTime.Now),
                    "",
                     10,
                     0);
            return ret;
        }

        // GET: api/BidPlan/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<BidPlanInfo> GetListWithCondition(
            string bidPlanCode,
            string quoteCode,
            int customerID,
            DateTime fromDate, DateTime toDate, string _userID, int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<BidPlanInfo> ret = new ListResponeMessage<BidPlanInfo>();
            try
            {
                BidPlanSeachCriteria _criteria = new BidPlanSeachCriteria();
                _criteria.BidPlanCode = bidPlanCode;
                _criteria.QuoteCode = quoteCode;
                _criteria.CustomerID = customerID; 
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;

                ret.isSuccess = true;
                ret.data = BidPlanService.GetInstance().getAllBidPlan(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = BidPlanService.GetInstance().getTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/BidPlan/5
        [HttpGet("{id}")]
        public SingleResponeMessage<BidPlanInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<BidPlanInfo> ret = new SingleResponeMessage<BidPlanInfo>();
            try
            {
                BidPlanInfo item = BidPlanService.GetInstance().getBidPlan(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no BidPlan found";
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

        // GET: api/BidPlan/getListAuditCode
        [HttpGet("getListBidPlanCode")]
        public ListResponeMessage<string> getListBidPlanCode(string bidPlanCode = "")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                ret.isSuccess = true;
                ret.data = BidPlanService.GetInstance().getListBidPlanCode(bidPlanCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/BidPlan
        [HttpPost]
        public ActionMessage Post([FromBody] BidPlanInfo _bidPlan)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BidPlanService.GetInstance().createBidPlan(_bidPlan, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/BidPlan/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] BidPlanInfo BidPlanObj, [FromForm] List<IFormFile> files, string _userID)
        {

            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await BidPlanService.GetInstance().createBidPlan2(BidPlanObj, GetUserId(), files,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;

        }

        // PUT: api/BidPlan/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] BidPlanInfo _bidPlan)
        {
            ActionMessage ret = new ActionMessage();
            //try
            //{
            //    ret = BidPlanService.GetInstance().editBidPlan(id, _bidPlan, getUserId());
            //}
            //catch (Exception ex)
            //{
            //    ret.isSuccess = false;
            //    ret.err.msgCode = "Internal Error !!!";
            //    ret.err.msgString = ex.ToString();
            //}
            return ret;
        }

        //PUT: api/BidPlan/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] BidPlanInfo _BidPlan, [FromForm] List<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await BidPlanService.GetInstance().editBidPlan(id, _BidPlan, GetUserId(), files,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;

            //ActionMessage ret = new ActionMessage();
            //try
            //{
            //    ret = BidPlanService.GetInstance().editBidPlan(id, _BidPlan, getUserId(), files);
            //    //update list file
            //    //ProposalService.DelteDocument(id.ToString());
            //    foreach (var item in files)
            //    {
            //        DocumentInfo documentInfo = new DocumentInfo();
            //        documentInfo.TableName = "BidPlan";
            //        documentInfo.PreferId = id.ToString();
            //        documentInfo.FileName = item.FileName;
            //        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
            //        documentInfo.Length = item.Length.ToString();
            //        documentInfo.Type = item.ContentType;
            //        ret = await FilesHelpers.UploadFile("BidPlan", _BidPlan.ProposalID.ToString(), item, documentInfo.Link);
            //        DocumentService.InsertDocument(documentInfo, getUserId());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ret.isSuccess = false;
            //    ret.err.msgCode = "Internal Error !!!";
            //    ret.err.msgString = ex.ToString();
            //}
            //return ret;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BidPlanService.GetInstance().Delete(id, _userID);
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
        public ActionMessage DeleteAll(string bidPlanIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = BidPlanService.GetInstance().DeleteMuti(bidPlanIDs,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/bidPlan/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<BidPlanInfo> GetByCode(string code)
        {
            SingleResponeMessage<BidPlanInfo> ret = new SingleResponeMessage<BidPlanInfo>();
            ret.isSuccess = true;
            ret.item = BidPlanService.GetInstance().GetBidPlanByCode(code);
            return ret;
        }
    }
}
