using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Helpers
{
    public class Utils
    {
        public static DateTime StartOfDate(DateTime date)
        { 
            return  new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
        public static DateTime EndOfDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime StartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        public static string getRootFolder()
        {
            // return Directory.GetCurrentDirectory();
            return @"./";
        }

        public static string uploadFolder()
        {
            return  @"/FileUpload";
        }
        public static string wordFolder()
        {
            return @"/Words";
        }
        public static string templateFolder()
        {
            // return Directory.GetCurrentDirectory();
            return @"/Template";
        }
      
        public static string ConvertString(string Str)
        {
            string strConvert = "";
            for (int i = 1; i <= Str.Length; i++)
            {
                if (Strings.Mid(Str, i, 1).Equals("'"))
                {
                    strConvert += "''";
                }
                else
                {
                    strConvert += Strings.Mid(Str, i, 1);
                }
            }

            return strConvert;
        }
        /// <summary>
        /// Hàm dùng để chuyển tuổi ra năm
        /// </summary>
        /// <param name="_age"></param>
        /// <returns></returns>
        public static string ConvertBirthYear(string _age)
        {
            if (string.IsNullOrEmpty(_age)) _age = "0";
            if (int.Parse(_age) < 1000 && int.Parse(_age) != 0)
                _age = (DateTime.Now.Year - int.Parse(_age)).ToString();

            return _age;
        }
        public static string ChuyenTVKhongDau(string strVietNamese)
        {
            string FindText = "áàảãạâấầẩẫậăắằẳẵặđèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1, index2;
            while ((index = strVietNamese.IndexOfAny(FindText.ToCharArray())) != -1)
            {
                index2 = FindText.IndexOf(strVietNamese[index]);
                strVietNamese = strVietNamese.Replace(strVietNamese[index], ReplText[index2]);
            }
            return strVietNamese;
        }
        /// <summary>
        /// _TypeCOnvert=1 (Convert from UNI), _TypeCOnvert=2 (Convert from VNI), _TypeCOnvert=3 (Convert from ABC)
        /// </summary>
        /// <param name="_Source"></param>
        /// <param name="_TypeConvert"></param>
        /// <returns></returns>
        public static string ConvertStringText(string _Source, int _TypeConvert)
        {
            string _ConvertResult = "";
            string _Temp = "";

            #region UNI
            if (_TypeConvert == 1)
            {
                for (int i = 1; i <= _Source.Length; i++)
                {
                    _Temp = Strings.Mid(_Source, i, 1);
                    switch (_Temp)
                    {
                        case "á"://1
                            _ConvertResult += "a";
                            break;
                        case "à"://2
                            _ConvertResult += "a";
                            break;
                        case "ả"://3
                            _ConvertResult += "a";
                            break;
                        case "ã"://4
                            _ConvertResult += "a";
                            break;
                        case "ạ"://5
                            _ConvertResult += "a";
                            break;
                        case "ă"://6
                            _ConvertResult += "a";
                            break;
                        case "ắ"://7
                            _ConvertResult += "a";
                            break;
                        case "ằ"://8
                            _ConvertResult += "a";
                            break;
                        case "ẳ"://9
                            _ConvertResult += "a";
                            break;
                        case "ẵ"://10
                            _ConvertResult += "a";
                            break;
                        case "ặ"://11
                            _ConvertResult += "a";
                            break;
                        case "â"://12
                            _ConvertResult += "a";
                            break;
                        case "ấ"://13
                            _ConvertResult += "a";
                            break;
                        case "ầ"://14
                            _ConvertResult += "a";
                            break;
                        case "ẩ"://15
                            _ConvertResult += "a";
                            break;
                        case "ẫ"://16
                            _ConvertResult += "a";
                            break;
                        case "ậ"://17
                            _ConvertResult += "a";
                            break;
                        case "é"://18
                            _ConvertResult += "e";
                            break;
                        case "è"://19
                            _ConvertResult += "e";
                            break;
                        case "ẻ"://20
                            _ConvertResult += "e";
                            break;
                        case "ẽ"://21
                            _ConvertResult += "e";
                            break;
                        case "ẹ"://22
                            _ConvertResult += "e";
                            break;
                        case "ê"://23
                            _ConvertResult += "e";
                            break;
                        case "ế"://24
                            _ConvertResult += "e";
                            break;
                        case "ề"://25
                            _ConvertResult += "e";
                            break;
                        case "ể"://26
                            _ConvertResult += "e";
                            break;
                        case "ễ"://27
                            _ConvertResult += "e";
                            break;
                        case "ệ"://28
                            _ConvertResult += "e";
                            break;
                        case "ó"://29
                            _ConvertResult += "o";
                            break;
                        case "ò"://30
                            _ConvertResult += "o";
                            break;
                        case "ỏ"://31
                            _ConvertResult += "o";
                            break;
                        case "õ"://32
                            _ConvertResult += "o";
                            break;
                        case "ọ"://33
                            _ConvertResult += "o";
                            break;
                        case "ô"://34
                            _ConvertResult += "o";
                            break;
                        case "ố"://35
                            _ConvertResult += "o";
                            break;
                        case "ồ"://36
                            _ConvertResult += "o";
                            break;
                        case "ổ"://37
                            _ConvertResult += "o";
                            break;
                        case "ỗ"://38
                            _ConvertResult += "o";
                            break;
                        case "ộ"://39
                            _ConvertResult += "o";
                            break;
                        case "ơ"://40
                            _ConvertResult += "o";
                            break;
                        case "ớ"://41
                            _ConvertResult += "o";
                            break;
                        case "ờ"://42
                            _ConvertResult += "o";
                            break;
                        case "ở"://43
                            _ConvertResult += "o";
                            break;
                        case "ỡ"://44
                            _ConvertResult += "o";
                            break;
                        case "ợ"://45
                            _ConvertResult += "o";
                            break;
                        case "ú"://46
                            _ConvertResult += "u";
                            break;
                        case "ù"://47
                            _ConvertResult += "u";
                            break;
                        case "ủ"://48
                            _ConvertResult += "u";
                            break;
                        case "ũ"://49
                            _ConvertResult += "u";
                            break;
                        case "ụ"://50
                            _ConvertResult += "u";
                            break;
                        case "ư"://51
                            _ConvertResult += "u";
                            break;
                        case "ứ"://52
                            _ConvertResult += "u";
                            break;
                        case "ừ"://53
                            _ConvertResult += "u";
                            break;
                        case "ử"://54
                            _ConvertResult += "u";
                            break;
                        case "ữ"://55
                            _ConvertResult += "u";
                            break;
                        case "ự"://56
                            _ConvertResult += "u";
                            break;
                        case "ý"://57
                            _ConvertResult += "y";
                            break;
                        case "ỳ"://58
                            _ConvertResult += "y";
                            break;
                        case "ỷ"://59
                            _ConvertResult += "y";
                            break;
                        case "ỹ"://60
                            _ConvertResult += "y";
                            break;
                        case "ỵ"://61
                            _ConvertResult += "y";
                            break;
                        case "Á"://62
                            _ConvertResult += "A";
                            break;
                        case "À"://63
                            _ConvertResult += "A";
                            break;
                        case "Ả"://64
                            _ConvertResult += "A";
                            break;
                        case "Ã"://65
                            _ConvertResult += "A";
                            break;
                        case "Ạ"://66
                            _ConvertResult += "A";
                            break;
                        case "Ă"://67
                            _ConvertResult += "A";
                            break;
                        case "Ắ"://68
                            _ConvertResult += "A";
                            break;
                        case "Ằ"://69
                            _ConvertResult += "A";
                            break;
                        case "Ẳ"://70
                            _ConvertResult += "A";
                            break;
                        case "Ẵ"://71
                            _ConvertResult += "A";
                            break;
                        case "Ặ"://72
                            _ConvertResult += "A";
                            break;
                        case "Â"://73
                            _ConvertResult += "A";
                            break;
                        case "Ấ"://74
                            _ConvertResult += "A";
                            break;
                        case "Ầ"://75
                            _ConvertResult += "A";
                            break;
                        case "Ẩ"://76
                            _ConvertResult += "A";
                            break;
                        case "Ẫ"://77
                            _ConvertResult += "A";
                            break;
                        case "Ậ"://78
                            _ConvertResult += "A";
                            break;
                        case "É"://79
                            _ConvertResult += "E";
                            break;
                        case "È"://80
                            _ConvertResult += "E";
                            break;
                        case "Ẻ"://81
                            _ConvertResult += "E";
                            break;
                        case "Ẽ"://82
                            _ConvertResult += "E";
                            break;
                        case "Ẹ"://83
                            _ConvertResult += "E";
                            break;
                        case "Ê"://84
                            _ConvertResult += "E";
                            break;
                        case "Ế"://85
                            _ConvertResult += "E";
                            break;
                        case "Ề"://86
                            _ConvertResult += "E";
                            break;
                        case "Ể"://87
                            _ConvertResult += "E";
                            break;
                        case "Ễ"://88
                            _ConvertResult += "E";
                            break;
                        case "Ệ"://89
                            _ConvertResult += "E";
                            break;
                        case "Ó"://90
                            _ConvertResult += "O";
                            break;
                        case "Ò"://91
                            _ConvertResult += "O";
                            break;
                        case "Ỏ"://92
                            _ConvertResult += "O";
                            break;
                        case "Õ"://93
                            _ConvertResult += "O";
                            break;
                        case "Ọ"://94
                            _ConvertResult += "O";
                            break;
                        case "Ô"://95
                            _ConvertResult += "O";
                            break;
                        case "Ố"://96
                            _ConvertResult += "O";
                            break;
                        case "Ồ"://97
                            _ConvertResult += "O";
                            break;
                        case "Ổ"://98
                            _ConvertResult += "O";
                            break;
                        case "Ỗ"://99
                            _ConvertResult += "O";
                            break;
                        case "Ộ"://100
                            _ConvertResult += "O";
                            break;
                        case "Ơ"://101
                            _ConvertResult += "O";
                            break;
                        case "Ớ"://102
                            _ConvertResult += "O";
                            break;
                        case "Ờ"://103
                            _ConvertResult += "O";
                            break;
                        case "Ở"://104
                            _ConvertResult += "O";
                            break;
                        case "Ỡ"://105
                            _ConvertResult += "O";
                            break;
                        case "Ợ"://106
                            _ConvertResult += "O";
                            break;
                        case "Ú"://107
                            _ConvertResult += "U";
                            break;
                        case "Ù"://108
                            _ConvertResult += "U";
                            break;
                        case "Ủ"://109
                            _ConvertResult += "U";
                            break;
                        case "Ũ"://110
                            _ConvertResult += "U";
                            break;
                        case "Ụ"://111
                            _ConvertResult += "U";
                            break;
                        case "Ư"://112
                            _ConvertResult += "U";
                            break;
                        case "Ứ"://113
                            _ConvertResult += "U";
                            break;
                        case "Ừ"://114
                            _ConvertResult += "U";
                            break;
                        case "Ử"://115
                            _ConvertResult += "U";
                            break;
                        case "Ữ"://116
                            _ConvertResult += "U";
                            break;
                        case "Ự"://117
                            _ConvertResult += "U";
                            break;
                        case "Ý"://118
                            _ConvertResult += "U";
                            break;
                        case "Ỳ"://119
                            _ConvertResult += "Y";
                            break;
                        case "Ỷ"://120
                            _ConvertResult += "Y";
                            break;
                        case "Ỹ"://121
                            _ConvertResult += "Y";
                            break;
                        case "í"://122
                            _ConvertResult += "i";
                            break;
                        case "ì"://123
                            _ConvertResult += "i";
                            break;
                        case "ỉ"://124
                            _ConvertResult += "i";
                            break;
                        case "ĩ"://125
                            _ConvertResult += "i";
                            break;
                        case "ị"://126
                            _ConvertResult += "i";
                            break;
                        case "Ỵ"://127
                            _ConvertResult += "Y";
                            break;
                        case "Í"://128
                            _ConvertResult += "I";
                            break;
                        case "Ì"://129
                            _ConvertResult += "I";
                            break;
                        case "Ỉ"://130
                            _ConvertResult += "I";
                            break;
                        case "Ĩ"://131
                            _ConvertResult += "I";
                            break;
                        case "Ị"://132
                            _ConvertResult += "I";
                            break;
                        case "Đ"://133
                            _ConvertResult += "D";
                            break;
                        case "đ"://134
                            _ConvertResult += "d";
                            break;
                        default:
                            _ConvertResult += _Temp;
                            break;
                    }
                }

                return _ConvertResult;
            }
            #endregion

            #region VNI
            else if (_TypeConvert == 2)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Pos", typeof(int));
                dt.Columns.Add("Source", typeof(string));
                dt.Columns.Add("Convert", typeof(string));
                //134 row
                dt.Rows.Add(1, "aù", "a");
                dt.Rows.Add(2, "aø", "a");
                dt.Rows.Add(3, "aû", "a");
                dt.Rows.Add(4, "aõ", "a");
                dt.Rows.Add(5, "aï", "a");
                dt.Rows.Add(6, "aê", "a");
                dt.Rows.Add(7, "aé", "a");
                dt.Rows.Add(8, "aè", "a");
                dt.Rows.Add(9, "aú", "a");
                dt.Rows.Add(10, "aü", "a");
                dt.Rows.Add(11, "aë", "a");
                dt.Rows.Add(12, "aâ", "a");
                dt.Rows.Add(13, "aá", "a");
                dt.Rows.Add(14, "aà", "a");
                dt.Rows.Add(15, "aå", "a");
                dt.Rows.Add(16, "aã", "a");
                dt.Rows.Add(17, "aä", "a");
                dt.Rows.Add(18, "eù", "e");
                dt.Rows.Add(19, "eø", "e");
                dt.Rows.Add(20, "eû", "e");
                dt.Rows.Add(21, "eõ", "e");
                dt.Rows.Add(22, "eï", "e");
                dt.Rows.Add(23, "eâ", "e");
                dt.Rows.Add(24, "eá", "e");
                dt.Rows.Add(25, "eà", "e");
                dt.Rows.Add(26, "eå", "e");
                dt.Rows.Add(27, "eã", "e");
                dt.Rows.Add(28, "eä", "e");
                dt.Rows.Add(29, "où", "o");
                dt.Rows.Add(30, "oø", "o");
                dt.Rows.Add(31, "oû", "o");
                dt.Rows.Add(32, "oõ", "o");
                dt.Rows.Add(33, "oï", "o");
                dt.Rows.Add(34, "oâ", "o");
                dt.Rows.Add(35, "oá", "o");
                dt.Rows.Add(36, "oà", "o");
                dt.Rows.Add(37, "oå", "o");
                dt.Rows.Add(38, "oã", "o");
                dt.Rows.Add(39, "oä", "o");
                dt.Rows.Add(40, "ô", "o");
                dt.Rows.Add(41, "ôù", "o");
                dt.Rows.Add(42, "ôø", "o");
                dt.Rows.Add(43, "ôû", "o");
                dt.Rows.Add(44, "ôõ", "o");
                dt.Rows.Add(45, "ôï", "o");
                dt.Rows.Add(46, "uù", "u");
                dt.Rows.Add(47, "uø", "u");
                dt.Rows.Add(48, "uû", "u");
                dt.Rows.Add(49, "uõ", "u");
                dt.Rows.Add(50, "uï", "u");
                dt.Rows.Add(51, "ö", "u");
                dt.Rows.Add(52, "öù", "u");
                dt.Rows.Add(53, "öø", "u");
                dt.Rows.Add(54, "öû", "u");
                dt.Rows.Add(55, "öõ", "u");
                dt.Rows.Add(56, "öï", "u");
                dt.Rows.Add(57, "yù", "y");
                dt.Rows.Add(58, "yø", "y");
                dt.Rows.Add(59, "yû", "y");
                dt.Rows.Add(60, "yõ", "y");
                dt.Rows.Add(61, "î", "y");
                dt.Rows.Add(62, "AÙ", "A");
                dt.Rows.Add(63, "AØ", "A");
                dt.Rows.Add(64, "AÛ", "A");
                dt.Rows.Add(65, "AÕ", "A");
                dt.Rows.Add(66, "AÏ", "A");
                dt.Rows.Add(67, "AÊ", "A");
                dt.Rows.Add(68, "AÉ", "A");
                dt.Rows.Add(69, "AÈ", "A");
                dt.Rows.Add(70, "AÚ", "A");
                dt.Rows.Add(71, "AÜ", "A");
                dt.Rows.Add(72, "AË", "A");
                dt.Rows.Add(73, "AÂ", "A");
                dt.Rows.Add(74, "AÁ", "A");
                dt.Rows.Add(75, "AÀ", "A");
                dt.Rows.Add(76, "AÅ", "A");
                dt.Rows.Add(77, "AÃ", "A");
                dt.Rows.Add(78, "AÄ", "A");
                dt.Rows.Add(79, "EÙ", "E");
                dt.Rows.Add(80, "EØ", "E");
                dt.Rows.Add(81, "EÛ", "E");
                dt.Rows.Add(82, "EÕ", "E");
                dt.Rows.Add(83, "EÏ", "E");
                dt.Rows.Add(84, "EÂ", "E");
                dt.Rows.Add(85, "EÁ", "E");
                dt.Rows.Add(86, "EÀ", "E");
                dt.Rows.Add(87, "EÅ", "E");
                dt.Rows.Add(88, "EÃ", "E");
                dt.Rows.Add(89, "EÄ", "E");
                dt.Rows.Add(90, "OÙ", "O");
                dt.Rows.Add(91, "OØ", "O");
                dt.Rows.Add(92, "OÛ", "O");
                dt.Rows.Add(93, "OÕ", "O");
                dt.Rows.Add(94, "OÏ", "O");
                dt.Rows.Add(95, "OÂ", "O");
                dt.Rows.Add(96, "OÁ", "O");
                dt.Rows.Add(97, "OÀ", "O");
                dt.Rows.Add(98, "OÅ", "O");
                dt.Rows.Add(99, "OÃ", "O");
                dt.Rows.Add(100, "OÄ", "O");
                dt.Rows.Add(101, "Ô", "O");
                dt.Rows.Add(102, "ÔÙ", "O");
                dt.Rows.Add(103, "ÔØ", "O");
                dt.Rows.Add(104, "ÔÛ", "O");
                dt.Rows.Add(105, "ÔÕ", "O");
                dt.Rows.Add(106, "ÔÏ", "O");
                dt.Rows.Add(107, "UÙ", "U");
                dt.Rows.Add(108, "UØ", "U");
                dt.Rows.Add(109, "UÛ", "U");
                dt.Rows.Add(110, "UÕ", "U");
                dt.Rows.Add(111, "UÏ", "U");
                dt.Rows.Add(112, "Ö", "U");
                dt.Rows.Add(113, "ÖÙ", "U");
                dt.Rows.Add(114, "ÖØ", "U");
                dt.Rows.Add(115, "ÖÛ", "U");
                dt.Rows.Add(116, "ÖÕ", "U");
                dt.Rows.Add(117, "ÖÏ", "U");
                dt.Rows.Add(118, "YÙ", "Y");
                dt.Rows.Add(119, "YØ", "Y");
                dt.Rows.Add(120, "YÛ", "Y");
                dt.Rows.Add(121, "YÕ", "Y");
                dt.Rows.Add(122, "í", "i");
                dt.Rows.Add(123, "ì", "i");
                dt.Rows.Add(124, "æ", "i");
                dt.Rows.Add(125, "ó", "i");
                dt.Rows.Add(126, "ò", "i");
                dt.Rows.Add(127, "Î", "Y");
                dt.Rows.Add(128, "Í", "I");
                dt.Rows.Add(129, "Ì", "I");
                dt.Rows.Add(130, "Æ", "I");
                dt.Rows.Add(131, "Ó", "I");
                dt.Rows.Add(132, "Ò", "I");
                dt.Rows.Add(133, "Ñ", "D");
                dt.Rows.Add(134, "ñ", "d");

                string _SourceTemp = _Source;
                string _Char = "";
                int _Pos = 0;

                do
                {
                    if (_SourceTemp.Length >= 2)
                    {
                        _Char = Strings.Mid(_SourceTemp, 1, 2);
                        _Pos = 0;
                        for (int i = 1; i <= 134; i++)
                        {
                            if (_Char == dt.Rows[i - 1][1].ToString())
                            {
                                _Pos = i;
                                break;
                            }
                        }

                        if (_Pos > 0)
                        {
                            _ConvertResult += dt.Rows[_Pos - 1][2].ToString();
                            _SourceTemp = Strings.Right(_SourceTemp, _SourceTemp.Length - 2);
                        }
                        else
                        {
                            _Char = Strings.Mid(_SourceTemp, 1, 1);
                            _Pos = 0;
                            for (int i = 1; i <= 134; i++)
                            {
                                if (_Char == dt.Rows[i - 1][1].ToString())
                                {
                                    _Pos = i;
                                    break;
                                }
                            }
                            if (_Pos > 0)
                            {
                                _ConvertResult += dt.Rows[_Pos - 1][2].ToString();
                                _SourceTemp = Strings.Right(_SourceTemp, _SourceTemp.Length - 1);
                            }
                            else
                            {
                                _Char = Strings.Mid(_SourceTemp, 1, 1);
                                _ConvertResult += _Char;
                                _SourceTemp = Strings.Right(_SourceTemp, _SourceTemp.Length - 1);
                            }
                        }

                    }
                    else
                    {
                        _Char = Strings.Mid(_SourceTemp, 1, 1);
                        _Pos = 0;
                        for (int i = 1; i <= 134; i++)
                        {
                            if (_Char == dt.Rows[i - 1][1].ToString())
                            {
                                _Pos = i;
                                break;
                            }
                        }
                        if (_Pos > 0)
                        {
                            _ConvertResult += dt.Rows[_Pos - 1][2].ToString();
                            _SourceTemp = Strings.Right(_SourceTemp, _SourceTemp.Length - 1);
                        }
                        else
                        {
                            _Char = Strings.Mid(_SourceTemp, 1, 1);
                            _ConvertResult += _Char;
                            _SourceTemp = Strings.Right(_SourceTemp, _SourceTemp.Length - 1);
                        }
                    }

                } while (_SourceTemp.Length > 0);

                return _ConvertResult;

            }
            #endregion

            #region ABC
            else if (_TypeConvert == 3)
            {
                for (int i = 1; i <= _Source.Length; i++)
                {
                    _Temp = Strings.Mid(_Source, i, 1);
                    switch (_Temp)
                    {
                        case "¸"://1
                            _ConvertResult += "a";
                            break;
                        case "µ"://2
                            _ConvertResult += "a";
                            break;
                        case "¶"://3
                            _ConvertResult += "a";
                            break;
                        case "•"://4
                            _ConvertResult += "a";
                            break;
                        case "¹"://5
                            _ConvertResult += "a";
                            break;
                        case "¨"://6
                            _ConvertResult += "a";
                            break;
                        case "¾"://7
                            _ConvertResult += "a";
                            break;
                        case "»"://8
                            _ConvertResult += "a";
                            break;
                        case "¼"://9
                            _ConvertResult += "a";
                            break;
                        case "½"://10
                            _ConvertResult += "a";
                            break;
                        case "Æ"://11
                            _ConvertResult += "a";
                            break;
                        case "©"://12
                            _ConvertResult += "a";
                            break;
                        case "Ê"://13
                            _ConvertResult += "a";
                            break;
                        case "Ç"://14
                            _ConvertResult += "a";
                            break;
                        case "È"://15
                            _ConvertResult += "a";
                            break;
                        case "É"://16
                            _ConvertResult += "a";
                            break;
                        case "Ë"://17
                            _ConvertResult += "a";
                            break;
                        case "Ð"://18
                            _ConvertResult += "e";
                            break;
                        case "Ì"://19
                            _ConvertResult += "e";
                            break;
                        case "Î"://20
                            _ConvertResult += "e";
                            break;
                        case "Ï"://21
                            _ConvertResult += "e";
                            break;
                        case "Ñ"://22
                            _ConvertResult += "e";
                            break;
                        case "ª"://23
                            _ConvertResult += "e";
                            break;
                        case "Õ"://24
                            _ConvertResult += "e";
                            break;
                        case "Ò"://25
                            _ConvertResult += "e";
                            break;
                        case "Ó"://26
                            _ConvertResult += "e";
                            break;
                        case "Ô"://27
                            _ConvertResult += "e";
                            break;
                        case "Ö"://28
                            _ConvertResult += "e";
                            break;
                        case "ã"://29
                            _ConvertResult += "o";
                            break;
                        case "ß"://30
                            _ConvertResult += "o";
                            break;
                        case "á"://31
                            _ConvertResult += "o";
                            break;
                        case "â"://32
                            _ConvertResult += "o";
                            break;
                        case "ä"://33
                            _ConvertResult += "o";
                            break;
                        case "«"://34
                            _ConvertResult += "o";
                            break;
                        case "è"://35
                            _ConvertResult += "o";
                            break;
                        case "å"://36
                            _ConvertResult += "o";
                            break;
                        case "æ"://37
                            _ConvertResult += "o";
                            break;
                        case "ç"://38
                            _ConvertResult += "o";
                            break;
                        case "é"://39
                            _ConvertResult += "o";
                            break;
                        case "¬"://40
                            _ConvertResult += "o";
                            break;
                        case "í"://41
                            _ConvertResult += "o";
                            break;
                        case "ê"://42
                            _ConvertResult += "o";
                            break;
                        case "ë"://43
                            _ConvertResult += "o";
                            break;
                        case "ì"://44
                            _ConvertResult += "o";
                            break;
                        case "î"://45
                            _ConvertResult += "o";
                            break;
                        case "ó"://46
                            _ConvertResult += "u";
                            break;
                        case "ï"://47
                            _ConvertResult += "u";
                            break;
                        case "ñ"://48
                            _ConvertResult += "u";
                            break;
                        case "ò"://49
                            _ConvertResult += "u";
                            break;
                        case "ô"://50
                            _ConvertResult += "u";
                            break;
                        case "­"://51
                            _ConvertResult += "u";
                            break;
                        case "ø"://52
                            _ConvertResult += "u";
                            break;
                        case "õ"://53
                            _ConvertResult += "u";
                            break;
                        case "ö"://54
                            _ConvertResult += "u";
                            break;
                        case "÷"://55
                            _ConvertResult += "u";
                            break;
                        case "ù"://56
                            _ConvertResult += "u";
                            break;
                        case "ý"://57
                            _ConvertResult += "y";
                            break;
                        case "ú"://58
                            _ConvertResult += "y";
                            break;
                        case "û"://59
                            _ConvertResult += "y";
                            break;
                        case "ü"://60
                            _ConvertResult += "y";
                            break;
                        case "þ"://61
                            _ConvertResult += "y";
                            break;
                        case "¡"://67
                            _ConvertResult += "A";
                            break;
                        case "¢"://73
                            _ConvertResult += "A";
                            break;
                        case "£"://84
                            _ConvertResult += "E";
                            break;
                        case "¤"://95
                            _ConvertResult += "O";
                            break;
                        case "¥"://101
                            _ConvertResult += "O";
                            break;
                        case "Ý"://122
                            _ConvertResult += "i";
                            break;
                        case "×"://123
                            _ConvertResult += "i";
                            break;
                        case "Ø"://124
                            _ConvertResult += "i";
                            break;
                        case "Ü"://125
                            _ConvertResult += "i";
                            break;
                        case "Þ"://126
                            _ConvertResult += "i";
                            break;
                        case "§"://133
                            _ConvertResult += "D";
                            break;
                        case "®"://134
                            _ConvertResult += "d";
                            break;
                        default:
                            _ConvertResult += _Temp;
                            break;
                    }
                }
                return _ConvertResult;
            }
            #endregion

            else
            {
                return "Wrong type !";
            }
        }
        public static int GetAVGMonth(DateTime date1, DateTime date2)
        {
            TimeSpan ts1 = new TimeSpan(date1.Ticks);
            TimeSpan ts2 = new TimeSpan(date2.Ticks);

            TimeSpan dt = ts2 - ts1;
            double _month = 1;
            if (dt.Days <= 29)
                _month = 1;
            else
            {
                _month = dt.Days / 30.00;
            }

            return (int)Math.Round(_month, 0);
        }
        public static bool IsNumeric(string _number)
        {
            int _i = 0;
            bool _f = int.TryParse(_number, out _i);

            return _f;
        }
        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số

                            }
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vịs
                            continue;
                        }
                    }


                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười").Replace("Mười,", "Mười");
            }
            catch
            {
                return "";
            }

        }
    }
}
