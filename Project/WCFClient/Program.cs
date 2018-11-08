using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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

                using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Events.resx"))
                {
                    client.Add(string.Format(resx.GetString("Event1"), "Value 1"));
                    client.Add(string.Format(resx.GetString("Event2"), "Value 1", "Value 2"));
                    client.Add(string.Format(resx.GetString("Event3"), "Value 1", "Value 2", "Value 3"));
                }

                using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Events.resx"))
                {
                    client.Update(1, string.Format(resx.GetString("Event1"), "Value 1"));
                    client.Update(2, string.Format(resx.GetString("Event2"), "Value 1", "Value 2"));
                    client.Update(3, string.Format(resx.GetString("Event3"), "Value 1", "Value 2", "Value 3"));
                }

                client.Delete(1);
                client.Delete(2);
                client.Delete(3);

                client.Read(1);
                client.Read(2);
                client.Read(3);

                client.ReadAll();
            }

            securePrivateKey.Dispose();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
