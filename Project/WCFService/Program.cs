using Helpers;
using System;
using System.Security;
using System.ServiceModel;

namespace WCFService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SecureString privateKey = InputHelper.InputPrivateKey();
            WCFService.PrivateKey = privateKey;
            DatabaseHelper.PrivateKey = privateKey;

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
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
