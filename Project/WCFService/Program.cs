using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            WCFService.Start();

            ServiceHost host = new ServiceHost(typeof(WCFService));

            try
            {
                host.Open();

                Console.WriteLine("Service is ready");
                Console.WriteLine("Press any key to close the service...");
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
                WCFService.Stop();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
