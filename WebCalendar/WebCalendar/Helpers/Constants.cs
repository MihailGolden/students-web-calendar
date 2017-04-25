using System;
using System.Collections.Generic;

namespace WebCalendar.Helpers
{
    public static class Constants
    {
        public const int MilliSeconds_Timeout = 60000;
        public const string String_Format = "g";
    }

    public static class SortList
    {
        public static List<DateTime> Ascending(List<DateTime> list)
        {
            list.Sort((a, b) => a.CompareTo(b));
            return list;
        }
    }
}