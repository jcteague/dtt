using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamNotification_Library.Extensions
{
    public static class EnumerableExtensions
    {
         public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
         {
             foreach (var entity in collection)
             {
                 action(entity);
             }
         }

        public static string Join<T>(this IEnumerable<T> collection, string separator)
        {
            var stringsCollection = collection.Select(x => x.ToString());
            return string.Join(separator, stringsCollection);
        }
    }
}