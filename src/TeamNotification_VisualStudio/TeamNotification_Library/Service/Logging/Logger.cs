using System;
using System.IO;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Logging
{
    public class Logger : ILog
    {
        private ICreateInstances<NLog.Logger> loggerFactory;

        public Logger(ICreateInstances<NLog.Logger> loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public void Write(string message)
        {
//            using (var writer = File.AppendText(Path.Combine(Path.GetTempPath(), GlobalConstants.Paths.LogFile)))
//            {
//                var now = DateTime.Now;
//                writer.WriteLine("[{0}]: {1}".FormatUsing(now.ToString("M/d/yyyy - h:mm tt"), message));
//            }
            loggerFactory.GetInstance().Error(message);
        }
    }
}