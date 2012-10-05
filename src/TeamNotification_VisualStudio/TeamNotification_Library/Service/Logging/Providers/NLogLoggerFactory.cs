using NLog;
using NLog.Config;
using NLog.Targets;
using TeamNotification_Library.Service.Factories;
using Logger = NLog.Logger;

namespace TeamNotification_Library.Service.Logging.Providers
{
    public class NLogLoggerFactory : ICreateInstances<NLog.Logger>
    {
        public NLog.Logger GetInstance()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();
	
            // Step 2. Create targets and add them to the configuration 
//            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
//            config.AddTarget("console", consoleTarget);

            var logentriesTarget = new Le.LeTarget();
            config.AddTarget("logentries", logentriesTarget);
            
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
	
            // Step 3. Set target properties 
//            consoleTarget.Layout = "${date:format=HH\:MM\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/dtt-nlog.log";
            fileTarget.Layout = "${date:format=ddd MMM dd} ${time:format=HH:mm:ss} ${date:format=zzz yyyy} ${logger} : ${LEVEL}, ${message}";

            logentriesTarget.Layout =
                "${date:format=ddd MMM dd} ${time:format=HH:mm:ss} ${date:format=zzz yyyy} ${logger} : ${LEVEL}, ${message}";
            
            // Step 4. Define rules
//            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
//            config.LoggingRules.Add(rule1);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
   
            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            return LogManager.GetCurrentClassLogger();
        }
    }
}