using System;

namespace TeamNotification_Library.Functional
{
    public interface Maybe<T> where T : class
    {
        bool IsDefined { get; }

        bool IsEmpty { get; }

        T Value { get; }

        Maybe<R> Select<R>(Func<T, R> func) where R : class;

        R SelectMany<R>(Func<T, R> func) where R : class;
    }


//    public class Maybe<T>
//    {
//        public T Value { get; private set; }
//
//        public bool IsDefined { get; private set; }
//
//        public bool IsEmpty 
//        { 
//            get { return !IsDefined; } 
//        }
//
//        public Maybe(T value)
//        {
//            Value = value;
//            IsDefined = true;
//        }
//
//        private Maybe()
//        {
//            IsDefined = false;
//        }
//
//        public void Each(Action<T> action)
//        {
//            action(Value);
//        }
//
//        public Maybe<R> Select<T, R>(Func<T, Maybe<R>> func)
//        {
//            return IsEmpty ? new Maybe<T>() : func(Value);
//        }
//    }
}