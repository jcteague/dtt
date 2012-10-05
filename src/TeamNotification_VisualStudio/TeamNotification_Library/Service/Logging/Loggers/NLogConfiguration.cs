using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Logging.Loggers
{
//    public class NLogConfiguration : IProvideLoggerConfiguration
//    {
//        private string layout =
//            "${date:format=ddd MMM dd} ${time:format=HH:mm:ss} ${date:format=zzz yyyy} ${logger} : ${LEVEL}, ${message}";
//
//        private LoggingConfiguration config;
//
//        public LoggingConfiguration Get()
//        {
//            if (config.IsNotNull())
//                return config;
//                
//            config = new LoggingConfiguration();
//
////            var fileTarget = new FileTarget();
////            fileTarget.FileName = "${specialfolder:LocalApplicationData}/yackety.log";
////            fileTarget.Layout = layout;
////            config.AddTarget("file", fileTarget);
//            
//            var logentriesTarget = new Le.LeTarget();
//            logentriesTarget.Layout = layout;
//            config.AddTarget("logentries", logentriesTarget);
//
////            var rule1 = new LoggingRule("*", fileTarget);
////            config.LoggingRules.Add(rule1);
//
//            var rule2 = new LoggingRule("*", logentriesTarget);
//            config.LoggingRules.Add(rule2);
//
//            return config;
//        }
//    }
}