using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.Clear();

            using (WCFServiceClient client = new WCFServiceClient())
            {
                byte[] key = client.CheckIn();

                if (key != null)
                {
                    privateKey = StringConverter.ToSecureString(RSAEncrypter.Decrypt(key, SecurityHelper.GetCertificate(client)));
                    Array.Clear(key, 0, key.Length);
                    Console.WriteLine("Private key retrieved from the service");

                    Console.WriteLine("\nStarting tests...\n");

                    HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());

                    Console.WriteLine("\nTesting [Add]\n");
                    client.Add(string.Format(ResourceHelper.GetString("Event1"), 1));
                    client.Add(string.Format(ResourceHelper.GetString("Event2"), 1, 2));

                    entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());

                    Console.WriteLine("\nTesting [Update]\n");
                    client.Update(1, string.Format(ResourceHelper.GetString("Event3"), 1, 2, 3));

                    entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());

                    Console.WriteLine("\nTesting [Delete]\n");
                    client.Delete(1);

                    entries = client.ReadAll(StringConverter.ToBytes(privateKey));
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());

                    Console.WriteLine("\nTesting [Read]\n");
                    EventEntry e = client.Read(2, StringConverter.ToBytes(privateKey));
                    Console.WriteLine(e.ToString());

                    Console.WriteLine("\nTesting [ReadFile]\n");
                    List<string> serializedEntries = new List<string>();
                    byte[] encryptedEntries = client.ReadFile();
                    if (encryptedEntries.Any())
                    {
                        serializedEntries.AddRange(AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToString(privateKey)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                    entries.Clear();
                    foreach (string serializedEntry in serializedEntries)
                    {
                        entries.Add(new EventEntry(serializedEntry));
                    }
                    foreach (var entry in entries) Console.WriteLine(entry.ToString());


                    Console.WriteLine("\nTests finished\n");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
