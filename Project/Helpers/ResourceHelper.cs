using System.Resources;

namespace Helpers
{
    public static class ResourceHelper
    {
        public static string GetString(string name)
        {
            using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Resources.resx"))
            {
                return resx.GetString(name);
            }
        }
    }
}
