using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using WCFServiceCommon;

namespace WCFClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey(true);
            Console.Clear();

            SecureString key = new SecureString();

            using (WCFServiceClient client = new WCFServiceClient())
            {
                // Get client's certificate from the client credentials
                X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(client);

                // Check in with the WCFService to receive the private key
                byte[] encryptedKey = client.CheckIn();

                // If the encrypted key is null, the service denied the request
                if (encryptedKey != null)
                {
                    // Decrypt the encrypted key
                    string privateKey = RSAEncrypter.Decrypt(encryptedKey, clientCertificate);

                    // Clear the encrypted key for security reasons
                    Array.Clear(encryptedKey, 0, encryptedKey.Length);

                    // Convert the key into a secure key
                    key = StringConverter.ToSecureString(privateKey);

                    // Clear the unsecure key from security reasons
                    privateKey = string.Empty;

                    Console.WriteLine("Private key retrieved from the service");
                }
                else
                {
                    Console.WriteLine("Unable to retrieve the private key from the service");
                }
            }

            // Run some custom made tests using the provided key
            RunTests(key);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        private static void RunTests(SecureString key)
        {
            Console.WriteLine("\nStarting tests...");

            Console.WriteLine("\nTesting [ReadAll]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            Console.WriteLine("\nTesting [Add]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                client.Add(string.Format(ResourceHelper.GetString("Event1"), 1));
                client.Add(string.Format(ResourceHelper.GetString("Event2"), 1, 2));

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            Console.WriteLine("\nTesting [Update]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                client.Update(1, string.Format(ResourceHelper.GetString("Event3"), 1, 2, 3));

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            Console.WriteLine("\nTesting [Delete]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                client.Delete(1);

                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

                if (!entries.Any()) Console.WriteLine("Entry list is empty");
                foreach (var entry in entries) Console.WriteLine(entry.ToString());
            }

            Console.WriteLine("\nTesting [Read]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                EventEntry e = client.Read(2, StringConverter.ToBytes(key));
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nTesting [ReadFile]...\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                List<string> serializedEntries = new List<string>();
                byte[] encryptedEntries = client.ReadFile();
                if (encryptedEntries.Any())
                {
                    serializedEntries.AddRange(AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToBytes(key)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
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
        }
    }
}
