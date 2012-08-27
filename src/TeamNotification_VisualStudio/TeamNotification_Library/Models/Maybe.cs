using System;

namespace TeamNotification_Library.Models
{
    public class Maybe<T>
    {
        public T Value { get; private set; }

        public bool IsDefined { get; private set; }

        public bool IsEmpty 
        { 
            get { return !IsDefined; } 
        }

        public Maybe(T value)
        {
            Value = value;
            IsDefined = true;
        }

        private Maybe()
        {
            IsDefined = false;
        }

        public void Each(Action<T> action)
        {
            action(Value);
        }

        public Maybe<T> Select(Func<T, Maybe<T>> func)
        {
            return IsEmpty ? new Maybe<T>() : func(Value);
        }
    }
}