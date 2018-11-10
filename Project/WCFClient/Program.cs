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
                privateKey = StringConverter.ToSecureString(RSAEncrypter.Decrypt(client.CheckIn(), SecurityHelper.GetCertificate(client)));
                Console.WriteLine("Private key retrieved from the service");
            }

            Console.WriteLine("\nStarting tests...");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [ReadAll]\n");

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [Add]\n");

                client.Add(string.Format(ResourceHelper.GetString("Event1"), 1));
                client.Add(string.Format(ResourceHelper.GetString("Event2"), 1, 2));

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [Update]\n");

                client.Update(1, string.Format(ResourceHelper.GetString("Event3"), 1, 2, 3));

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [Delete]\n");

                client.Delete(1);

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(privateKey));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [Read]\n");

                EventEntry e = client.Read(2, StringConverter.ToBytes(privateKey));
                Console.WriteLine(e.ToString());
            }

            using (WCFServiceClient client = new WCFServiceClient())
            {
                Console.WriteLine("\nTesting [ReadFile]\n");

                List<string> serializedEntries = new List<string>();
                byte[] encryptedEntries = client.ReadFile();
                if (encryptedEntries.Any())
                {
                    serializedEntries.AddRange(AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToString(privateKey)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                }

                HashSet<EventEntry> entries = new HashSet<EventEntry>();
                foreach (string serializedEntry in serializedEntries)
                {
                    entries.Add(new EventEntry(serializedEntry));
                }

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            Console.WriteLine("\nTests finished");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
