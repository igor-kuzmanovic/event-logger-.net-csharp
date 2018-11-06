using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceCommon;

namespace WCFClient
{
    internal class WCFServiceClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        private IWCFService channel;

        public WCFServiceClient() : this("WCFService_Endpoint")
        {

        }

        public WCFServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            channel = CreateChannel();
        }

        public void Add()
        {
            try
            {
                channel.Add();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Add] ERROR = {0}", e.Message);
            }
        }

        public void Update()
        {
            try
            {
                channel.Update();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Update] ERROR = {0}", e.Message);
            }
        }

        public void Delete()
        {
            try
            {
                channel.Delete();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Delete] ERROR = {0}", e.Message);
            }
        }

        public void Read()
        {
            try
            {
                channel.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Read] ERROR = {0}", e.Message);
            }
        }

        public void ReadAll()
        {
            try
            {
                channel.ReadAll();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReadAll] ERROR = {0}", e.Message);
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
