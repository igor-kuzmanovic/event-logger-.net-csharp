using IDSServiceCommon;
using System;
using System.ServiceModel;

namespace WCFService
{
    internal class IDSServiceClient : ChannelFactory<IIDSService>, IIDSService, IDisposable
    {
        private readonly IIDSService channel;

        public IDSServiceClient() : this("IDSService_Endpoint") { }

        public IDSServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            channel = CreateChannel();
        }

        public void Alarm(int entryID)
        {
            try
            {
                channel.Alarm(entryID);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Alarm] ERROR = {0}", e.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                Close();
            }
            catch
            {
                try
                {
                    Abort();
                }
                catch { }
            }
        }
    }
}
