using Helpers;
using System;
using System.Collections.Generic;
using System.Security;
using WCFServiceCommon;

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
                byte[] key = client.CheckIn();

                if (key != null)
                {
                    privateKey = StringConverter.ToSecureString(RSAEncrypter.Decrypt(key, SecurityHelper.GetCertificate(client)));
                    Console.WriteLine("Private key retrieved from the service");

                    Console.WriteLine();

                    client.Add(string.Format(ResourceHelper.GetString("Event1"), 1));
                    client.Add(string.Format(ResourceHelper.GetString("Event2"), 1, 2));

                    HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());
                    Console.WriteLine();

                    client.Update(1, string.Format(ResourceHelper.GetString("Event3"), 1, 2, 3));

                    entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());
                    Console.WriteLine();

                    client.Delete(1);

                    entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());
                    Console.WriteLine();

                    EventEntry e = client.Read(2, StringConverter.ToBytes(privateKey));
                    Console.WriteLine(e.ToString());
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
