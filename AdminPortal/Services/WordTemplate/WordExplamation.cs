using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Word;
using AdminPortal.Entities;

namespace AdminPortal.Services.WordTemplate
{
    public  static class WordExplamation
    {
        public static MemoryStream GetTemplate(int id, string path, out string code, string _userID)
        {
            var memoryStream = new MemoryStream();
            ExplanationDetailInfo item = ExplanationService.GetInstance().getDetailExplanation(id,_userID);
            // var index = item.ProposalType - 1;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = item.ProposalCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = "Ngày " + date.ToString("dd/MM/yyyy");
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                var itemName = "";
                foreach (ItemPropsalInfo record in item.Items)
                {
                    itemName = itemName + record.ItemName + ", ";
                }
                itemName = itemName.Substring(0, itemName.Length - 1);

                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("itemname", WordUtils.checkNull(itemName));
                    text.Text = text.Text.Replace("nbNumber", WordUtils.checkNull(item.NBNum));
                    text.Text = text.Text.Replace("xnNumber", WordUtils.checkNull(item.XNNum));
                    text.Text = text.Text.Replace("numberMachine", WordUtils.checkNull(item.Available.ToString()));
                    text.Text = text.Text.Replace("explainationcode", WordUtils.checkNull(item.ExplanationCode));
                    text.Text = text.Text.Replace("reason", WordUtils.checkNull(item.Comment));
                    text.Text = text.Text.Replace("tncb", WordUtils.checkNull(item.TNCB));
                    text.Text = text.Text.Replace("dbltcn", WordUtils.checkNull(item.DBLTCN));
                    text.Text = text.Text.Replace("nvh", WordUtils.checkNull(item.NVHTTB));
                    text.Text = text.Text.Replace("dtnl", WordUtils.checkNull(item.DTNL));
                    text.Text = text.Text.Replace("nql", WordUtils.checkNull(item.NQL));
                    text.Text = text.Text.Replace("hqktcxh", WordUtils.checkNull(item.HQKTXH));
                }
                List<int> Checks = new List<int>();
                if (item.Necess == true)
                {
                    Checks.Add(0);
                }
                else
                {
                    Checks.Add(1);
                }

                if (item.Suitable == true)
                {
                    Checks.Add(2);
                }
                else
                {
                    Checks.Add(3);
                }

                if (item.IsAvailable == true)
                {
                    Checks.Add(4);
                }
                else
                {
                    Checks.Add(5);
                }
                int i = 0;
                foreach (SdtContentCheckBox ctrl in body.Descendants<SdtContentCheckBox>())
                {
                    if (Checks.IndexOf(i) > -1)
                    {
                        ctrl.Checked.Val = OnOffValues.One;
                        ctrl.Parent.Parent.Descendants<Run>().First().GetFirstChild<Text>().Text = "☒";

                    }
                    i++;
                }
                document.Save();
                document.Close();
            }
            return memoryStream;
        }

      
    }
}
