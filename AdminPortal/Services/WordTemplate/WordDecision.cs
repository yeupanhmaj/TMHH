using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using AdminPortal.Entities;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.Helpers;


namespace AdminPortal.Services.WordTemplate
{
    public  static class WordDecision
    {

        public  static MemoryStream GetTemplate(int id, string path, out string code, string _userID)
        {
            double Totalcost = 0;
            var memoryStream = new MemoryStream();

            DecisionInfo item = DecisionService.GetInstance().GetDecision(id,_userID);

            QuoteRelation relation =  ProposalService.GetInstance().getQuoteRelation(item.QuoteID);

            item.AuditCode = relation.AuditCode;
            item.AuditTime = relation.AuditTime;
            item.BidPlanCode = relation.BidPlanCode;
            item.BidPlanTime = relation.BidPlanTime;

            item.NegotiationCode = relation.NegotiationCode;
            item.NegotiationTime = relation.NegotiationTime;

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            code = "";//item.ProposalCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
               
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();
                foreach (ItemInfo record in item.Items)
                {
                    Totalcost += record.ItemPrice * record.Amount;
                }

                if(item.IsVAT)
                {
                    Totalcost = Totalcost * (item.VATNumber + 100) / 100;
                }

                Table tableData = CreateTable(item , Totalcost);

                string dateInStr = "";
                dateInStr = "ngày " + item.DateIn.Day + " tháng " + item.DateIn.Month + " năm " + item.DateIn.Year;

                string audittimeStr = "ngày " + item.AuditTime.Day + "/" + item.AuditTime.Month + "/" + item.AuditTime.Year;
                string bidplantimeStr = "ngày " + item.BidPlanTime.Day + "/" + item.BidPlanTime.Month + "/" + item.BidPlanTime.Year;
                string negotiationtimeStr = "ngày " + item.NegotiationTime.Day + "/" + item.NegotiationTime.Month + "/" + item.NegotiationTime.Year;

                string decisionCodeStr = "";
                if(item.DecisionCode != null && item.DecisionCode != "")
                {
                    decisionCodeStr = item.DecisionCode + "/QĐ-TMHH";
                }
                else
                {
                    decisionCodeStr = "...../QĐ-TMHH";
                }


                var newType = "Về việc chọn đơn vị cung cấp hàng hóa ";
                if (item.BidMethod != 1)
                {
                    newType += "theo hình thức " + HardData.bidMethod[item.BidMethod - 1];
                }
                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("auditcode", WordUtils.checkNull(item.AuditCode));
                    text.Text = text.Text.Replace("bidplancode", WordUtils.checkNull(item.BidPlanCode));
                    text.Text = text.Text.Replace("negotiationcode", WordUtils.checkNull(item.NegotiationCode));
                    text.Text = text.Text.Replace("audittime", audittimeStr);
                    text.Text = text.Text.Replace("bidplantime", bidplantimeStr);
                    text.Text = text.Text.Replace("negotiationtime", negotiationtimeStr);
                    text.Text = text.Text.Replace("newtype", newType);
                    text.Text = text.Text.Replace("datein", WordUtils.checkNull(dateInStr));
                    text.Text = text.Text.Replace("newtype", WordUtils.checkNull(dateInStr));
                    text.Text = text.Text.Replace("costnumber", string.Format("{0:0,0}", Totalcost).Replace(",", "."));
                    text.Text = text.Text.Replace("coststring", WordUtils.checkNull(Utils.NumberToTextVN((decimal)Totalcost)));
                    text.Text = text.Text.Replace("capname", WordUtils.checkNull(item.CapitalName));
                    text.Text = text.Text.Replace("bidtype", item.BidType);
                    text.Text = text.Text.Replace("bidexpired", item.BidExpirated + " " + item.BidExpiratedUnit);
                    text.Text = text.Text.Replace("customername", WordUtils.checkNull(item.CustomerName));
                    text.Text = text.Text.Replace("address", WordUtils.checkNull(item.Address));
                    text.Text = text.Text.Replace("departmentNames", WordUtils.checkNull(item.DepartmentNames));
                    text.Text = text.Text.Replace("vatnumber", WordUtils.checkNull(item.VATNumber.ToString()));
                    //   text.Text = text.Text.Replace("currentyear", );
                    text.Text = text.Text.Replace("bidmethod", HardData.bidMethod[item.BidMethod - 1]);
                    text.Text = text.Text.Replace("decisionCode", " " + WordUtils.checkNull(decisionCodeStr));
                    if (text.Text == "table")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData , textP2);
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
        private static Table CreateTable(DecisionInfo item , double Totalcost)
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

            int[] widths = { 1000, 3000, 1600, 1600, 1600, 2100 };


            int index = 1;

            foreach (ItemInfo record in item.Items)
            {
                List<string> row = new List<string>();
                row.Add(index.ToString());
                row.Add(record.ItemName);
                row.Add(record.ItemUnit);
                row.Add(record.Amount.ToString());
                row.Add(string.Format("{0:0,0}", record.ItemPrice).Replace(",", ".") );
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
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(Bold(), new Text(data))));
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
                totalCostWithoutVAT = Totalcost / ((item.VATNumber + 100) / 100);
                var totalCostWithoutVATstr = string.Format("{0:0,0}", totalCostWithoutVAT).Replace(",", ".");
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
                vatCost = (Totalcost * item.VATNumber) / 100;
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
