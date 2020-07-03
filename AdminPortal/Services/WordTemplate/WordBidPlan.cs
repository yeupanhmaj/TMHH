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
using AdminPortal.Entities;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.Helpers;


namespace AdminPortal.Services.WordTemplate
{
    public  static class WordBidPlan
    {

        public  static MemoryStream GetTemplate(int id, string path, out string code,string _userID)
        {
            double Totalcost = 0;
            var memoryStream = new MemoryStream();
            BidPlanInfo item = BidPlanService.GetInstance().getBidPlan(id,_userID);


            SearchAuditInfo audit = AuditService.GetInstance().GetAuditWordByQuoteID(item.QuoteID); 
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = item.BidPlanCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = "Ngày " + date.ToString("dd/MM/yyyy");
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                List<string> headers = new List<string>();
                List<List<string>> items = new List<List<string>>();
                headers.Add("STT");
                headers.Add("Tên gói thầu");
                headers.Add("Giá gói thầu");
                headers.Add("Nguồn vốn");
                headers.Add("Hình thức phương thức lựa chọn nhà thầu");
                headers.Add("Thời gian tổ chức lựa chọn nhà thầu");
                headers.Add("Loại hợp đồng");
                headers.Add("Thời gian thực hiện hợp đồng");

                foreach (ItemInfo record in item.Items)
                {
                    Totalcost += record.ItemPrice * record.Amount;

                }
                if (item.IsVAT)
                {
                    Totalcost = Totalcost * (item.VATNumber + 100) / 100;
                }
                string auditCodeEmpty = "         /BB-BKG";
                if (audit.AuditCode== null || audit.AuditCode=="") audit.AuditCode = auditCodeEmpty;
                List<string> row = new List<string>();
                row.Add("1");
                row.Add(item.BidName);

                row.Add(string.Format("{0:0,0}", Totalcost).Replace(",", ".") + " VNĐ");

                row.Add(WordUtils.checkNull(item.CapitalName));
                row.Add((HardData.bidMethod[item.BidMethod - 1]));
                row.Add(item.BidTime);
                row.Add(item.BidType);
                row.Add(item.BidExpirated + " " + item.BidExpiratedUnit);
                items.Add(row);

                Table tableData1 = CreateBidplan1(headers, items, string.Format("{0:0,0}", Totalcost).Replace(",", ".") + " VNĐ");
                Table tableData2 = CreateBidplan2(item , Totalcost);

                string dateInStr = "";
                string auditDateInStr = "";
                dateInStr = "ngày " + item.DateIn.Day + " tháng " + item.DateIn.Month + " năm " + item.DateIn.Year;
                auditDateInStr = "ngày " + audit.InTime.Day + " tháng " + audit.InTime.Month + " năm " + audit.InTime.Year;
                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("bidcompany", WordUtils.checkNull(item.Bid));
                    text.Text = text.Text.Replace("bidplandatein", WordUtils.checkNull(dateInStr));
                    text.Text = text.Text.Replace("currentYear ", DateTime.Now.Year.ToString());
                    text.Text = text.Text.Replace("bidname", WordUtils.checkNull(item.BidName));
                    text.Text = text.Text.Replace("costnumber", " " + string.Format("{0:0,0}", Totalcost).Replace(",", "."));
                    text.Text = text.Text.Replace("coststring", WordUtils.checkNull(Utils.NumberToTextVN((decimal)Totalcost)));
                    text.Text = text.Text.Replace("bidtime", WordUtils.checkNull(item.BidTime));
                    text.Text = text.Text.Replace("capname", WordUtils.checkNull(item.CapitalName));
                    text.Text = text.Text.Replace("bidcontracttype", WordUtils.checkNull(item.BidType));
                    text.Text = text.Text.Replace("bidExpirated", WordUtils.checkNull(item.BidExpirated + " " + item.BidExpiratedUnit));
                    text.Text = text.Text.Replace("bidmethod", WordUtils.checkNull(HardData.bidMethod[item.BidMethod - 1]));
                    text.Text = text.Text.Replace("auditdate", auditDateInStr);
                    text.Text = text.Text.Replace("auditcode", audit.AuditCode + "/BB-BKG");
                    text.Text = text.Text.Replace("location", WordUtils.checkNull(HardData.location[Int32.Parse(item.BidLocation) - 1]));
                    if (text.Text == "table1")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData1, textP2);
                        textP1.Remove();
                    }
                    if (text.Text == "table2")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData2, textP2);
                        textP1.Remove();
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
        private static Table CreateBidplan1(List<string> headers, List<List<string>> items, string Cost)
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
            TableCellProperties cellOneProperties = new TableCellProperties();
            List<TableCell> headerCells = new List<TableCell>();
            int index = 0;
            int[] widths = { 1400, 1800, 1600, 1300, 1300, 1300, 1300, 1300 };
            foreach (string itemHeaders in headers)
            {
                TableCell tcName12 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text(itemHeaders))));
                tcName12.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[index].ToString() }));
                headerCells.Add(tcName12);
                index++;
            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
            TableCell tcName1;
            foreach (List<string> rowItems in items)
            {
                index = 0;
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text( data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[index].ToString() }));
                    dataCells.Add(tcName1);
                    index++;
                }
                tr = new TableRow();
                tr.Append(dataCells.ToArray());
                tbl.AppendChild(tr);
            }

            // total row
            tr = new TableRow();
            dataCells = new List<TableCell>();

            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text("Tổng Giá"))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1400" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);

            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text())));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2200" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Continue
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);


            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text(Cost))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1800" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);


            for (int loopindex = 3; loopindex < widths.Length; loopindex++)
            {
                tcName1 = new TableCell(new Paragraph(new Run(new Text(Cost))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[loopindex].ToString() }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Continue
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);
            }
            tr.Append(dataCells.ToArray());
            tbl.AppendChild(tr);

            return tbl;
        }

        private static Table CreateBidplan2(BidPlanInfo item, double Totalcost)
        {
            //prepare ITEMs
            List<string> headers = new List<string>();
            List<List<string>> items = new List<List<string>>();
            headers.Add("STT");
            headers.Add("Tên và mô tả thiết bị");
            headers.Add("Đơn vị tính");
            headers.Add("Số lượng");
           
            headers.Add("Đơn giá(VNĐ)");
            headers.Add("Thành tiền (VNĐ)");

            int[] widths = { 1000, 3000, 1600, 1600, 2100, 2100 };


            int index = 1;
          
            foreach (ItemInfo record in item.Items)
            {
                List<string> row = new List<string>();
                row.Add(index.ToString());
                row.Add(record.ItemName);
                row.Add(record.ItemUnit);
                row.Add(record.Amount.ToString());         
                row.Add(string.Format("{0:0,0}", record.ItemPrice).Replace(",", "."));
                row.Add(string.Format("{0:0,0}", record.ItemPrice * record.Amount).Replace(",", "."));
                items.Add(row);
                index++;
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
            //if VAT
            if (item.IsVAT)
            {
                // total row
                tr = new TableRow();
                dataCells = new List<TableCell>();
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text("Tổng công (chưa bao gồm VAT)"))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1000" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                for (int loopindex = 1; loopindex < widths.Length - 1; loopindex++)
                {
                    tcName1 = new TableCell(new Paragraph());
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[loopindex].ToString() }));
                    cellOneProperties = new TableCellProperties();
                    cellOneProperties.Append(new HorizontalMerge()
                    {
                        Val = MergedCellValues.Continue
                    });
                    tcName1.Append(cellOneProperties);
                    dataCells.Add(tcName1);
                }
                double totalCostWithoutVAT = 0;
                totalCostWithoutVAT = Totalcost / ((item.VATNumber + 100 ) / 100);
                var totalCostWithoutVATstr =  string.Format("{0:0,0}", totalCostWithoutVAT).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text(totalCostWithoutVATstr))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1900" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);
                tr.Append(dataCells.ToArray());
                tbl.AppendChild(tr);

                // total row
                tr = new TableRow();
                dataCells = new List<TableCell>();
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text("Thuế VAT " + item.VATNumber.ToString() + "%"))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "200" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                for (int loopindex = 1; loopindex < widths.Length - 1; loopindex++)
                {
                    tcName1 = new TableCell(new Paragraph());
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[loopindex].ToString() }));
                    cellOneProperties = new TableCellProperties();
                    cellOneProperties.Append(new HorizontalMerge()
                    {
                        Val = MergedCellValues.Continue
                    });
                    tcName1.Append(cellOneProperties);
                    dataCells.Add(tcName1);
                }
                double vatCost = 0;
                vatCost = (Totalcost * item.VATNumber ) / 100;
                var vatCoststr = string.Format("{0:0,0}", vatCost).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text(vatCoststr))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1900" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                tr.Append(dataCells.ToArray());
                tbl.AppendChild(tr);

                // total row
                tr = new TableRow();
                dataCells = new List<TableCell>();
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text("TỔNG CỘNG (đã bao gồm VAT)"))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "200" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                for (int loopindex = 1; loopindex < widths.Length - 1; loopindex++)
                {
                    tcName1 = new TableCell(new Paragraph());
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[loopindex].ToString() }));
                    cellOneProperties = new TableCellProperties();
                    cellOneProperties.Append(new HorizontalMerge()
                    {
                        Val = MergedCellValues.Continue
                    });
                    tcName1.Append(cellOneProperties);
                    dataCells.Add(tcName1);
                }
              
                var TotalCost = string.Format("{0:0,0}", Totalcost).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text(TotalCost))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1900" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                tr.Append(dataCells.ToArray());
                tbl.AppendChild(tr);

            }
            else
            {
                // total row
                tr = new TableRow();
                dataCells = new List<TableCell>();
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text("TỔNG CỘNG "))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "200" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                for (int loopindex = 1; loopindex < widths.Length - 1; loopindex++)
                {
                    tcName1 = new TableCell(new Paragraph());
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[loopindex].ToString() }));
                    cellOneProperties = new TableCellProperties();
                    cellOneProperties.Append(new HorizontalMerge()
                    {
                        Val = MergedCellValues.Continue
                    });
                    tcName1.Append(cellOneProperties);
                    dataCells.Add(tcName1);
                }

                var TotalCost = string.Format("{0:0,0}", Totalcost).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(new Text(TotalCost))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1900" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);

                tr.Append(dataCells.ToArray());
                tbl.AppendChild(tr);
            }
            return tbl;
        }
    }
}
