using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WCFClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SecureString securePrivateKey = new SecureString();

            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("Service private key: {0}", RSAEncrypter.Decrypt(client.CheckIn(), client.Credentials.ClientCertificate.Certificate));
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
