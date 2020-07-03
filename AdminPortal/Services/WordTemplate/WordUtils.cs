using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Services.WordTemplate
{
    public static class WordUtils
    {
        public static string checkNull(string text)
        {
            if (text == "null") return "";
            return text;
        }
    }
}
