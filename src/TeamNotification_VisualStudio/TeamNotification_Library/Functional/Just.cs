using System;

namespace TeamNotification_Library.Functional
{
    public class Just<T> : Maybe<T> where T : class 
    {
        public T Value { get; private set; }

        public bool IsDefined { get; private set; }

        public Just(T value)
        {
            Value = value;
            IsDefined = true;
        }

        public bool IsEmpty
        {
            get { return !IsDefined; }
        }

        public Maybe<R> Select<R>(Func<T, R> func) where R : class 
        {
            return IsEmpty ? (Maybe<R>) new Nothing<R>() : new Just<R>(func(Value));
        }

        public R SelectMany<R>(Func<T, R> func) where R : class 
        {
            return IsEmpty ? null : func(Value);
        }
    }
}