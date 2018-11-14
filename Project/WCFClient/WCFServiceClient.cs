using System;
using System.Collections.Generic;
using System.ServiceModel;
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
            byte[] result = new byte[0];
			
            try
            {
                result = channel.CheckIn();
            }
            catch (Exception e)
            {
                Console.WriteLine("[CheckIn] ERROR = {0}", e.Message);
            }
			
            return result;
        }

        public bool Add(string content)
        {
			bool result = false;
			
            try
            {
                result = channel.Add(content);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Add] ERROR = {0}", e.Message);
            }
			
			return result;
        }

        public bool Update(int entryID, string content)
        {
            bool result = false;
			
            try
            {
                result = channel.Update(entryID, content);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Update] ERROR = {0}", e.Message);
            }
			
            return result;
        }

        public bool Delete(int entryID)
        {
            bool result = false;
			
            try
            {
                result = channel.Delete(entryID);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Delete] ERROR = {0}", e.Message);
            }
			
            return result;
        }

        public byte[] ReadFile()
        {
            byte[] result = new byte[0];
			
            try
            {
                result = channel.ReadFile();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReadFile] ERROR = {0}", e.Message);
            }
			
            return result;
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
