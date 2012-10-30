using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Logging
{
    public class Loggr : ILog
    {
        private string APIKEY { get { return "ab92172f734744139af7e4edaed1ae1a"; } }
        private string LOGKEY { get { return "Yackety"; } }
        private string URI { get { return String.Format("http://post.loggr.net/1/logs/{0}/events",LOGKEY); } }

        private IHandleDialogMessages alertMessageEvents;
        private ISendHttpRequests httpRequestSender;

        
        private void LogEvent(string type, string message, string details)
        {
            var parameters = new List<KeyValuePair<string, string>>
                                 {
                                     new KeyValuePair<string, string>("text", message),
                                     new KeyValuePair<string, string>("tags", type),
                                     new KeyValuePair<string, string>("source", "VS plugin"),
                                     new KeyValuePair<string, string>("data", details),
                                     new KeyValuePair<string, string>("apikey", APIKEY)
                                 };
            httpRequestSender.Post(URI, parameters.ToArray());
        }

        public Loggr(ISendHttpRequests httpRequestSender, IHandleDialogMessages alertMessageEvents)
        {
            this.httpRequestSender = httpRequestSender;
            this.alertMessageEvents = alertMessageEvents;
        }

        public void Info(string message)
        {
            Info(this, message);
        }

        public void Info(object source, string message)
        {
            LogEvent("Info", message, source.ToString());
        }

        public void Warn(string message)
        {
            LogEvent("Warning", message,this.ToString());
        }

        public void Warn(object source, string message)
        {
            Warn("Warning", message);
        }

        public void Error(string message)
        {
            LogEvent("Error", message,this.ToString());
        }

        public void Error(object source, string message)
        {
            LogEvent("Error", message, source.ToString());
        }

        public void FatalException(string message, Exception exception)
        {
            LogEvent("Error", message,exception.StackTrace);
        }

        public void FatalException(object source, string message, Exception exception)
        {
            LogEvent("Error", message, exception.StackTrace);
        }

        public void TryOrLog(Action action)
        {
            try
            {
                action();
            }catch(Exception exc)
            {
                FatalException(this, exc.Message, exc);
                alertMessageEvents.OnAlertMessageRequested(exc.Source, new AlertMessageWasRequested { Message = "Exception: {0}".FormatUsing(exc.Message) });
            }
        }
    }
}
