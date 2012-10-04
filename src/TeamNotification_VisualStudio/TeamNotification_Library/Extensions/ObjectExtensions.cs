using System;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static T Cast<T>(this object obj)
        {
            return (T) obj;
        }
    }
}