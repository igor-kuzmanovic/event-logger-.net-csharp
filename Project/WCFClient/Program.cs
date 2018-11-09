using Helpers;
using System;
using System.Security;

namespace WCFClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SecureString privateKey = new SecureString();

            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            using (WCFServiceClient client = new WCFServiceClient())
            {
                privateKey = StringConverter.ToSecureString(RSAEncrypter.Decrypt(client.CheckIn(), SecurityHelper.GetCertificate(client)));
                Console.WriteLine("Private key retrieved from the service");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
