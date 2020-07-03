using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;
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
    public class DecisionController : BaseController
    {
        // GET: api/Decision
        [HttpGet]
        public ListResponeMessage<DecisionInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            return GetListWithCondition(
                     "",
                     "",
                     0,
                     Helpers.Utils.StartOfDate(DateTime.Now),
                     Helpers.Utils.EndOfDate(DateTime.Now),
                      10,
                      0,"");
        }

        // GET: api/Decision/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<DecisionInfo> GetListWithCondition(
            string decisionCode = "", 
            string quoteCode = "", int customerID = 0, 
            DateTime? fromDate = null, DateTime? toDate = null, int pageSize = 10, int pageIndex = 0,string _userID="")
        {
            ListResponeMessage<DecisionInfo> ret = new ListResponeMessage<DecisionInfo>();
            try
            {
                DecisionSeachCriteria _criteria = new DecisionSeachCriteria();         
                _criteria.DecisionCode = decisionCode;
                _criteria.QuoteCode = quoteCode;
                _criteria.CustomerID = customerID;
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;
                ret.isSuccess = true;
                ret.data = DecisionService.GetInstance().getAllDecision(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = DecisionService.GetInstance().getTotalRecords(_criteria, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Decision/5
        [HttpGet("{id}")]
        public SingleResponeMessage<DecisionInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<DecisionInfo> ret = new SingleResponeMessage<DecisionInfo>();
            try
            {
                DecisionInfo item = DecisionService.GetInstance().GetDecision(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Decision found";
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

        // POST: api/Decision
        [HttpPost]
        public ActionMessage Post([FromBody] DecisionInfo _decision,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DecisionService.GetInstance().createDecision(_decision, GetUserId(), _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Decision/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] DecisionInfo DecisionObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                insetId = DecisionService.GetInstance().createDecision2(DecisionObj, GetUserId());
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    ret.id = insetId;
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Decision.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.Decision.ToString(), insetId.ToString(), item, documentInfo.Link);
                        DocumentService.GetInstance().InsertDocument(documentInfo, GetUserId());
                    }
                }
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // PUT: api/Decision/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] DecisionInfo _decision,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DecisionService.GetInstance().editDecision(id, _decision, GetUserId(), _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //PUT: api/Decision/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] DecisionInfo _Decision, [FromForm] List<IFormFile> files,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DecisionService.GetInstance().editDecision(id, _Decision, GetUserId(), _userID);
                //update list file
                DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Decision.ListDocument, TableFile.Decision.ToString(), id);
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Decision.ToString();
                    documentInfo.PreferId = id.ToString();
                    documentInfo.FileName = item.FileName;
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Decision.ToString(), _Decision.DecisionID.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, GetUserId());
                }
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Decision/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DecisionService.GetInstance().Delete(id, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Decision/5
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string decisionIDs,string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DecisionService.GetInstance().DeleteMuti(decisionIDs, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Decision/getListAuditCode
        [HttpGet("getListdecisionCode")]
        public ListResponeMessage<string> GetListNegotiationByCode(string code = "", string _userID="")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                ret.isSuccess = true;
                ret.data = DecisionService.GetInstance().GetListDecisionByCode(code,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        // GET: api/Decision/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<DecisionInfo> GetByCode(string code, string _userID)
        {
            SingleResponeMessage<DecisionInfo> ret = new SingleResponeMessage<DecisionInfo>();
            ret.isSuccess = true;
            ret.item = DecisionService.GetInstance().GetDecisionByCode(code,_userID);
            return ret;
        }
    }
}
