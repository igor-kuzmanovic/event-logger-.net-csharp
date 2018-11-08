using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

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
