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

            // Get the key for accessing the database from the service
            SecureString key = GetKey();

            if (key.Length > 0)
            {
                try
                {
                    // Try running some custom made tests using the provided key
                    TestReadFile(key);
                    TestAdd(key, "OK", "John Doe");
                    TestAdd(key, "NotFound", "Jane Doe");
                    TestUpdate(key, 1, "Forbidden", "John Doe");
                    TestDelete(key, 1);
                    TestRead(key, 2);
                    TestReadAll(key);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        private static SecureString GetKey()
        {
            SecureString key = new SecureString();

            try
            {
                using (WCFServiceClient client = new WCFServiceClient())
                {
                    // Get client's certificate from the client credentials
                    X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(client);

                    // Check in with the WCFService to receive the private key
                    byte[] encryptedKey = client.CheckIn();

                    // If the encrypted key is null or empty, the service denied the request
                    if (encryptedKey != null && encryptedKey.Length > 0)
                    {
                        // Decrypt the encrypted key data
                        string privateKey = RSAEncrypter.Decrypt(encryptedKey, clientCertificate);

                        // Clear the encrypted key data for security reasons
                        Array.Clear(encryptedKey, 0, encryptedKey.Length);

                        // Convert the decrypted key into a secure string
                        key = StringConverter.ToSecureString(privateKey);

                        // Clear the unsecure key for security reasons
                        privateKey = string.Empty;

                        Console.WriteLine("Private key retrieved from the service");
                    }
                    else
                    {
                        Console.WriteLine("Unable to retrieve the private key from the service");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
            }

            return key;
        }

        private static void TestAdd(SecureString key, string eventType, string user)
        {
            Console.Write("\n===Testing ADD=====================================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result = client.Add(string.Format(ResourceHelper.GetString(eventType), user));

				if (result == true)
				{
					HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

					if (entries != null)
					{
						if (!entries.Any()) Console.WriteLine("Entry list is empty");
						else foreach (var entry in entries) Console.WriteLine(entry.ToString());
					}
				}
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }

        private static void TestUpdate(SecureString key, int entryID, string eventType, string user)
        {
            Console.Write("\n===Testing UPDATE==================================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result =client.Update(entryID, string.Format(ResourceHelper.GetString(eventType), user));

				if (result == true)
				{
					HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

					if (entries != null)
					{
						if (!entries.Any()) Console.WriteLine("Entry list is empty");
						else foreach (var entry in entries) Console.WriteLine(entry.ToString());
					}
				}
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }

        private static void TestDelete(SecureString key, int entryID)
        {
            Console.Write("\n===Testing DELETE==================================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result = client.Delete(entryID);

				if (result == true)
				{
					HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

					if (entries != null)
					{
						if (!entries.Any()) Console.WriteLine("Entry list is empty");
						else foreach (var entry in entries) Console.WriteLine(entry.ToString());
					}
				}
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }

        private static void TestRead(SecureString key, int entryID)
        {
            Console.Write("\n===Testing READ==================================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                EventEntry entry = client.Read(entryID, StringConverter.ToBytes(key));

                if (entry != null)
                {
                    if (entry.ID == 0) Console.WriteLine("Entry not found");
                    else Console.WriteLine(entry.ToString());
                }
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }

        private static void TestReadAll(SecureString key)
        {
            Console.Write("\n===Testing READ ALL================================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                HashSet<EventEntry> entries = client.ReadAll(StringConverter.ToBytes(key));

                if (entries != null)
                {
                    if (!entries.Any()) Console.WriteLine("Entry list is empty");
                    else foreach (var entry in entries) Console.WriteLine(entry.ToString());
                }
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }

        private static void TestReadFile(SecureString key)
        {
            Console.Write("\n===Testing READ FILE===============================");
            Console.WriteLine("===============================================\n");

            using (WCFServiceClient client = new WCFServiceClient())
            {
                byte[] encryptedEntries = client.ReadFile();

                if (encryptedEntries != null)
                {
                    List<string> serializedEntries = new List<string>();

                    if (encryptedEntries.Any())
                        serializedEntries.AddRange(AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToBytes(key)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));

                    HashSet<EventEntry> entries = new HashSet<EventEntry>();
                    foreach (string serializedEntry in serializedEntries)
                        entries.Add(new EventEntry(serializedEntry));

                    if (!entries.Any()) Console.WriteLine("Entry list is empty");
                    else foreach (var entry in entries) Console.WriteLine(entry.ToString());
                }
            }

            Console.Write("\n===================================================");
            Console.WriteLine("===============================================\n");
        }
    }
}
