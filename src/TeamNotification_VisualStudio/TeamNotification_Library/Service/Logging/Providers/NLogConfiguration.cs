using NLog;
using NLog.Config;
using NLog.Targets;

namespace TeamNotification_Library.Service.Logging.Providers
{
    public class NLogConfiguration : IConfigureLogging
    {
        public void Initialize()
        {
//            // Step 1. Create configuration object 
//            LoggingConfiguration config = new LoggingConfiguration();
//            
//            // Step 2. Create targets and add them to the configuration 
//            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
//            config.AddTarget("console", consoleTarget);
//            
//            FileTarget fileTarget = new FileTarget();
//            config.AddTarget("file", fileTarget);
//            
//            // Step 3. Set target properties 
//            consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
////            fileTarget.FileName = "${basedir}/file.txt";
////            fileTarget.FileName = "${specialfolder:folder=LocalApplicationData}/dtt-yackety.log";
//            fileTarget.FileName = "${basedir}/dtt-yackety.log";
//            fileTarget.Layout = "${date:format=ddd MMM dd} ${time:format=HH:mm:ss} ${date:format=zzz yyyy} ${logger} : ${LEVEL}, ${message}";
//            
//            // Step 4. Define rules
////            LoggingRule rule1 = new LoggingRule("*", consoleTarget);
////            config.LoggingRules.Add(rule1);
//            
//            LoggingRule rule2 = new LoggingRule("*", fileTarget);
//            config.LoggingRules.Add(rule2);
//            
//            // Step 5. Activate the configuration
//            LogManager.Configuration = config;
        }
    }
}