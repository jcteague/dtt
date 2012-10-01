using System;
using System.IO;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Logging
{
    public class Logger : ILog
    {
        public void Write(string message)
        {
            using (var writer = File.AppendText(Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), GlobalConstants.Paths.LogFile)))
            {
                var now = DateTime.Now;
                writer.WriteLine("[{0}]: {1}".FormatUsing(now.ToString("M/d/yyyy - h:mm tt"), message));
            }
        }
    }
}