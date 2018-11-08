using IDSServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceCommon;

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

        public void Alarm()
        {
            try
            {
                channel.Alarm();
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
