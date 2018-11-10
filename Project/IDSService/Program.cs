using Helpers;
using System;
using System.Security.Principal;
using System.ServiceModel;

namespace IDSService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceHost host = null;

            try
            {
                host = new ServiceHost(typeof(IDSService));

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

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
