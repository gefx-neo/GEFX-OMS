using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GreatEastForex.Helper
{
    public class FileHelper
    {
        public static string sanitiseFilename(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            name = Regex.Replace(name, invalidReStr, "_");
            name = name.Replace('&', '_');
            name = name.Replace(' ', '_');
            return name;
        }
    }
}