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
            SecureString privateKey = InputHelper.InputPrivateKey();
            WCFService.PrivateKey = privateKey;

            ServiceHost host = new ServiceHost(typeof(WCFService));

            try
            {
                host.Open();
                Console.WriteLine("Service is ready");

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
                privateKey.Dispose();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
