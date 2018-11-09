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

        public void Alarm(string message)
        {
            try
            {
                channel.Alarm(message);
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
                if (State != CommunicationState.Faulted)
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                if (State != CommunicationState.Closed)
                {
                    Abort();
                }
            }
        }
    }
}
