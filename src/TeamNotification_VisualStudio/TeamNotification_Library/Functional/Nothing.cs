using System;

namespace TeamNotification_Library.Functional
{
    public class Nothing<T> : Maybe<T>
    {
        public bool IsDefined
        {
            get { return false; }
        }

        public bool IsEmpty
        {
            get { return !IsDefined; }
        }

        public T Value
        {
            get
            {
                return Activator.CreateInstance<T>();
            }
        }

        public Maybe<R> Select<R>(Func<T, R> func) where R : class 
        {
            return new Nothing<R>();
        }

        public R SelectMany<R>(Func<T, R> func) where R : class 
        {
            return null;
        }
    }
}