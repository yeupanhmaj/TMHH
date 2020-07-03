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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace AdminPortal.Services.WordTemplate
{
    public  static class WordDeliveryReceipt
    {
        public static MemoryStream GetTemplate(int id , string rootpath, string _userID)
        {
            var memoryStream = new MemoryStream();
            DeliveryReceiptInfo item = DeliveryReceiptServices.GetInstance().GetDetail(id,_userID);
            var type = item.DeliveryReceiptType;
            string fileName = "";
            switch (type)
            {
                case 1:
                    fileName = @"GiaoNhan34.docx";
                    break;
                case 2:
                    fileName = @"GiaoNhanC50.docx";
                    break;
                case 3:
                    fileName = @"BBGNNOIBO.docx";
                    break;
                default:
                    fileName = @"GiaoNhan34.docx";
                    break;
            }

          
            string filePath = rootpath + "/" + fileName;
    

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
          
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;

                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                List<string> headers = new List<string>();
                List<List<string>> items = new List<List<string>>();



                if (type == 1)
                {
                    headers.Add("STT");
                    headers.Add("Tên nhãn hiệu, quy cách, phẩm chất nguyên liệu, vật liệu, công cụ, dụng cụ");
                    headers.Add("Mã số");
                    headers.Add("Đơn vị tính");
                    headers.Add("Số lượng");
                    headers.Add("Đơn giá");
                    headers.Add("Thành tiền");
                    headers.Add("Ghi chú");
                    var index = 1;
                    double totalcost = 0;
                    foreach (DeliveryReceiptItemInfoNew record in item.Items)
                    {
                        List<string> row = new List<string>();
                        row.Add(index.ToString());
                        row.Add(record.ItemName);
                        row.Add("");
                        row.Add(record.ItemUnit);
                        row.Add(record.Amount.ToString());
                        row.Add(record.ItemPrice.ToString());
                        row.Add((record.Amount * record.ItemPrice).ToString());
                        row.Add(WordUtils.checkNull(record.Description));
                        items.Add(row);
                        totalcost += record.Amount * record.ItemPrice;
                        index++;
                    }
                    Table tableData = CreateTablec34(headers, items, totalcost);

                    string dateInStr = "Ngày " + item.DeliveryReceiptDate.Day + " tháng " + item.DeliveryReceiptDate.Month + " năm " + item.DeliveryReceiptDate.Year;
                    string proposalDateStr = "ngày " + item.ProposalTime.Day + " tháng " + item.ProposalTime.Month + " năm " + item.ProposalTime.Year;
                    foreach (var text in body.Descendants<Text>())
                    {
                        text.Text = text.Text.Replace("#", "");

                        text.Text = text.Text.Replace("DeliveryReceiptDate", WordUtils.checkNull(dateInStr));
                        text.Text = text.Text.Replace("DeliveryReceiptCode", WordUtils.checkNull(item.DeliveryReceiptCode));
                        text.Text = text.Text.Replace("DeliveryReceiptPlace", WordUtils.checkNull(HardData.location[Int32.Parse(item.DeliveryReceiptPlace)]));
                        text.Text = text.Text.Replace("ProposalCode", WordUtils.checkNull(item.ProposalCode));
                        text.Text = text.Text.Replace("ProposalTime", WordUtils.checkNull(proposalDateStr));
                        text.Text = text.Text.Replace("CurDepartmentName", WordUtils.checkNull(item.CurDepartmentName));
                        text.Text = text.Text.Replace("DepartmentName", WordUtils.checkNull(item.DepartmentName));
                        if (text.Text == "lstItem")
                        {
                            DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                            DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                            body.InsertAfter(tableData, textP2);
                            textP1.Remove();
                        }
                    }
                }
                else 
                {
                    if (type == 2)
                    {
                        headers.Add("STT");
                        headers.Add("Tên, ký hiệu quy cách (cấp hạng TSCĐ)");
                        headers.Add("Số hiệu TSCĐ");
                        headers.Add("Nước sản xuất");
                        headers.Add("Năm sản xuất");
                        headers.Add("Năm đưa vào sử dụng");
                        headers.Add("Đvt");
                        headers.Add("Số lượng");
                        headers.Add("Giá mua (ZSX)");
                        headers.Add("Chi phí vận chuyển");
                        headers.Add("Chi phí chạy thử");
                        headers.Add("Nguyên giá TSCĐ");
                        headers.Add("TL kỹ thuật kèm theo");
                        var index = 1;
                        double totalcost = 0;
                        var currentDate = DateTime.Now;
                        foreach (DeliveryReceiptItemInfoNew record in item.Items)
                        {
                            List<string> row = new List<string>();
                            row.Add(index.ToString());
                            row.Add(record.ItemName);
                            row.Add("");
                            row.Add("");
                            row.Add("");
                            row.Add(currentDate.Year.ToString());
                            row.Add(record.ItemUnit);
                            row.Add(record.Amount.ToString());
                            row.Add(record.ItemPrice.ToString());
                            row.Add("");
                            row.Add("");
                            row.Add((record.Amount * record.ItemPrice).ToString());
                            row.Add("");
                            items.Add(row);
                            totalcost += record.Amount * record.ItemPrice;
                            index++;
                        }
                        Table tableData = CreateTablec50(headers, items, totalcost);

                        string dateInStr = "Ngày " + item.DeliveryReceiptDate.Day + " tháng " + item.DeliveryReceiptDate.Month + " năm " + item.DeliveryReceiptDate.Year;
                        string proposalDateStr = "ngày " + item.ProposalTime.Day + " tháng " + item.ProposalTime.Month + " năm " + item.ProposalTime.Year;
                        foreach (var text in body.Descendants<Text>())
                        {
                            text.Text = text.Text.Replace("#", "");

                            text.Text = text.Text.Replace("DeliveryReceiptDate", WordUtils.checkNull(dateInStr));
                            text.Text = text.Text.Replace("DeliveryReceiptCode", WordUtils.checkNull(item.DeliveryReceiptCode));
                            text.Text = text.Text.Replace("DeliveryReceiptPlace", WordUtils.checkNull(HardData.location[Int32.Parse(item.DeliveryReceiptPlace)]));
                            text.Text = text.Text.Replace("ProposalCode", WordUtils.checkNull(item.ProposalCode));
                            text.Text = text.Text.Replace("ProposalTime", WordUtils.checkNull(proposalDateStr));
                            text.Text = text.Text.Replace("CurDepartmentName", WordUtils.checkNull(item.CurDepartmentName));
                            if (text.Text == "lstIlistItemtem")
                            {
                                DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                                DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                                body.InsertAfter(tableData, textP2);
                                textP1.Remove();
                            }
                        }
                    }
                    else
                    {
                        headers.Add("STT");
                        headers.Add("Tên mặt hàng");
                        headers.Add("Đơn vị tính");
                        headers.Add("Số lượng");
                    
                        var index = 1;
                     
                        var currentDate = DateTime.Now;
                        foreach (DeliveryReceiptItemInfoNew record in item.Items)
                        {
                            List<string> row = new List<string>();
                            row.Add(index.ToString());
                            row.Add(record.ItemName);                          
                            row.Add(record.ItemUnit);
                            row.Add(record.Amount.ToString());                         
                            index++;
                            items.Add(row);
                        }
                        Table tableData = CreateTableInternal(headers, items);

                        string dateInStr = "Ngày " + item.DeliveryReceiptDate.Day + " tháng " + item.DeliveryReceiptDate.Month + " năm " + item.DeliveryReceiptDate.Year;
                        string proposalDateStr = "ngày " + item.ProposalTime.Day + " tháng " + item.ProposalTime.Month + " năm " + item.ProposalTime.Year;
                        foreach (var text in body.Descendants<Text>())
                        {
                            text.Text = text.Text.Replace("#", "");

                            text.Text = text.Text.Replace("currentyear", WordUtils.checkNull(currentDate.Year.ToString()));
                            text.Text = text.Text.Replace("departmentName", WordUtils.checkNull(item.DepartmentName));
                            text.Text = text.Text.Replace("curDepartmentName", WordUtils.checkNull(item.CurDepartmentName));
                            if (text.Text == "lstItem")
                            {
                                DocumentFormat.OpenXml.OpenXmlElement textP1 = text.Parent;
                                DocumentFormat.OpenXml.OpenXmlElement textP2 = textP1.Parent;
                                body.InsertAfter(tableData, textP2);
                                textP1.Remove();
                            }
                        }
                    }
                }
             
                document.Save();
                document.Close();
            }
            return memoryStream;
        }


        private static Table CreateTableInternal(List<string> headers, List<List<string>> items)
        {
            int[] widths = { 1000, 5800, 1000, 1000 };




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
            int i = 0;
            foreach (string itemHeaders in headers)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[i].ToString() }));
                headerCells.Add(tcName1);
                i++;

            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
            i = 0;
            foreach (List<string> rowItems in items)
            {
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[i].ToString() }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
                i++;
            }
          
     
            return tbl;
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

        private static Table CreateTablec50(List<string> headers, List<List<string>> items, double Totalcost)
        {
            int[] widths = { 1000, 6000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 2400 , 1000 };

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
            List<string> Symbols = new List<string>();
            Symbols.Add("A");
            Symbols.Add("B");
            Symbols.Add("C");
            Symbols.Add("D");
            Symbols.Add("E");
            Symbols.Add("F");
            Symbols.Add("G");
            Symbols.Add("1");
            Symbols.Add("2");
            Symbols.Add("3");
            Symbols.Add("4");
            Symbols.Add("5");
            Symbols.Add("H");
            TableCell tcName1;
            int i = 0;
            foreach (string itemHeaders in headers)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                headerCells.Add(tcName1);
                i++;
            }


          
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();

            i = 0;

            dataCells = new List<TableCell>();
            foreach (string data in Symbols)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(data))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                dataCells.Add(tcName1);
                i++;
            }
            TableRow trsymbol = new TableRow();
            trsymbol.Append(dataCells.ToArray());
            tbl.AppendChild(trsymbol);


            

            i = 0;
            foreach (List<string> rowItems in items)
            {
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
                i++;
            }
            TableCellProperties cellOneProperties = new TableCellProperties();
            //if VAT
            tr = new TableRow();
            dataCells = new List<TableCell>();
            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text("TỔNG CỘNG "))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);

            for (int loopindex = 1; loopindex < widths.Length - 2; loopindex++)
            {
                tcName1 = new TableCell(new Paragraph());
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
                cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Continue
                });
                tcName1.Append(cellOneProperties);
                dataCells.Add(tcName1);
            }

            var TotalCostfm = string.Format("{0:0,0}", Totalcost).Replace(",", ".");
            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Left }), new Run(FontTable(), new Text(TotalCostfm))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);


            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text())));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "3000" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);

            tr.Append(dataCells.ToArray());
            tbl.AppendChild(tr);

            return tbl;
        }

        private static Table CreateTablec34(List<string> headers, List<List<string>> items, double Totalcost)
        {
            int[] widths = { 900, 3600, 900, 900, 1200, 1200, 1500, 900 };




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
            int i = 0;
            foreach (string itemHeaders in headers)
            {
                tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(itemHeaders))));
                tcName1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[i].ToString() }));
                headerCells.Add(tcName1);
                i++;
                    
            }
            //// Create a new row
            TableRow tr = new TableRow();
            tr.Append(headerCells.ToArray());
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
             i = 0;
            foreach (List<string> rowItems in items)
            {
                dataCells = new List<TableCell>();
                foreach (string data in rowItems)
                {
                    tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text(data))));
                    tcName1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = widths[i].ToString() }));
                    dataCells.Add(tcName1);
                }
                TableRow tr1 = new TableRow();
                tr1.Append(dataCells.ToArray());
                tbl.AppendChild(tr1);
                i++;
            }
            TableCellProperties cellOneProperties = new TableCellProperties();
            //if VAT
            tr = new TableRow();
            dataCells = new List<TableCell>();
            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text("TỔNG CỘNG "))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "800" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);

            for (int loopindex = 1; loopindex < widths.Length - 2; loopindex++)
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
            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Left }), new Run(FontTable(), new Text(TotalCostfm))));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1500" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);


            tcName1 = new TableCell(new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(FontTable(), new Text())));
            tcName1.Append(new TableCellProperties(
            new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "900" }));
            cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });
            tcName1.Append(cellOneProperties);
            dataCells.Add(tcName1);

            tr.Append(dataCells.ToArray());
            tbl.AppendChild(tr);

            return tbl;
        }
    }
}
