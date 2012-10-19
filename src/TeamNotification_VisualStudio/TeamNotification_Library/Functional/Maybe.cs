using System;

namespace TeamNotification_Library.Functional
{
    public interface Maybe<T>
    {
        bool IsDefined { get; }

        bool IsEmpty { get; }

        T Value { get; }

        Maybe<R> Select<R>(Func<T, R> func) where R : class;

        R SelectMany<R>(Func<T, R> func) where R : class;
    }
}