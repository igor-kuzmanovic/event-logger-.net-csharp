using System.Configuration;

namespace Helpers
{
    public static class ConfigHelper
    {
        public static string GetString(string name)
        {
            string result = string.Empty;

            // Assume there's only one value for the provided key name and get it
            result = ConfigurationManager.AppSettings.GetValues(name)[0];

            return result;
        }
    }
}
