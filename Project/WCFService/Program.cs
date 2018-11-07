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
            WCFService.PrivateKey = InputPrivateKey();

            ServiceHost host = new ServiceHost(typeof(WCFService));

            try
            {
                host.Open();
                Console.WriteLine("Service is ready");

                using (IDSServiceClient client = new IDSServiceClient())
                {
                    client.Alarm();
                }

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
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        private static SecureString InputPrivateKey()
        {
            SecureString privateKey = new SecureString();
            ConsoleKeyInfo keyInfo;

            Console.Write("Enter the private key: ");

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace && privateKey.Length > 0)
                {
                    privateKey.RemoveAt(privateKey.Length - 1);
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    privateKey.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return privateKey;
        }
    }
}
