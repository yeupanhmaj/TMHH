using System;
using System.IO;
using System.Threading.Tasks;
using AdminPortal.Helpers;
using Microsoft.AspNetCore.Mvc;

using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using AdminPortal.Models.Common;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        // GET: api/Files
        [HttpGet("download")]
        public async Task<IActionResult> DownloadAsync(string feature, string preferId, string fileName)
        {
            try
            {
                var rootFolder = Utils.getRootFolder();
                var imageFolder = rootFolder + Utils.uploadFolder();
                var filePath = imageFolder + @"/" + feature + @"/" + preferId + @"/" + fileName;

                if (filePath == null)
                    return Content("filename not present");
                var memory = new MemoryStream();
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, FilesHelpers.GetContentType(filePath), Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("DownloadTemplate")]
        public async Task<IActionResult> DownloadTemplateAsync(string feature)
        {
            try
            {             
                var fileName = "";
                switch (feature)
                {
                    case "Proposal":
                        fileName = @"DeXuat.docx";
                        break;
                    case "Explanation":
                        fileName = @"GiaiTrinh.docx";
                        break;
                    case "Survey":
                        fileName = @"KhaoSat.docx";
                        break;
                    case "Audit":
                        fileName = @"HopGia.docx";
                        break;
                    case "BidPlan":
                        fileName = @"ChonThau.docx";
                        break;
                    case "Negotiation":
                        fileName = @"ThuongThao.docx";
                        break;
                    case "Decision":
                        fileName = @"QuyetDinhChonThau.docx";
                        break;
                    case "Contract":
                        fileName = @"HopDong.docx";
                        break;
                    case "DeliveryReceiptC34":
                        fileName = @"GiaoNhan34.docx";
                        break;
                    case "DeliveryReceiptC50":
                        fileName = @"GiaoNhanC50.docx";
                        break;
                    case "Acceptance":
                        fileName = @"NghiemThu.docx";
                        break;
                    default:
                        break;
                }
                var rootFolder = Utils.getRootFolder();
                var filePath = rootFolder + Utils.templateFolder() +"/" + fileName;

                if (filePath == null || System.IO.File.Exists(filePath) == false)
                    return Content("filename not present");
                var memory = new MemoryStream();
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, FilesHelpers.GetContentType(filePath), Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("DownloadDocFile")]
        public IActionResult GenerateDocxBrowser(string feature, int id, string _userID)
        {

            try
            {

                string fileName = "";
                MemoryStream ret = WordTemplateService.GetTemplate(feature, id, out fileName,_userID);
                ret.Position = 0;
                return File(ret.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
         
           
         
        }


        [HttpPost("{id}")]
        public async Task<ActionMessage> importQuote(int id ,  [FromForm] IFormFile file)
        {

            ActionMessage ret = new ActionMessage();
            try
            {

                  ret = await FileService.GetInstance().ImportQuote(id, file, GetUserId());
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
