using System;
using System.Collections.Generic;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.DataLayer;
using AdminPortal.Commons;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        // Upload File: api/Comment/addcomment
        [HttpPost("addcomment")]
        [RequestSizeLimit(5242880)]
        public async Task<ActionMessage> AddComment([FromForm] string preferId, [FromForm] string comment, [FromForm] string feature, [FromForm] List<IFormFile> files)
        {
            int insetId = -1;
            ActionMessage ret = new ActionMessage();
            try
            {
                CommentInfo commentInfo = new CommentInfo();
                commentInfo.TableName = feature;
                commentInfo.PreferId = preferId;
                commentInfo.Comment = comment;
                insetId = CommentService.GetInstance().InsertComment(commentInfo, GetUserId());
                ret.isSuccess = true;
                if (insetId > -1)
                {
                    foreach (var file in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Comment.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(file.FileName);
                        documentInfo.FileName = file.FileName;
                        documentInfo.Length = file.Length.ToString();
                        documentInfo.Type = file.ContentType;
                        ret = await FilesHelpers.UploadFile(documentInfo.TableName, insetId.ToString(), file, documentInfo.Link);
                        DocumentService.GetInstance().InsertDocument(documentInfo, GetUserId());
                    }
                }
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Lỗi thêm comment";
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
                ret = CommentService.GetInstance().deleteComment(id);
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
        public ActionMessage DeleteAll(string commentIDs)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = CommentService.GetInstance().deleteComments(commentIDs);
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
