using System;
using System.IO;
using NLog;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Logging
{
    public class Logger : ILog
    {
        public void Info(string message)
        {
            Info(this, message);
        }

        public void Info(object source, string message)
        {
            Log(source, x => x.Info(message));
        }

        public void Warn(string message)
        {
            Warn(this, message);
        }

        public void Warn(object source, string message)
        {
            Log(source, x => x.Warn(message));
        }

        public void Error(string message)
        {
            Error(this, message);
        }

        public void Error(object source, string message)
        {
            Log(source, x => x.Error(message));
        }

        private void Log(object source, Action<NLog.Logger> logAction)
        {
            logAction(LogManager.GetLogger(source.GetType().ToString()));
        }
    }
}