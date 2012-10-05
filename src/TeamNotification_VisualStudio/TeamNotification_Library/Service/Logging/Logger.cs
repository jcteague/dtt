using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
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
//            loggerFactory.GetInstance().Error(message);
            GetInstance().Debug(message);
            GetInstance().Error(message);
            GetInstance().Trace(message);
        }

        private string layout =
            "${date:format=ddd MMM dd} ${time:format=HH:mm:ss} ${date:format=zzz yyyy} ${logger} : ${LEVEL}, ${message}";

        private LoggingConfiguration config;

        private NLog.Logger GetInstance()
        {
            if (config.IsNull())
            {
                // Step 1. Create configuration object 
                config = new LoggingConfiguration();

                // Step 2. Create targets and add them to the configuration 
                //            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
                //            config.AddTarget("console", consoleTarget);

                var fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);

//                var logentriesTarget = new Le.LeTarget();
//                config.AddTarget("logentries", logentriesTarget);

                // Step 3. Set target properties 
                //            consoleTarget.Layout = "${date:format=HH\:MM\:ss} ${logger} ${message}";
                fileTarget.FileName = "${basedir}/dtt-nlog2.log";
                fileTarget.Layout = layout;

//                logentriesTarget.Layout = layout;

                // Step 4. Define rules
                //            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                //            config.LoggingRules.Add(rule1);

                LoggingRule rule1 = new LoggingRule("*", fileTarget);
                config.LoggingRules.Add(rule1);

//                LoggingRule rule2 = new LoggingRule("*", logentriesTarget);
//                config.LoggingRules.Add(rule2);

                // Step 5. Activate the configuration
                LogManager.Configuration = config;
            }

            NLog.Logger logger = LogManager.GetCurrentClassLogger();
//            logger.Debug("This is a message");

            //            return LogManager.GetCurrentClassLogger();
            return logger;
        }
    }
}