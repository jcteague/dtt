using NLog;
using NLog.Config;
using NLog.Targets;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Logging.Providers
{
    public class NLogConfiguration : IConfigureLogging
    {
        public void Initialize()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();
            
            // Step 2. Create targets and add them to the configuration 
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            
            // Step 3. Set target properties 
            consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
            fileTarget.FileName = "${specialfolder:folder=LocalApplicationData}/" + GlobalConstants.Paths.LogFile;
//            fileTarget.FileName = "${basedir}/dtt-yackety.txt";
            fileTarget.Layout = "${date:format=yyyy-M-dd} ${time:format=HH:mm:ss} ${logger} : ${LEVEL}, ${message}";
//            fileTarget.Layout = "${message}";
            
            // Step 4. Define rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule1);
            
            LoggingRule rule2 = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule2);
            
            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }
    }
}