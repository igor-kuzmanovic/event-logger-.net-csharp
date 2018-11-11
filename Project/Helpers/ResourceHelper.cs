using System.Resources;

namespace Helpers
{
    public static class ResourceHelper
    {
        public static string GetString(string name)
        {
            using (ResXResourceSet resx = new ResXResourceSet(ConfigHelper.GetString("ResourcePath")))
            {
                // Opens the resource file and grabs the value from the provided key name
                return resx.GetString(name);
            }
        }
    }
}
