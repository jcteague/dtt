using NLog.Config;

namespace TeamNotification_Library.Service.Logging.Loggers
{
    public interface IProvideLoggerConfiguration
    {
        LoggingConfiguration Get();
    }
}