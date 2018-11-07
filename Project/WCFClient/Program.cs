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
                Console.WriteLine(RSAEncrypter.Decrypt(RSAEncrypter.Encrypt(AESEncrypter.Decrypt(AESEncrypter.Encrypt("key", "key"), "key"), client.Credentials.ClientCertificate.Certificate), client.Credentials.ClientCertificate.Certificate));
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
