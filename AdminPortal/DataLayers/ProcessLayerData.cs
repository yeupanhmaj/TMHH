using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using AdminPortal.Helpers;
namespace AdminPortal.DataLayer
{

    /// <summary>
    /// Lớp này chứa các hàm thường dùng kho code form
    /// </summary>
    public class ProcessServices
    {
        static DataProvider db = new DataProvider();
        public static DateTime GetServerDate(SqlConnection connection)
        {
            return DateTime.Parse(db.GetDataTable(connection, "select getdate()", CommandType.Text).Rows[0][0].ToString());
        }
       /* public static void LoadHeader(SqlConnection connection, ReportClass rp)
        {
            DataTable dt = db.GetDataTable(connection, "select AutoID, CompanyName, Address, Phone, Fax, Email, Website, UserI, Intime from tbl_Header where Checked = 1", CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                //strCompanyName = r["CompanyName"].ToString();
                //strAddress = r["Address"].ToString();
                //strPhone = r["Phone"].ToString();
                //strFax = r["Fax"].ToString();
                //strEmail = r["Email"].ToString();
                //strWebsite = r["Website"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtCompanyName"]).Text = r["CompanyName"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtAddress"]).Text = r["Address"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtPhone"]).Text = (r["Phone"].ToString() == "" ? "" : "Điện thoại: " + r["Phone"].ToString()) + (r["Fax"].ToString() == "" ? "" : " - Fax: " + r["Fax"].ToString());
            }
        }
        public static void LoadHeader(SqlConnection connection, ReportClass rp, string _DepartmentCode)
        {
            DataTable dt = db.GetDataTable(connection, "select AutoID, CompanyName, Address, Phone, Fax, Email, Website, UserI, Intime from tbl_Header where DepartmentCode='" + Utils.ConvertString(_DepartmentCode) + "'", CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                //strCompanyName = r["CompanyName"].ToString();
                //strAddress = r["Address"].ToString();
                //strPhone = r["Phone"].ToString();
                //strFax = r["Fax"].ToString();
                //strEmail = r["Email"].ToString();
                //strWebsite = r["Website"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtCompanyName"]).Text = r["CompanyName"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtAddress"]).Text = r["Address"].ToString();
                ((TextObject)rp.ReportDefinition.ReportObjects["txtPhone"]).Text = (r["Phone"].ToString() == "" ? "" : "Điện thoại: " + r["Phone"].ToString()) + (r["Fax"].ToString() == "" ? "" : " - Fax: " + r["Fax"].ToString());
            }
        }
*/

        public static bool CheckFuntion(string functionID, string functionCompare)
        {
            if (Strings.InStr(1, functionID, functionCompare, CompareMethod.Text) > 0)
                return true;
            else
                return false;
        }
        ///// <summary>
        ///// Hàm đọc tất cả máy in trong máy tính lên ListBox
        ///// </summary>
        ///// <param name="lstPrinter">ListBox</param>
        //public static void LoadPrinterName(ListBox lstPrinter)
        //{
        //    foreach (string printerName in PrinterSettings.InstalledPrinters)
        //    {
        //        lstPrinter.Items.Add(printerName);
        //    }
        //}
        /// <summary>
        /// Hàm thực hiện in report
        /// </summary>
        /// <param name="_report">Report cần in</param>
        /// <param name="_printerName">Tên máy in</param>
        /// <param name="_nCopies">Số lần in</param>
        /// <param name="_startPageN">Trang đầu</param>
        /// <param name="_endPageN">Trang kết thúc</param>
     /*   public static void PrintReport(ReportClass _report, string _printerName, int _nCopies, int _startPageN, int _endPageN)
        {
            _report.PrintOptions.PrinterName = _printerName;
            _report.PrintToPrinter(_nCopies, false, _startPageN, _endPageN);
        }*/
        /// <summary>
        /// Hàm ghi ra file log báo lỗi chương trình
        /// </summary>
        /// <param name="msg">Câu thông báo lỗi</param>
        public static void RecordError(string msg)
        {
            FileStream fs = new FileStream(@"..\..\Error\Err_" + DateTime.Now.ToString("ddMMyy") + ".txt", FileMode.OpenOrCreate);
            fs.Seek(0, SeekOrigin.End);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(msg);
            sw.Close();
            fs.Close();
        }

       
        /// <summary>
        /// Hàm trả về chuỗi giới tính
        /// </summary>
        /// <param name="_sex">Truyền vào mã giới tính</param>
        /// <returns>Trả về giới tính</returns>
        public static string GetSexToLable(string _sex)
        {
            string _sexCaption = "";
            if (_sex.ToUpper() == "M")
                _sexCaption = "(Nam)";
            else if (_sex.ToUpper() == "F")
                _sexCaption = "(Nữ)";
            else
                _sexCaption = "(Chưa rõ)";

            return _sexCaption;
        }
        
        public static double Round(double value, int digits)
        {

            if ((digits < -15) || (digits > 15))

                throw new ArgumentOutOfRangeException("digits", "Rounding digits must be between -15 and 15, inclusive.");



            if (digits >= 0)

                return Math.Round(value, digits);



            double n = Math.Pow(10, -digits);

            return Math.Round(value / n, 0) * n;

        }

        public static decimal Round(decimal d, int decimals)
        {

            if ((decimals < -28) || (decimals > 28))

                throw new ArgumentOutOfRangeException("decimals", "Rounding decimals must be between -28 and 28, inclusive.");



            if (decimals >= 0)

                return decimal.Round(d, decimals);



            decimal n = (decimal)Math.Pow(10, -decimals);

            return decimal.Round(d / n, 0) * n;

        }
    }
    public class CallReportName
    {
        //public static void CallReport(ReportClass rp, DataTable dt, CrystalReportViewer rpW)
        //{
        //    rp.SetDataSource(dt);
        //    rpW.ReportSource = rp;
        //}
    }
    public class ReadNumberToCharactor
    {
        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string ReadNumber(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }
    }

    public class VNCurrency
    {
        public static string ToString(decimal number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str + " ";
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            str = str.Trim() + " đồng chẵn";
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public static string ToString(double number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = "";
            bool booAm = false;
            double decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDouble(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str + " ";
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            str = str.Trim() + " đồng chẵn";
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
