using System.Configuration;

namespace Helpers
{
    public static class ConfigHelper
    {
        public static string GetString(string name)
        {
            return ConfigurationManager.AppSettings.GetValues(name)[0];
        }
    }
}
