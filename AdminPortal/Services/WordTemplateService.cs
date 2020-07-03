using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Word;
using System.Linq;
using System.Data;
using AdminPortal.Entities;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.Services.WordTemplate;
using AdminPortal.Helpers;

namespace AdminPortal.Services
{
    public class WordTemplateService : BaseService<AcceptanceServices>
    {
        public static MemoryStream GetTemplate(string feature, int id, out string fileName, string _userID)
        {
            var rootFolder = Utils.getRootFolder();
            fileName = "";
            var filePath = "";
            // open xml sdk - docx
            var code = "";
            MemoryStream ret = new MemoryStream();
            switch (feature)
            {
                case "Proposal":
                    fileName = @"DeXuat.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordProposal.GetTemplate(id, filePath, out code);
                    break;
                case "Explanation":
                    fileName = @"GiaiTrinh.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordExplamation.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "Survey":
                    fileName = @"KhaoSat.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordExplamation.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "Audit":
                    fileName = @"HopGia.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordAudit.GetTemplate(id, filePath, out code,_userID);
                    break;
                case "AuditWithItemPrice":
                    fileName = @"HopGia.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordAudit.GetTemplate2(id, filePath, out code, _userID);
                    break;
                case "BidPlan":
                    fileName = @"ChonThau.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordBidPlan.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "Negotiation":
                    fileName = @"ThuongThao.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordNegotiation.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "Decision":
                    fileName = @"QuyetDinhChonThau.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordDecision.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "Contract":
                    fileName = @"HopDong.docx";
                    filePath = rootFolder + Utils.wordFolder() +"/" + fileName;
                    ret = WordContract.GetTemplate(id, filePath, out code, _userID);
                    break;
                case "DeliveryReceipt":
                    ret = WordDeliveryReceipt.GetTemplate(id, rootFolder + Utils.wordFolder(), _userID);
                    break;
            
                case "Acceptance":
                    ret = WordAcceptance.GetTemplate(id, rootFolder + Utils.wordFolder(), _userID);
                    break;
                default:
                    break;
            }
            fileName = code + "-" + fileName;
            return ret;
        }   
    }
}
