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
    public class QuoteController : BaseController
    {
        // GET: api/Quote
        [HttpGet]
        public ListResponeMessage<QuoteInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            return GetListWithCondition(
                      "",
                      "",
                      0,
                      Helpers.Utils.StartOfDate(DateTime.Now),
                      Helpers.Utils.EndOfDate(DateTime.Now),
                       10,
                       0);
        }

        // GET: api/Quote/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<QuoteInfo> GetListWithCondition( string quoteCode = "", 
            string proposalCode = "",  int customerID = 0, DateTime? fromDate = null, 
            DateTime? toDate = null, int pageSize = 10, int pageIndex = 0, string _userID="")
        {
            ListResponeMessage<QuoteInfo> ret = new ListResponeMessage<QuoteInfo>();
            try
            {
                QuoteSeachCriteria _criteria = new QuoteSeachCriteria();
                _criteria.ProposalCode = proposalCode;
                _criteria.QuoteCode = quoteCode;
                _criteria.CustomerID = customerID;
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;

                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getAllQuote(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = ret.data.Count;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Quote/5
        [HttpGet("{id}")]
        public SingleResponeMessage<QuoteInfo> Get(int id,string _userID)
        {
            SingleResponeMessage<QuoteInfo> ret = new SingleResponeMessage<QuoteInfo>();
            ret.isSuccess = true;
            ret.item = QuoteService.GetInstance().getQuote(id, _userID);
            return ret;
        }

        // GET: api/Quote/getListQuoteCode
        [HttpGet("getListQuoteCode")]
        public ListResponeMessage<SelectItem> getListQuoteCode(string quoteCode = "", bool isContract = false)
        {
            ListResponeMessage<SelectItem> ret = new ListResponeMessage<SelectItem>();
            try
            {
                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getListQuoteCode(quoteCode , isContract);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/Quote
        [HttpPost]
        public ActionMessage Post([FromBody] QuoteInfo _quote)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = QuoteService.GetInstance().createQuote(_quote, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Quote/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] QuoteInfo QuoteObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await QuoteService.GetInstance().CreateQuoteWithAttachFIles(QuoteObj, GetUserId(), files);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
            
        }

        // PUT: api/Quote/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] QuoteInfo _quote)
        {
            ActionMessage ret = new ActionMessage();
          /*  try
            {
                ret = QuoteService.GetInstance().editQuote(id, _quote, getUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }*/
            return ret;
        }

        //PUT: api/Quote/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] QuoteInfo _quote, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = await QuoteService.GetInstance().EditQuote(id, _quote, GetUserId(), files);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;

        }

        // DELETE: api/Quote/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = QuoteService.GetInstance().Delete(id,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Quote/5
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string quoteIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = QuoteService.GetInstance().DeleteMuti(quoteIDs,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        // GET: api/Quote/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<QuoteInfo> GetByCode(string code, string _userID)
        {
            SingleResponeMessage<QuoteInfo> ret = new SingleResponeMessage<QuoteInfo>();
            ret.isSuccess = true;
            ret.item = QuoteService.GetInstance().GetQuoteDetailsBycode(code,_userID);
            return ret;
        }


        [HttpPost("createQuoteWithMutilProposal")]
        public  ActionMessage CreateQuote([FromBody] QuoteCreateRequest QuoteObj )
        {


            ActionMessage ret = new ActionMessage();
            try
            { 
                ret = QuoteService.GetInstance().CreateQuoteWithMutilProposal(QuoteObj, this.GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;

        }


        [HttpPost("getExitQuote")]
        public ListResponeMessage<int> getExitsQuote([FromBody] QuoteCreateRequest QuoteObj)
        {


            ListResponeMessage<int> ret = new ListResponeMessage<int>();
            try
            {
                ret.data = QuoteService.GetInstance().getExitsQuoteProposal(QuoteObj, this.GetUserId());
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



        [HttpPost("upload")]
        public async Task<ActionMessage> upload([FromForm] IFormFile file , int quoteID , int customerID)
        {


            ActionMessage ret = new ActionMessage();
            try
            {
               ret = await QuoteService.GetInstance().importData(file, quoteID, customerID);
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


        [HttpGet("getItems")]
        public ListResponeMessage<ItemInfo> getItems(int QuoteID, int CustomerID)
        {
            ListResponeMessage<ItemInfo> ret = new ListResponeMessage<ItemInfo>();
        
            try
            {
                ret.data = QuoteService.GetInstance().getItemsQuote(QuoteID , CustomerID);
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

        [HttpGet("getQuoteInfo")]
        public SingleResponeMessage<SelectedQuoteInfo> getQuoteInfo(int QuoteID, string _userID)
        {
            SingleResponeMessage<SelectedQuoteInfo> ret = new SingleResponeMessage<SelectedQuoteInfo>();

            try
            {
                ret.item = QuoteService.GetInstance().getSelectedItemsQuote(QuoteID,_userID);
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



        // PUT: api/Quote/5
        [HttpPut("updatenew/{id}")]
        public ActionMessage updatenew(int id, [FromBody] QuoteUpdateRequest data)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = QuoteService.GetInstance().updateQuoteNew(id, data);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Quote/5
        [HttpPut("selectQuote")]
        public ActionMessage ChooseQuote([FromBody] SelectQuoteRequest data)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = QuoteService.GetInstance().selectQuote(data.quoteID, data.customerID);
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

        [HttpGet("searchQuoteCanCreateAudit")]
        public ListResponeMessage<searchQuoteRespone> searchQuoteCanCreateAudit(string text)
        {
            ListResponeMessage<searchQuoteRespone> ret = new ListResponeMessage<searchQuoteRespone>();

            try
            {
                ret.data = QuoteService.GetInstance().searchQuoteCanCreateAudit(text);
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

        [HttpGet("searchQuoteItem")]
        public ListResponeMessage<searchProposalItemRespone> searchQuoteItem(string text)
        {
            ListResponeMessage<searchProposalItemRespone> ret = new ListResponeMessage<searchProposalItemRespone>();

            try
            {
                ret.data = QuoteService.GetInstance().searchQuoteItem(text);
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


        [HttpGet("getListquoteCodeCanCreateBiplan")]
        public ListResponeMessage<SelectItem> getListquoteCodeCanCreateBiplan(string quoteCode = "")
        {
            ListResponeMessage<SelectItem> ret = new ListResponeMessage<SelectItem>();
            try
            {
                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getListquoteCodeCanCreateBiplan(quoteCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("getListquoteCodeCanCreateNegotiation")]
        public ListResponeMessage<SelectItem> getListquoteCodeCanCreateNegotiation(string quoteCode = "")
        {
            ListResponeMessage<SelectItem> ret = new ListResponeMessage<SelectItem>();
            try
            {
                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getListquoteCodeCanCreateNegotiation(quoteCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("getListquoteCodeCanCreateDecision")]
        public ListResponeMessage<SelectItem> getListquoteCodeCanCreateDecision(string quoteCode = "")
        {
            ListResponeMessage<SelectItem> ret = new ListResponeMessage<SelectItem>();
            try
            {
                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getListquoteCodeCanCreateDecision(quoteCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("getListquoteCodeCanCreateContract")]
        public ListResponeMessage<SelectItem> getListquoteCodeCanCreateContract(string quoteCode = "")
        {
            ListResponeMessage<SelectItem> ret = new ListResponeMessage<SelectItem>();
            try
            {
                ret.isSuccess = true;
                ret.data = QuoteService.GetInstance().getListquoteCodeCanCreateContract(quoteCode);
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
