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
    public class ContractController : BaseController
    {
        // GET: api/Contract
        [HttpGet]
        public ListResponeMessage<ContractInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            return GetListWithCondition(
                    "",
                    "",
              
                    0,
                    Helpers.Utils.StartOfDate(DateTime.Now),
                    Helpers.Utils.EndOfDate(DateTime.Now),
                    "",
                     10,
                     0);

        }

        // GET: api/Contract/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<ContractInfo> GetListWithCondition(
            string quoteCode, string contractCode,
         
            int customerID,
            DateTime fromDate, DateTime toDate,string _userID, int pageSize = 10, int pageIndex = 0)
        { 
            ListResponeMessage<ContractInfo> ret = new ListResponeMessage<ContractInfo>();
            try
            {
                ContractSeachCriteria _criteria = new ContractSeachCriteria();
                _criteria.QuoteCode = quoteCode;
                _criteria.ContractCode = contractCode;
                _criteria.CustomerID = customerID;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.ToDate = toDate;
                ret.isSuccess = true;
                ret.data = ContractService.GetInstance().getAllContract(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = ContractService.GetInstance().getTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Contract/5
        [HttpGet("{id}")]
        public SingleResponeMessage<NewContractInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<NewContractInfo> ret = new SingleResponeMessage<NewContractInfo>();
            try
            {
                NewContractInfo item = ContractService.GetInstance().getContractNew(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Contract found";
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

        // POST: api/Contract
        [HttpPost]
        public ActionMessage Post([FromBody] ContractInfo _contract, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ContractService.GetInstance().createContract(_contract, GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //POST: api/Contract/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] ContractInfo ContractObj, [FromForm] List<IFormFile> files, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                insetId = ContractService.GetInstance().createContract2(ContractObj, GetUserId(),_userID);
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    ret.id = insetId;
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Contract.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.Contract.ToString(), insetId.ToString(), item, documentInfo.Link);
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

        // PUT: api/Contract/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] ContractInfo _contract)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ContractService.GetInstance().editContract(id, _contract, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //PUT: api/Contract/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] ContractInfo _Contract, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ContractService.GetInstance().editContract(id, _Contract, GetUserId());
                DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Contract.ListDocument, TableFile.Contract.ToString(), id);
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Contract.ToString();
                    documentInfo.PreferId = id.ToString();
                    documentInfo.FileName = item.FileName;
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Contract.ToString(), _Contract.ContractID.ToString(), item, documentInfo.Link);
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ContractService.GetInstance().deleteContract(id,_userID);
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
        public ActionMessage DeleteAll(string contractIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ContractService.GetInstance().deleteMuti(contractIDs,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/contract/getbycode? code =
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<ContractInfo> GetByCode(string code, string _userID)
        {
            SingleResponeMessage<ContractInfo> ret = new SingleResponeMessage<ContractInfo>();
            ret.isSuccess = true;
            ret.item = ContractService.GetInstance().GetContractByCode(code,_userID);
            return ret;
        }


        [HttpGet("print/{id}")]
        public SingleResponeMessage<ContractPrintModel> GetProint(int id, string _userID)
        {
            SingleResponeMessage<ContractPrintModel> ret = new SingleResponeMessage<ContractPrintModel>();
            try
            {
                ContractPrintModel item = ContractService.GetInstance().GetNegotiationPrintModel(id,_userID);
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

        [HttpGet("getItems")]
        public ListResponeMessage<ContractSelectItem> GetContractSelectItem(string code, string _userID)
        {
            ListResponeMessage<ContractSelectItem> ret = new ListResponeMessage<ContractSelectItem>();
            try
            {
                ret.data = ContractService.GetInstance().GetContractSelectItem(code,_userID);
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
