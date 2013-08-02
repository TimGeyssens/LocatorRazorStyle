using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeekAndDestroy
{
    public class Utility
    {
        public static System.Globalization.NumberFormatInfo NumberFormatInfo
        {
            get
            {
                System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
                info.NumberDecimalSeparator = ".";
                info.NumberGroupSeparator = ",";
                return info;
            }
        }
    }
}