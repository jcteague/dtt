using System;

namespace TeamNotification_Library.Extensions
{
    public static class NumberExtensions
    {
         public static int Floor(this double number)
         {
             return (int) Math.Floor(number);
         }
    }
}