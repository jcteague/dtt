using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TeamNotification_Library.Extensions
{
    public static class NumberExtensions
    {
        public static int Floor(this double number)
        {
            return (int) Math.Floor(number);
        }

        public static IEnumerable<int> RangeTo(this int start, int toNumber)
        {
            return Enumerable.Range(start, toNumber - start);
        }
    }
}