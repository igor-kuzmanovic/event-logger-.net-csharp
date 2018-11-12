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
            // Get the name of the windows user running the application
            string userName = SecurityHelper.GetName(WindowsIdentity.GetCurrent());

            // If the 'WCFServiceUser' is specified (not 'Any') check the user's name
            if (ConfigHelper.GetString("WCFServiceUser") != "Any" && userName != ConfigHelper.GetString("WCFServiceUser"))
            {
                // If it doesn't match the expected windows user's name, stop the program 
                Console.WriteLine("You are unauthorized to run the application");
            }
            else
            {
                // Get the private key from the console
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
                    try
                    {
                        host.Close();
                    }
                    catch { }
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
