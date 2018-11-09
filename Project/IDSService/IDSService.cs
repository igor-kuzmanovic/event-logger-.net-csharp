using Helpers;
using IDSServiceCommon;
using System;
using System.ServiceModel;

namespace IDSService
{
    internal class IDSService : IIDSService
    {
        public void Alarm(string message)
        {
            if (SecurityHelper.GetName(OperationContext.Current) != "WCFService")
            {
                throw new FaultException("Unauthorized");
            }

            Console.WriteLine("[ALARM] {0}", message);
        }
    }
}
