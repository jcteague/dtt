﻿using TeamNotification_Library.Functional;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Extensions
{
    public static class FunctionalExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T source) where T : class 
        {
            if (source.IsNull())
                return new Nothing<T>();

            return new Just<T>(source);
        }
    }
}