using Helpers;
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
        private readonly IWCFService channel;

        public WCFServiceClient() : this("WCFService_Endpoint") { }

        public WCFServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            channel = CreateChannel();
        }

        public byte[] CheckIn()
        {
            byte[] result = null;
            try
            {
                result = channel.CheckIn();
            }
            catch (FaultException e)
            {
                Console.WriteLine("[CheckIn] ERROR = {0}", e.Message);
            }
            return result;
        }

        public void Add(string entry)
        {
            try
            {
                channel.Add(entry);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Add] ERROR = {0}", e.Message);
            }
        }

        public void Update(int entryId, string entry)
        {
            try
            {
                channel.Update(entryId, entry);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Update] ERROR = {0}", e.Message);
            }
        }

        public void Delete(int entryId)
        {
            try
            {
                channel.Delete(entryId);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Delete] ERROR = {0}", e.Message);
            }
        }

        public object Read(int entryId)
        {
            object result = null;
            try
            {
                result = channel.Read(entryId);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Read] ERROR = {0}", e.Message);
            }
            return result;
        }

        public HashSet<object> ReadAll()
        {
            HashSet<object> result = null;
            try
            {
                result = channel.ReadAll();
            }
            catch (FaultException e)
            {
                Console.WriteLine("[ReadAll] ERROR = {0}", e.Message);
            }
            return result;
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
