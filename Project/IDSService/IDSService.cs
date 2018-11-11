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
            // Get the client's name from the current operation context
            string clientName = SecurityHelper.GetName(OperationContext.Current);

            // If the 'IDSServiceClient' is specified (not 'Any') check the client's name
            if (ConfigHelper.GetString("IDSServiceClient") != "Any" && clientName != ConfigHelper.GetString("IDSServiceClient"))
            {
                // If the client's name doesn't match the expected client's name deny the request
                throw new FaultException("Unauthorized");
            }

            // Write the alarm message to the console
            Console.WriteLine("[{0}] {1}", clientName, message);
        }
    }
}
