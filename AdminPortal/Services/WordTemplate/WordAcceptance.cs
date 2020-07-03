using AdminPortal.Entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Services.WordTemplate
{
    public class WordAcceptance
    {
        public static MemoryStream GetTemplate(int id, string path, string _userID)
        {

            var memoryStream = new MemoryStream();
            AcceptanceInfo item = AcceptanceServices.GetInstance().GetDetail(id,_userID);
            var type = item.AcceptanceType;
            string fileName = string.Empty;
            if (type == 1)
            {
                fileName = @"NghiemThu.docx";
            }
            else
            {
                fileName = @"NghiemThuSuaChua.docx";
            }

            string filePath = path + "/" + fileName;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(memoryStream);
            using (var document = WordprocessingDocument.Open(memoryStream, true))
            {
                document.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                DateTime date = DateTime.Now;
                string currentDate = "Ngày " + date.ToString("dd/MM/yyyy");
                var body = document.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();


              
                List<string> headers = new List<string>();
                List<List<string>> items = new List<List<string>>();
                headers.Add("Các công việc đã thực hiện");
                headers.Add("Đạt");
                headers.Add("Không Đạt");
                foreach (DeliveryReceiptItemInfoNew record in item.Items)
                {
                    List<string> row = new List<string>();
                    row.Add(record.ItemName);
                    if (record.AcceptanceResult)
                    {
                        row.Add("☒");
                        row.Add("☐");
                    }
                    else
                    {
                        row.Add("☐");
                        row.Add("☒");
                    }
                    items.Add(row);
                }

                string typeis1 = "☐";
                string typeis2 = "☐";
                string typeis3 = "☐";

                if(item.AcceptanceResult == 1)
                {
                     typeis1 = "☒";
                }
                if (item.AcceptanceResult == 2)
                {
                    typeis2 = "☒";
                }
                if (item.AcceptanceResult == 3)
                {
                    typeis3 = "☒";
                }

                Table tableData = CreateTableInternal(headers, items);
                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = text.Text.Replace("#", "");
                    string nameItem = string.Empty;
                    foreach (DeliveryReceiptItemInfoNew row in item.Items)
                    {
                        nameItem += row.ItemName + "\n";
                    }
                    text.Text = text.Text.Replace("datein", $"Ngày {item.CreateTime.Day} Tháng {item.CreateTime.Month} Năm {item.CreateTime.Year}");
                    text.Text = text.Text.Replace("itemName", nameItem);
                    text.Text = text.Text.Replace("departmentName", item.DepartmentName);
                    text.Text = text.Text.Replace("acceptanceNote", WordUtils.checkNull(item.AcceptanceNote));
                    text.Text = text.Text.Replace("typeis1", typeis1);
                    text.Text = text.Text.Replace("typeis2", typeis2);
                    text.Text = text.Text.Replace("typeis3", typeis3);


                    if (text.Text == "lstItem")
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

        private static Table CreateTableInternal(List<string> headers, List<List<string>> items)
        {
            int[] widths = { 8800, 1500, 1500 };




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
            TableRow tr = new TableRow();
         
            tbl.AppendChild(tr);
            List<TableCell> dataCells = new List<TableCell>();
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


            tr.Append(headerCells.ToArray());

            //// Create a new row
        
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

    }
}
