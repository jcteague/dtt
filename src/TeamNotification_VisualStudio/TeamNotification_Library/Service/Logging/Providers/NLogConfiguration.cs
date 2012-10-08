using NLog;
using NLog.Config;
using NLog.Targets;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Logging.Providers
{
    public class NLogConfiguration : IConfigureLogging
    {
        private string layout = "${date:format=yyyy-M-dd} ${time:format=HH:mm:ss} ${logger} : ${LEVEL}, ${message}";

        public void Initialize()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();
            
            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            
            // Step 3. Set target properties 
            fileTarget.FileName = "${specialfolder:folder=LocalApplicationData}/" + GlobalConstants.Paths.LogFile;
            fileTarget.Layout = layout;
            
            // Step 4. Define rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule1);
            
            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }
    }
}