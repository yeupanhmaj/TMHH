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
    public class AuditController : BaseController
    {
        // GET: api/Audit
        [HttpGet]
        public ListResponeMessage<SearchAuditInfo> GetList(int pageSize = 10, int pageIndex = 0)
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

        // GET: api/Audit/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<SearchAuditInfo> GetListWithCondition(
            string auditCode = "",
             string quoteCode = "",
            int CustomerID = 0, DateTime? fromDate = null,
            DateTime? toDate = null, int pageSize = 10, int pageIndex = 0,string _userID="")
        {
            ListResponeMessage<SearchAuditInfo> ret = new ListResponeMessage<SearchAuditInfo>();
            try
            {
                AuditSeachCriteria _criteria = new AuditSeachCriteria();
                _criteria.CustomerID = CustomerID;
                _criteria.AuditCode = auditCode;
                _criteria.QuoteCode = quoteCode;
                _criteria.FromDate = fromDate;
                _criteria.ToDate = toDate;
                _criteria.pageSize = pageSize;
                _criteria.pageIndex = pageIndex;
                ret.isSuccess = true;
                ret.data = AuditService.GetInstance().getAllAudit(pageSize, pageIndex, _criteria,_userID);
                ret.totalRecords = AuditService.GetInstance().getTotalRecords(_criteria,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Audit/5
        [HttpGet("{id}")]
        public SingleResponeMessage<AuditDetailInfo> Get(int id, string _userID)
        {
            SingleResponeMessage<AuditDetailInfo> ret = new SingleResponeMessage<AuditDetailInfo>();
            try
            {
                AuditDetailInfo item = AuditService.GetInstance().getAuditInfo(id,_userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no Audit found";
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

        // GET: api/Quote/getListAuditCode
        [HttpGet("getListAuditCode")]
        public ListResponeMessage<string> getListAuditCode(string auditCode = "")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                ret.isSuccess = true;
                ret.data = AuditService.GetInstance().getListAuditCode(auditCode);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

      

        //POST: api/Audit/withDFile
        [RequestSizeLimit(5242880)]
        [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile([FromForm] AuditInfo AuditObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                insetId = AuditService.GetInstance().createAudit2(AuditObj, GetUserId());
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    ret = AuditService.GetInstance().insertEmployees(insetId, AuditObj.Employees);
                    if (ret.isSuccess == false)
                    {
                        return ret;
                    }
                    ret.id = insetId;
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Audit.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.Audit.ToString(), insetId.ToString(), item, documentInfo.Link);
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

     

        //PUT: api/Audit/5/withDFile
        [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] AuditInfo _Audit, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AuditService.GetInstance().editAudit(id, _Audit, GetUserId());
                if (ret.isSuccess == false) return ret;
                //++edit item
                ret = AuditService.GetInstance().EditEmployees(id, _Audit.Employees);
                if (ret.isSuccess == false) return ret;

                //update list file
                 DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_Audit.Documents, TableFile.Audit.ToString(), id);
                //add new document
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Audit.ToString();
                    documentInfo.PreferId = id.ToString();
                    documentInfo.FileName = item.FileName;
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Audit.ToString(), _Audit.AuditID.ToString(), item, documentInfo.Link);
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
                ret = AuditService.GetInstance().Delete(id,_userID);
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
        public ActionMessage DeleteAll(string auditIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AuditService.GetInstance().DeleteMuti(auditIDs,_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        // GET: api/Audit/getbycode?code=
        // [Authorize]
        [HttpGet("getbycode")]
        public SingleResponeMessage<AuditDetailInfo> GetByCode(string code, string _userID)
        {
            SingleResponeMessage<AuditDetailInfo> ret = new SingleResponeMessage<AuditDetailInfo>();
            ret.isSuccess = true;
            ret.item = AuditService.GetInstance().GetAuditByCode(code,_userID);
            return ret;
        }



        // GET: api/Audit/getbycode?code=
        // [Authorize]
        [HttpGet("GetDefaultMember")]
        public ListResponeMessage<AuditEmployeeInfo> GetDefaultMember()
        {
            ListResponeMessage<AuditEmployeeInfo> ret = new ListResponeMessage<AuditEmployeeInfo>();
        
            try
            {
                ret.isSuccess = true;
                ret.data = AuditService.GetInstance().GetAuditDefaultMember();
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }




        // POST: api/Audit
        [HttpPost]
        public ActionMessage Post([FromBody] AuditInfo _audit)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                insetId = AuditService.GetInstance().createAudit2(_audit, GetUserId());
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    ret = AuditService.GetInstance().insertEmployees(insetId, _audit.Employees);
                    if (ret.isSuccess == false)
                    {
                        return ret;
                    }
                    ret.id = insetId;
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


        // PUT: api/Audit/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromBody] AuditInfo _Audit)
        {

            ActionMessage ret = new ActionMessage();
            try
            {
                ret = AuditService.GetInstance().editAudit(id, _Audit, GetUserId());
                if (ret.isSuccess == false) return ret;
                //++edit item
                ret = AuditService.GetInstance().EditEmployees(id, _Audit.Employees);
                if (ret.isSuccess == false) return ret;
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
