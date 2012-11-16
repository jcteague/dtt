using System.Collections.Generic;

namespace TeamNotification_Library.Extensions
{
    public static class SortedListExtensions
    {
        public static void AddOrUpdate<T, R>(this SortedList<T, R> list, T key, R value)
        {
            if (list.ContainsKey(key))
                list[key] = value;
            else
                list.Add(key, value);
        }
    }
}