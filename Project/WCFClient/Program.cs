using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
                // Loop a main menu
                MainMenu(key);
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

        private static void MainMenu(SecureString key)
        {
            int entryID;
            string eventType;
            string user;
            int numberOfTries;
            int sleepTime;

            do
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);

                Console.Clear();

                Console.WriteLine("=========================");
                Console.WriteLine("|\tMain Menu\t|");
                Console.WriteLine("=========================");
                Console.WriteLine("[1] Add");
                Console.WriteLine("[2] Update");
                Console.WriteLine("[3] Delete");
                Console.WriteLine("[4] Read File");
                Console.WriteLine("[5] [Test] Update Loop");
                Console.WriteLine("[6] [Test] Delete Loop");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                try
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();

                            Console.Write("Event type: ");
                            eventType = Console.ReadLine();

                            Console.Write("User: ");
                            user = Console.ReadLine();

                            Console.Clear();

                            Add(key, eventType, user);

                            break;
                        case ConsoleKey.D2:
                            Console.Clear();

                            Console.Write("Entry ID: ");
                            entryID = int.Parse(Console.ReadLine());

                            Console.Write("Event type: ");
                            eventType = Console.ReadLine();

                            Console.Write("User: ");
                            user = Console.ReadLine();

                            Console.Clear();

                            Update(key, entryID, eventType, user);

                            break;
                        case ConsoleKey.D3:
                            Console.Clear();

                            Console.Write("Entry ID: ");
                            entryID = int.Parse(Console.ReadLine());

                            Console.Clear();

                            Delete(key, entryID);

                            break;
                        case ConsoleKey.D4:
                            Console.Clear();

                            ReadFile(key);

                            break;
                        case ConsoleKey.D5:
                            Console.Clear();

                            Console.Write("Entry ID: ");
                            entryID = int.Parse(Console.ReadLine());

                            Console.Write("Event type: ");
                            eventType = Console.ReadLine();

                            Console.Write("User: ");
                            user = Console.ReadLine();

                            Console.Write("Number of tries: ");
                            numberOfTries = int.Parse(Console.ReadLine());

                            Console.Write("Sleep time (milliseconds): ");
                            sleepTime = int.Parse(Console.ReadLine());

                            Console.Clear();

                            for (int i = 0; i < numberOfTries; i++)
                            {
                                Update(key, entryID, eventType, user);

                                Thread.Sleep(sleepTime);
                            }

                            break;
                        case ConsoleKey.D6:
                            Console.Clear();

                            Console.Write("Entry ID: ");
                            entryID = int.Parse(Console.ReadLine());

                            Console.Write("Number of tries: ");
                            numberOfTries = int.Parse(Console.ReadLine());

                            Console.Write("Sleep time (milliseconds): ");
                            sleepTime = int.Parse(Console.ReadLine());

                            Console.Clear();

                            for (int i = 0; i < numberOfTries; i++)
                            {
                                Delete(key, entryID);

                                Thread.Sleep(sleepTime);
                            }

                            break;
                        case ConsoleKey.Escape:
                            return;
                        default:
                            continue;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                }
            } while (true);
        }

        private static void Add(SecureString key, string eventType, string user)
        {
            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result = client.Add(string.Format(ResourceHelper.GetString(eventType), user));

                if (result == true)
                {
                    Console.WriteLine("Add successful");
                }
            }
        }

        private static void Update(SecureString key, int entryID, string eventType, string user)
        {
            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result = client.Update(entryID, string.Format(ResourceHelper.GetString(eventType), user));

                if (result == true)
                {
                    Console.WriteLine("Update successful");
                }
            }
        }

        private static void Delete(SecureString key, int entryID)
        {
            using (WCFServiceClient client = new WCFServiceClient())
            {
                bool result = client.Delete(entryID);

                if (result == true)
                {
                    Console.WriteLine("Delete successful");
                }
            }
        }

        private static void ReadFile(SecureString key)
        {
            using (WCFServiceClient client = new WCFServiceClient())
            {
                byte[] encryptedFile = client.ReadFile();

                if (encryptedFile != null)
                {
                    List<string> serializedEntries = new List<string>();

                    if (encryptedFile.Any())
                    {
                        string decryptedFile = AESEncrypter.Decrypt(encryptedFile, StringConverter.ToBytes(key));

                        serializedEntries.AddRange(decryptedFile.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                    }

                    HashSet<EventEntry> entries = new HashSet<EventEntry>();
                    foreach (string serializedEntry in serializedEntries)
                        entries.Add(new EventEntry(serializedEntry));

                    if (!entries.Any()) Console.WriteLine("Entry list is empty");
                    else foreach (var entry in entries) Console.WriteLine(entry.ToString());
                }
            }
        }
    }
}
