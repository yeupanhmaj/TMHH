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
    public  static class WordNegotiation
    {

        public  static MemoryStream GetTemplate(int id, string path, out string code, string _userID)
        {

            double Totalcost = 0;
            var memoryStream = new MemoryStream();
            NegotiationPrintModel item = NegotiationService.GetInstance().GetNegotiationPrintModel(id,_userID);
            code = item.NegotiationCode;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
          //  code = item.ProposalCode;
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = " Ngày " + date.ToString("dd/MM/yyyy");
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

                List<string> row = new List<string>();

         

                Table tableData  = CreateTable(item , Totalcost);


                string dateInStr = "";
                string auditDateInStr = "";
                dateInStr = item.DateIn.Hour + " giờ ";
                if(item.DateIn.Minute != 0)
                {
                    dateInStr += item.DateIn.Minute + " phút ";
                }
                dateInStr += ", ngày"  + item.DateIn.Day + " tháng " + item.DateIn.Month + " năm " + item.DateIn.Year;
            //    auditDateInStr = "ngày " + item.AuditTime.Day + " tháng " + item.AuditTime.Month + " năm " + item.AuditTime.Year;
                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    text.Text = text.Text.Replace("datein", WordUtils.checkNull(dateInStr));
                    text.Text = text.Text.Replace("audittime", WordUtils.checkNull(auditDateInStr));
                    text.Text = text.Text.Replace("inputcode", WordUtils.checkNull(item.Code));
                    //A side
                    text.Text = text.Text.Replace("aaddress", WordUtils.checkNull(HardData.location[Int32.Parse(item.ALocation) - 1]));
                    text.Text = text.Text.Replace("aside", WordUtils.checkNull(item.ASide));
                    text.Text = text.Text.Replace("aphone", WordUtils.checkNull(item.APhone));
                    text.Text = text.Text.Replace("arepresent", WordUtils.checkNull(item.ARepresent));
                    text.Text = text.Text.Replace("afax", WordUtils.checkNull(item.AFax));
                    text.Text = text.Text.Replace("aposition", WordUtils.checkNull(item.APosition));
                    text.Text = text.Text.Replace("ataxcode", WordUtils.checkNull(item.ATaxCode));
                    text.Text = text.Text.Replace("abankidlabel", WordUtils.checkNull(HardData.NegotiationBankIDArr[Int32.Parse(item.ABankID) - 1]));
                    //B side
                    text.Text = text.Text.Replace("baddress", WordUtils.checkNull(item.BLocation));
                    text.Text = text.Text.Replace("bside", WordUtils.checkNull(item.BSide));
                    text.Text = text.Text.Replace("bphone", WordUtils.checkNull(item.BPhone));
                    text.Text = text.Text.Replace("brepresent", WordUtils.checkNull(item.BRepresent));
                    text.Text = text.Text.Replace("bfax", WordUtils.checkNull(item.BFax));
                    text.Text = text.Text.Replace("bposition", WordUtils.checkNull(item.BPosition));
                    text.Text = text.Text.Replace("btaxcode", WordUtils.checkNull(item.BTaxCode));
                    text.Text = text.Text.Replace("bbankidlabel", WordUtils.checkNull(item.BBankID));

                    text.Text = text.Text.Replace("costnumber", string.Format("{0:0,0}", Totalcost).Replace(",", "."));
                    text.Text = text.Text.Replace("coststring", WordUtils.checkNull(Utils.NumberToTextVN((decimal)Totalcost)));
                    text.Text = text.Text.Replace("bidtype", item.BidType);
                    text.Text = text.Text.Replace("term", WordUtils.checkNull(item.Term.ToString()));
                    text.Text = text.Text.Replace("bidtime", item.BidExpirated + " "+ item.BidExpiratedUnit);
                    if (text.Text == "table")
                    {
                        DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                        DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                        body.InsertAfter(tableData, textP2);
                        textP1.Remove();
                    }
                
                }

                document.Save();
                document.Close();
            }
            return memoryStream;
        }

        private static RunProperties FontTable()
        {
            RunProperties runProperties = new RunProperties(
                        new RunFonts()
                        {
                            Ascii = "Times New Roman",

                        });
            runProperties.FontSize = new FontSize() { Val = "26" };
            Bold bold = new Bold();

            bold.Val = OnOffValue.FromBoolean(true);
            return runProperties;

        }

        private static Table CreateTable(NegotiationPrintModel item , double Totalcost)
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

            int[] widths = { 1000, 4200, 1800, 1500, 2100, 2100 };


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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(itemHeaders))));
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
                int i = 0;
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    if (i == rowItems.Count - 1)
                    {
                        tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text(data))));
                    }
                    else
                    {
                        tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(data))));
                    }
                    tcName1.Append(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                        dataCells.Add(tcName1);

                    i++;
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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text("Tổng công (chưa bao gồm VAT)"))));
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
                totalCostWithoutVAT = Totalcost;
                Totalcost = Totalcost * (100 + item.VATNumber) / 100;
                var totalCostWithoutVATstr = string.Format("{0:0,0}", totalCostWithoutVAT).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text(totalCostWithoutVATstr))));
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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text("Thuế VAT " + item.VATNumber.ToString() + "%"))));
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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text(vatCoststr))));
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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text("TỔNG CỘNG (đã bao gồm VAT)"))));
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

                var TotalCostfm = string.Format("{0:0,0}", Totalcost).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text(TotalCostfm))));
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
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text("TỔNG CỘNG "))));
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

                var TotalCostfm = string.Format("{0:0,0}", Totalcost).Replace(",", ".");
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Right }), new Run(FontTable(), new Text(TotalCostfm))));
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
