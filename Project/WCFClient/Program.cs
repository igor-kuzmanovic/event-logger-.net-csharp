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
                securePrivateKey = StringConverter.ToSecureString(RSAEncrypter.Decrypt(client.CheckIn(), SecurityHelper.GetUserCertificate(client)));
                Console.WriteLine("Private key retrieved from the service");

            }

            securePrivateKey.Dispose();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
