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
            // Get the name of the windows user running the application
            string userName = SecurityHelper.GetName(WindowsIdentity.GetCurrent());

            if (userName != ConfigHelper.GetString("IDSServiceUser"))
            {
                // If it doesn't match the expected windows user's name, stop the program
                Console.WriteLine("You are unauthorized to run the application");
            }
            else
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
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
