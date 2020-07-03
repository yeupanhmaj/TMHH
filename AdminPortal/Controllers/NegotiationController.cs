using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using AdminPortal.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiationController : BaseController
    {
        // GET: api/Negotiation
        [HttpGet]
        public ListResponeMessage<NegotiationInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ListResponeMessage<NegotiationInfo> ret = new ListResponeMessage<NegotiationInfo>();
            try
            {            
                ret = GetListWithCondition(                 
                    "",
                    "",
                    0,
                    Helpers.Utils.StartOfDate(DateTime.Now),
                    Helpers.Utils.EndOfDate(DateTime.Now),
                     10,
                     0);
               
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Negotiation/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<NegotiationInfo> GetListWithCondition(
            string negotiationCode ,
            string quoteCode,
            int customerID , 
            DateTime fromDate  , DateTime toDate , int pageSize = 10, int pageIndex = 0, string _userID="")
        {
            ListResponeMessage<NegotiationInfo> ret = new ListResponeMessage<NegotiationInfo>();
            try
            {
                NegotiationSeachCriteria _criteria = new NegotiationSeachCriteria();
                _criteria.QuoteCode = quoteCode;
                _criteria.NegotiationCode = negotiationCode;                      
                _criteria.CustomerID = customerID;
                _criteria.FromDate = Helpers.Utils.StartOfDate(fromDate);
                _criteria.ToDate = Helpers.Utils.EndOfDate(toDate);
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;

                ret.isSuccess = true;
                ret.data = NegotiationService.GetInstance().getAllNegotiation(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = NegotiationService.GetInstance().getTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Negotiation/5
        [HttpGet("{id}")]
        public SingleResponeMessage<NegotiationInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<NegotiationInfo> ret = new SingleResponeMessage<NegotiationInfo>();
            try
            {
                NegotiationInfo item = NegotiationService.GetInstance().GetNegotiation(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Negotiation found";
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

        // GET: api/Negotiation/getListAuditCode
       
        // POST: api/Negotiation
        [HttpPost]
        public ActionMessage Post([FromBody] NegotiationInfo _negotiation)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = NegotiationService.GetInstance().createNegotiation(_negotiation, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Negotiation/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] NegotiationInfo NegotiationObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                insetId = NegotiationService.GetInstance().createNegotiation2(NegotiationObj, GetUserId());
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    ret.id = insetId;
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Negotiation.ToString() ;
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.Negotiation.ToString(), insetId.ToString(), item, documentInfo.Link);
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

        // PUT: api/Negotiation/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] NegotiationInfo _negotiation, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = NegotiationService.GetInstance().editNegotiation(id, _negotiation, GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //PUT: api/Negotiation/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] NegotiationInfo _Negotiation, [FromForm] List<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = NegotiationService.GetInstance().editNegotiation(id, _Negotiation, GetUserId(),_userID);
                //update list file

                DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Negotiation.ListDocument, TableFile.Negotiation.ToString(), id);
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Negotiation.ToString();
                    documentInfo.PreferId = id.ToString();
                    documentInfo.FileName = item.FileName;
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Negotiation.ToString(), _Negotiation.NegotiationID.ToString(), item, documentInfo.Link);
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

        // DELETE: api/Negotiation/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = NegotiationService.GetInstance().Delete(id,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // DELETE: api/Negotiation/5
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string negotiationIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = NegotiationService.GetInstance().DeleteMuti(negotiationIDs,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        // GET: api/Negotiation/getListAuditCode
        [HttpGet("getListNegotiationByCode")]
        public ListResponeMessage<NegotiationLstcb> GetListNegotiationByCode(string code = "")
        {
            ListResponeMessage<NegotiationLstcb> ret = new ListResponeMessage<NegotiationLstcb>();
            try
            {
                ret.isSuccess = true;
                ret.data = NegotiationService.GetInstance().GetListNegotiationByCode(code);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Negotiation/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<NegotiationInfo> GetByCode(string code)
        {
            SingleResponeMessage<NegotiationInfo> ret = new SingleResponeMessage<NegotiationInfo>();
            ret.isSuccess = true;
            ret.item = NegotiationService.GetInstance().GetNegotiationByCode(code);
            return ret;
        }


        [HttpGet("print/{id}")]
        public SingleResponeMessage<NegotiationPrintModel> GetPrint(int id, string _userID)
        {
            SingleResponeMessage<NegotiationPrintModel> ret = new SingleResponeMessage<NegotiationPrintModel>();
            try
            {
                NegotiationPrintModel item = NegotiationService.GetInstance().GetNegotiationPrintModel(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Negotiation found";
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

    }
}
