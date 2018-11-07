using IDSServiceCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IDSService
{
    internal class IDSService : IIDSService
    {
        public void Alarm()
        {
            string user = OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name;

            Console.WriteLine("[{0}] Alarm!", user);
        }
    }
}
