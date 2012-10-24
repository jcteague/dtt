using System;
using System.IO;
using NLog;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async;

namespace TeamNotification_Library.Service.Logging
{
    public class Logger : ILog
    {
        private IHandleDialogMessages dialogMessageEvents;

        public Logger(IHandleDialogMessages dialogMessageEvents)
        {
            this.dialogMessageEvents = dialogMessageEvents;
        }

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

        public void FatalException(string message, Exception exception)
        {
            FatalException(this, message, exception);
        }

        public void FatalException(object source, string message, Exception exception)
        {
            Log(source, x => x.FatalException(message, exception));
        }

        public void TryOrLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                FatalException(exc.Source, "Got a fatal exception: {0}".FormatUsing(exc.Message), exc);
                dialogMessageEvents.OnAlertMessageRequested(exc.Source, new AlertMessageWasRequested {Message = "Exception: {0}".FormatUsing(exc.Message)});
            }
        }

        private void Log(object source, Action<NLog.Logger> logAction)
        {
            logAction(LogManager.GetLogger(source.GetType().ToString()));
        }
    }
}