using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Word;
using AdminPortal.Entities;
using AdminPortal.Commons;

namespace AdminPortal.Services.WordTemplate
{
    public static class WordAudit
    {
        public static MemoryStream GetTemplate(int id, string path, out string code, string _userID)
        {
            var memoryStream = new MemoryStream();
            AuditDetailInfo item = AuditService.GetInstance().getAuditInfo(id,_userID);

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = item.AuditCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = "Ngày " + date.ToString("dd/MM/yyyy");
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                Table tableData = CreateTable(item);
                var startTimeText = "Vào lúc ..... giờ ..... phút ngày " + item.StartTime.Day + " tháng " + item.StartTime.Month + " năm " + item.StartTime.Year;
                var endTimeText = "Vào lúc ..... giờ ..... phút ngày " + item.EndTime.Day + " tháng " + item.EndTime.Month + " năm " + item.EndTime.Year;
                var intime = "ngày " + item.InTime.Day + " tháng " + item.InTime.Month + " năm " + item.InTime.Year;
                foreach (var text in body.Descendants<Text>())
                {
                    string auditCode = WordUtils.checkNull(item.AuditCode);

                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("code", auditCode.Contains("/BB-BKG") ? auditCode : "Số: ......./BB-BKG");
                    text.Text = text.Text.Replace("starttime", startTimeText);
                    text.Text = text.Text.Replace("endtime", endTimeText);
                    text.Text = text.Text.Replace("location", WordUtils.checkNull(HardData.location[Int32.Parse(item.Location) - 1]));
                    text.Text = text.Text.Replace("preside", WordUtils.checkNull(item.PresideTitle + " " + item.PresideName + " - " + item.PresideRoleName));
                    text.Text = text.Text.Replace("secretary", WordUtils.checkNull(item.SecretaryTitle + " " + item.SecretaryName + " - " + item.SecretaryRoleName ));
                    /*        text.Text = text.Text.Replace("curDepart", item.CurDepartmentName);
                            text.Text = text.Text.Replace("depart", item.DepartmentName);*/
                    text.Text = text.Text.Replace("preName", item.PresideName);
                    text.Text = text.Text.Replace("secName", item.SecretaryName);
                    text.Text = text.Text.Replace("datein", intime);
                    if (text.Text == "lstItem")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData, textP2);
                        textP1.Remove();
                    }

                    if (text.Text == "firstelist")
                    {
                        text.Text = text.Text.Replace("firstelist", "");
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        foreach (AuditEmployeeInfo employ in item.Employees)
                        {
                            textP1.AppendChild(new TabChar());
                            textP1.AppendChild(new TabChar());
                           
                            textP1.AppendChild(new Text("- " + employ.Title + " " + employ.Name + " - " + employ.RoleName));
                            textP1.AppendChild(new Break());
                        }
                    }


                    if (text.Text == "endelist")
                    {
                        text.Text = text.Text.Replace("endelist", "");
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        foreach (AuditEmployeeInfo employ in item.Employees)
                        {
                            int tempLen = 50 - employ.Name.Length;
                            string dot = " :";
                            for (var i = 0; i < tempLen; i++)
                            {
                                dot += ".";
                            }
                            textP1.AppendChild(new Text(" * " + employ.Name + dot));
                            textP1.AppendChild(new Break());
                            textP1.AppendChild(new Break());
                            textP1.AppendChild(new Break());
                        }
                    }
                }

