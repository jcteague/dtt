using System.Collections.Generic;
using TeamNotification_Library.UI.Avalon;

namespace TeamNotification_Library.Extensions
{
    public static class SortedListExtensions
    {
        public static void AddOrUpdate<T, R>(this SortedList<T, R> list, T key, R value)
        {
            if (list.ContainsKey(key))
                if(list[key] is MixedEditorLineData)
                {
                    (list[key] as MixedEditorLineData).Message = value as string;
                }
                else
                    list[key] = value;
            else
                list.Add(key, value);
        }
    }
}