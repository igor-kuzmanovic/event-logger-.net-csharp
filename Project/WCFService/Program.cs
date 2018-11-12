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
                SecureString privateKey = GetKey();
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

        private static SecureString GetKey()
        {
            SecureString key = new SecureString();

            Console.Write("Enter the private key: ");

            do
            {
                // Read a key from the console without printing it
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter && key.Length > 0)
                {
                    // If 'Enter' key is pressed and the key contains at least one character finish reading the keys
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && key.Length > 0)
                {
                    // If the pressed key is Backspace remove the last character from the key
                    key.RemoveAt(key.Length - 1);

                    // Remove the last star from the console
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    // If the pressed key is a valid character append it to the key
                    key.AppendChar(keyInfo.KeyChar);

                    // Write a star to the console to reflect the key length
                    Console.Write("*");
                }
            } while (true);

            Console.Clear();

            return key;
        }
    }
}
