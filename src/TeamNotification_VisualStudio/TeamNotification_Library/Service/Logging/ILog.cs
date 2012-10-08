using System;

namespace TeamNotification_Library.Service.Logging
{
    public interface ILog
    {
        void Info(string message);
        void Info(object source, string message);
        void Warn(string message);
        void Warn(object source, string message);
        void Error(string message);
        void Error(object source, string message);
        void FatalException(string message, Exception exception);
        void FatalException(object source, string message, Exception exception);
    }
}