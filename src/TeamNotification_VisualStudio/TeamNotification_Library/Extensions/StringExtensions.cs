using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TeamNotification_Library.Extensions
{
    public static class StringExtensions
    {
        public static string[] Split(this string str, string splitter)
        {
            return Regex.Split(str, "\r\n");
        }

        public static string FormatUsing(this string str, params string[] values)
        {
            return string.Format(str, values);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static int ParseToInteger(this string str)
        {
            return int.Parse(str);
        }

        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }
    }
}