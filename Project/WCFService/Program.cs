using Helpers;
using System;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;

namespace WCFService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (SecurityHelper.GetName(WindowsIdentity.GetCurrent()) == ConfigHelper.GetString("WCFServiceUser"))
            {
                Console.WriteLine("Unauthorized");
            }
            else
            {
                SecureString privateKey = InputHelper.GetKey();
                WCFService.PrivateKey = privateKey;
                DatabaseHelper.PrivateKey = privateKey;

                ServiceHost host = null;

                try
                {
                    host = new ServiceHost(typeof(WCFService));

                    host.Open();
                    Console.WriteLine("Service is ready");

                    while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                }
                finally
                {
                    if (host != null)
                    {
                        host.Close();
                    }
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
