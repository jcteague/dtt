using System.Configuration;

namespace AvenidaSoftware.TeamNotification_Package.Configuration
{
    public class LogentriesConfiguration : IInitializeConfiguration
    {
        public void Initialize()
        {
            ConfigurationManager.AppSettings.Add("LOGENTRIES_TOKEN", "3d9d1d17-371b-4082-86d8-efa722772719");
        }
    }
}