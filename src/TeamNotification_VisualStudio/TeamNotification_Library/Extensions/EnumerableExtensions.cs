﻿using System;
using System.Collections.Generic;
using System.Linq;
using TeamNotification_Library.Functional;

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
        
        public static Maybe<T> FirstAsMaybe<T>(this IEnumerable<T> collection, Func<T, bool> predicate) where T : class
        {
            return collection.FirstOrDefault(predicate).ToMaybe();
        }

        public static string Join<T>(this IEnumerable<T> collection, string separator)
        {
            var stringsCollection = collection.Select(x => x.ToString());
            return string.Join(separator, stringsCollection);
        }

        public static IEnumerable<Tuple<T, R>> Zip<T, R>(this IEnumerable<T> collection, IEnumerable<R> secondCollection)
        {
            return collection.Zip(secondCollection, (a, b) => new Tuple<T, R>(a, b));
        }

        public static IEnumerable<Tuple<T, int>> ZipWithIndex<T>(this IList<T> collection)
        {
            var indexes = Enumerable.Range(0, collection.Count);
            return Zip(collection, indexes);
        }
    }
}