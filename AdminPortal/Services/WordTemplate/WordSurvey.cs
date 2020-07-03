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
    public  static class WordSurvey
    {
        public static MemoryStream GetTempPlate(int id, string path, out string code,string _userID)
        {
            var memoryStream = new MemoryStream();
            SurveyDetailInfo item = SurveyService.GetInstance().GetDetailSurvey(id,_userID);

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = item.ProposalCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string curDate = date.ToString("dd/MM/yyyy");
                string currentdate = "Ngày " + date.Day + " tháng " + date.Month + " năm" + date.Year;
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();
                var itemName = "";
                foreach (ItemPropsalInfo record in item.Items)
                {
                    itemName = itemName + record.ItemName + ", ";
                }
                itemName = itemName.Substring(0, itemName.Length - 1);
                List<string> headers = new List<string>();
                List<List<string>> items = new List<List<string>>();
                headers.Add("STT");
                headers.Add("Tên linh kiện, tài sản");
                headers.Add("ĐVT");
                headers.Add("Số Lượng");
                headers.Add("Ghi Chú");

                var sttIndex = 1;

                foreach (ItemSurveyInfo record in item.SurveyItems)
                {
                    List<string> row = new List<string>();
                    row.Add(sttIndex.ToString());
                    row.Add(record.ItemName);
                    row.Add(record.ItemUnit);
                    row.Add(record.ItemAmount.ToString());
                    row.Add(record.Note);
                    items.Add(row);
                    sttIndex++;
                }
                Table tableData = CreateTable(headers, items);


                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("proposalcode", WordUtils.checkNull(item.ProposalCode));
                    text.Text = text.Text.Replace("surveycode", WordUtils.checkNull(item.SurveyCode));
                    text.Text = text.Text.Replace("solution", WordUtils.checkNull(item.SolutionText));
                    text.Text = text.Text.Replace("validtext", WordUtils.checkNull(item.ValidText));
                    text.Text = text.Text.Replace("proposaldate", WordUtils.checkNull(item.ProposalDate.ToString("dd/MM/yyyy")));
                    text.Text = text.Text.Replace("curdate", WordUtils.checkNull(curDate));
                    text.Text = text.Text.Replace("currentdate", WordUtils.checkNull(currentdate));
                    text.Text = text.Text.Replace("itemname", WordUtils.checkNull(itemName));
                    text.Text = text.Text.Replace("depart", WordUtils.checkNull(item.DepartmentName));


                    if (text.Text == "lstItem")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData, textP2);
                        textP1.Remove();
                    }
                }
                List<int> Checks = new List<int>();

                Checks.Add(item.Solution - 1);

                if (item.IsSample == true)
                {
                    Checks.Add(4);
                }

                if (item.Valid == true)
                {
                    Checks.Add(5);
                }
                else
                {
                    Checks.Add(6);
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

        private static Table CreateTable(List<string> headers, List<List<string>> items)
        {
            //// Create a new table
            Table tbl = new Table();

            //// Create the table properties
            TableProperties tblProperties = new TableProperties();
            TableJustification just = new TableJustification();

            just.Val = TableRowAlignmentValues.Center;
            //// Create Table Borders
            TableBorders tblBorders = new TableBorders();
            TopBorder topBorder = new TopBorder();
            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            topBorder.Color = "000000";
            tblBorders.AppendChild(topBorder);
            BottomBorder bottomBorder = new BottomBorder();
            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            bottomBorder.Color = "000000";
            tblBorders.AppendChild(bottomBorder);
            RightBorder rightBorder = new RightBorder();
            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            rightBorder.Color = "000000";
            tblBorders.AppendChild(rightBorder);
            LeftBorder leftBorder = new LeftBorder();
            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            leftBorder.Color = "000000";
            tblBorders.AppendChild(leftBorder);
            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();
            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideHBorder.Color = "000000";
            tblBorders.AppendChild(insideHBorder);
            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();
            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideVBorder.Color = "000000";
            tblBorders.AppendChild(insideVBorder);
            //// Add the table borders to the properties
            tblProperties.AppendChild(tblBorders);
            tblProperties.AppendChild(just);
            //// Add the table properties to the table
            tbl.AppendChild(tblProperties);
            //// Add a cell to each column in the row
            ///
            List<TableCell> headerCells = new List<TableCell>();
            foreach (string itemHeaders in headers)
            {
                TableCell tcName1 = new TableCell(new Paragraph(new Run(new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                headerCells.Add(tcName1);
            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);

            foreach (List<string> rowItems in items)
            {
                List<TableCell> dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    TableCell tcName1 = new TableCell(new Paragraph(new Run(new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
            }

            return tbl;
        }
    }
}
