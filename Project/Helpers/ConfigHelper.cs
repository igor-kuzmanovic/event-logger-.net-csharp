using System.Configuration;

namespace Helpers
{
    public static class ConfigHelper
    {
        public static string GetString(string name)
        {
            string result = string.Empty;

            result = ConfigurationManager.AppSettings.GetValues(name)[0];

            return result;
        }
    }
}
