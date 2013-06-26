using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locator
{
    public class GeoItemComparer : IComparer<GeoItem>
    {
        public int Compare(GeoItem s1, GeoItem s2)
        {
            int returnValue = 1;
            if (s1 != null && s2 != null)
            {
                returnValue = s1.Distance.CompareTo(s2.Distance);
            }
            return returnValue;
        }
    }
}