                document.Save();
                document.Close();
            }
            return memoryStream;
        }
        private static RunProperties Bold()
        {
            RunProperties runProperties = new RunProperties();

            Bold bold = new Bold();

            bold.Val = OnOffValue.FromBoolean(true);
            return runProperties;

        }


        private static Table CreateTable(AuditDetailInfo item)
        {
            //prepare ITEMs
            List<string> headers = new List<string>();
            List<List<string>> items = new List<List<string>>();
            headers.Add("Tên");
            headers.Add("Mặt hàng & dịch vụ");
            headers.Add("ĐVT");
            headers.Add("SL");
            headers.Add("Đơn giá (Đã bao gồm thuế VAT )(VNĐ)");
            headers.Add("Thành tiền (VNĐ)");

            int[] widths = { 1000, 3500, 1600, 1600, 1600, 2100 };


            int index = 1;
            bool IsVAT = false;
            double VATNumber = 0;
            foreach (QuoteAuditInfo Quoteinfo in item.Quotes)
            {
                IsVAT = Quoteinfo.IsVAT;
                VATNumber = Quoteinfo.VATNumber;
                foreach (ItemInfo record in Quoteinfo.Items)
                {
                    double VATcost = (record.ItemPrice * VATNumber) / 100;
                    List<string> row = new List<string>();
                    row.Add(index.ToString());
                    row.Add(record.ItemName);
                    row.Add(record.ItemUnit);
                    row.Add(record.Amount.ToString());
                   
                    row.Add(string.Format("{0:0,0}", (record.ItemPrice + VATcost)).Replace(",", "."));
                    row.Add(string.Format("{0:0,0}", ((record.ItemPrice + VATcost) * record.Amount)).Replace(",", "."));

                    items.Add(row);
                    index++;
                }
            }

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
            TableCell tcName1;
            foreach (string itemHeaders in headers)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                headerCells.Add(tcName1);
            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
            foreach (List<string> rowItems in items)
            {
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(Bold(), new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
            }
            TableCellProperties cellOneProperties = new TableCellProperties();

            return tbl;
        }




        public static MemoryStream GetTemplate2(int id, string path, out string code, string _userID)
        {
            var memoryStream = new MemoryStream();
            AuditDetailInfo item = AuditService.GetInstance().getAuditInfo(id,_userID);

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = item.AuditCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = "Ngày " + date.ToString("dd/MM/yyyy");
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                Table tableData = CreateTable2(item);
                var startTimeText = "Vào lúc " + item.StartTime.Hour + " giờ " + item.StartTime.Minute + " phút ngày " + item.StartTime.Day + " tháng " + item.StartTime.Month + " năm " + item.StartTime.Year;
                var endTimeText = "Vào lúc " + item.EndTime.Hour + " giờ " + item.EndTime.Minute + " phút ngày " + item.EndTime.Day + " tháng " + item.EndTime.Month + " năm " + item.EndTime.Year;
                var intime = "ngày " + item.InTime.Day + " tháng " + item.InTime.Month + " năm " + item.InTime.Year;
                foreach (var text in body.Descendants<Text>())
                {
                    string auditCode = WordUtils.checkNull(item.AuditCode);
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("code", auditCode.Contains("/BB-BKG") ? auditCode : "Số: ......./BB-BKG");
                    text.Text = text.Text.Replace("starttime", startTimeText);
                    text.Text = text.Text.Replace("endtime", endTimeText);
                    text.Text = text.Text.Replace("location", WordUtils.checkNull(HardData.location[Int32.Parse(item.Location) - 1]));
                    text.Text = text.Text.Replace("preside", WordUtils.checkNull(item.PresideTitle + " " + item.PresideName + " - " + item.PresideRoleName));
                    text.Text = text.Text.Replace("secretary", WordUtils.checkNull(item.SecretaryTitle + " " + item.SecretaryName + " - " + item.SecretaryRoleName));
                    /*        text.Text = text.Text.Replace("curDepart", item.CurDepartmentName);
                            text.Text = text.Text.Replace("depart", item.DepartmentName);*/
                    text.Text = text.Text.Replace("preName", item.PresideName);
                    text.Text = text.Text.Replace("secName", item.SecretaryName);
                    text.Text = text.Text.Replace("datein", intime);
                    if (text.Text == "lstItem")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData, textP2);
                        textP1.Remove();
                    }

                    if (text.Text == "firstelist")
                    {
                        text.Text = text.Text.Replace("firstelist", "");
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        foreach (AuditEmployeeInfo employ in item.Employees)
                        {
                            textP1.AppendChild(new Tabs());
                            textP1.AppendChild(new Tabs());
                            textP1.AppendChild(new Tabs());
                            textP1.AppendChild(new Tabs());

                            textP1.AppendChild(new Text("- " + employ.Title + " " + employ.Name + " - " + employ.RoleName));
                            textP1.AppendChild(new Break());
                        }
                    }


                    if (text.Text == "endelist")
                    {
                        text.Text = text.Text.Replace("endelist", "");
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        foreach (AuditEmployeeInfo employ in item.Employees)
                        {
                            int tempLen = 50 - employ.Name.Length;
                            string dot = " :";
                            for (var i = 0; i < tempLen; i++)
                            {
                                dot += ".";
                            }
                            textP1.AppendChild(new Text(" * " + employ.Name + dot));
                            textP1.AppendChild(new Break());
                            textP1.AppendChild(new Break());
                            textP1.AppendChild(new Break());
                        }
                    }
                }

                document.Save();
                document.Close();
            }
            return memoryStream;
        }
     
        
        private static Table CreateTable2(AuditDetailInfo item)
        {
            int index = 1;
            bool IsVAT = false;
            double VATNumber = 0;
            foreach (QuoteAuditInfo Quoteinfo in item.Quotes)
            {
                IsVAT = Quoteinfo.IsVAT;
                VATNumber = Quoteinfo.VATNumber;
             
            }
            //prepare ITEMs
            List<string> headers = new List<string>();
            List<List<string>> items = new List<List<string>>();
            headers.Add("STT");
            headers.Add("Mặt hàng & dịch vụ");
            headers.Add("ĐVT");
            headers.Add("SL");
            headers.Add("Đơn giá");
            headers.Add("Thuế VAT " + VATNumber + "%");
            headers.Add("Đơn giá (Đã bao gồm thuế " + VATNumber + "% VAT )(VNĐ)");
            headers.Add("Thành tiền (VNĐ)");

            int[] widths = { 1000, 2700, 1300, 1300, 1300, 1300 , 1300,1300 };

            foreach (QuoteAuditInfo Quoteinfo in item.Quotes)
            {         
                foreach (ItemInfo record in Quoteinfo.Items)
                {
                    double VATcost = (record.ItemPrice * VATNumber) / 100;
                    List<string> row = new List<string>();
                    row.Add(index.ToString());
                    row.Add(record.ItemName);
                    row.Add(record.ItemUnit);
                    row.Add(record.Amount.ToString());
                    row.Add(string.Format("{0:0,0}", record.ItemPrice).Replace(",", "."));
                    row.Add(string.Format("{0:0,0}", VATcost).Replace(",", "."));
                    row.Add(string.Format("{0:0,0}", (record.ItemPrice + VATcost)).Replace(",", "."));
                    row.Add(string.Format("{0:0,0}", ((record.ItemPrice + VATcost) * record.Amount)).Replace(",", "."));      
                    items.Add(row);
                    index++;
                }
            }


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
            TableCell tcName1;
            foreach (string itemHeaders in headers)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(Bold(), new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                headerCells.Add(tcName1);
            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
            foreach (List<string> rowItems in items)
            {
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(Bold(), new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
            }
            TableCellProperties cellOneProperties = new TableCellProperties();

            return tbl;
        }
    }
}

