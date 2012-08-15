using System;
using System.Collections.Generic;

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
    }
}