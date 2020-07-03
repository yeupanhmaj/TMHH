using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DocumentService.GetInstance().DeleteDocument(id);
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
        [HttpDelete()]
        public ActionMessage DeleteAll(string DocumentIDs)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = DocumentService.GetInstance().DeleteDocuments(DocumentIDs);
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
