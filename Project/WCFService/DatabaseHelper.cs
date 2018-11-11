using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using WCFServiceCommon;

namespace WCFService
{
    internal static class DatabaseHelper
    {
        private static readonly string path = ConfigHelper.GetString("DatabasePath");

        public static SecureString PrivateKey { get; set; }

        static DatabaseHelper()
        {
            if (!File.Exists(path))
            {
                // If the database file doesn't exist, create it
                File.Create(path).Dispose();
            }
        }

        private static List<string> ReadFromFile()
        {
            List<string> entries = new List<string>();

            // Read the whole file into a byte array
            byte[] encryptedFile = File.ReadAllBytes(path);

            if (encryptedFile.Any())
            {
                // Convert the secure key into a byte array
                byte[] privateKeyBytes = StringConverter.ToBytes(PrivateKey);

                // Decrypt the file using the key
                string decryptedFile = AESEncrypter.Decrypt(encryptedFile, privateKeyBytes);

                // Clear the key byte array for security reasons
                Array.Clear(privateKeyBytes, 0, privateKeyBytes.Length);

                // Split the decrypted file into strings representing serialized entries
                string[] serializedEntries = decryptedFile.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                // Add all serialized entries into the list
                entries.AddRange(serializedEntries);
            }

            return entries;
        }

        private static void WriteToFile(List<string> entries)
        {
            // Concatenate all serialized entries into a string ready for encrypting
            string serializedEntries = string.Join("|", entries);

            // Convert the secure key into a byte array
            byte[] privateKeyBytes = StringConverter.ToBytes(PrivateKey);

            // Encrypt the file using the key
            byte[] encryptedFile = AESEncrypter.Encrypt(serializedEntries, privateKeyBytes);

            // Clear the key byte array for security reasons
            Array.Clear(privateKeyBytes, 0, privateKeyBytes.Length);

            // Overwrite the existing file with the encryped and serialized entries
            File.WriteAllBytes(path, encryptedFile);
        }

        private static int GetFreeID()
        {
            int freeID = 0;

            // Get all the serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            // Create an empty set for storing used IDs
            HashSet<int> usedIDs = new HashSet<int>();

            foreach (string serializedEntry in serializedEntries)
            {
                // Get the ID from the serialized entry
                int usedID = EventEntry.GetId(serializedEntry);

                // Add it to the list of used IDs
                usedIDs.Add(usedID);
            }

            // Go through the list of all possible integers
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!usedIDs.Contains(i))
                {
                    // If an integer is not contained in the used IDs list return it to the caller
                    freeID = i;
                    break;
                }
            }

            return freeID;
        }

        public static void Add(string userID, string content)
        {
            // Get all the serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            // Try to get a free ID
            int freeID = GetFreeID();

            if (freeID > 0)
            {
                // If the free ID is greater than zero a new entry can be added
                EventEntry entry = new EventEntry(DateTime.Now, freeID, userID, content);

                // Serialize the new entry
                string serializedEntry = entry.ToString();

                // Add the serialized entry to the list
                serializedEntries.Add(serializedEntry);

                // Write the new list of entries into the file
                WriteToFile(serializedEntries);
            }
        }

        public static bool Update(int entryID, string userID, string content)
        {
            bool result = false;

            // Get all the serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            foreach (string serializedEntry in serializedEntries)
            {
                // Get the ID of the serialized entry
                int id = EventEntry.GetId(serializedEntry);

                if (id == entryID)
                {
                    // If the specified entry is found create a new entry with the same ID and new data
                    EventEntry newEntry = new EventEntry(DateTime.Now, id, userID, content);

                    // Serialize the updated entry
                    string serializedNewEntry = newEntry.ToString();

                    // Remove the old entry from the list
                    serializedEntries.Remove(serializedEntry);

                    // Add the updated entry into the list
                    serializedEntries.Add(serializedNewEntry);

                    // Write the list back to the file
                    WriteToFile(serializedEntries);

                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool Delete(int entryID)
        {
            bool result = false;
            
            // Get all the serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            foreach (string serializedEntry in serializedEntries)
            {
                // Get the ID of the serialized entry
                int id = EventEntry.GetId(serializedEntry);

                if (id == entryID)
                {
                    // If the specified entry is found remove it from the list
                    serializedEntries.Remove(serializedEntry);

                    // Write the list back to the file
                    WriteToFile(serializedEntries);

                    result = true;
                    break;
                }
            }

            return result;
        }

        public static EventEntry Read(int entryID)
        {
            EventEntry result = null; 

            // Get the list of serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            foreach (string serializedEntry in serializedEntries)
            {
                // Get the ID of the serialized entry
                int id = EventEntry.GetId(serializedEntry);

                if (id == entryID)
                {
                    // If the specified entry is found, return it's data back to the caller
                    result = new EventEntry(serializedEntry);
                    break;
                }
            }

            return result;
        }

        public static HashSet<EventEntry> ReadAll()
        {
            HashSet<EventEntry> entries = new HashSet<EventEntry>();

            // Get the list of serialized entries from the file
            List<string> serializedEntries = ReadFromFile();

            foreach (string serializedEntry in serializedEntries)
            {
                // Create a deserialized object for each entry
                EventEntry entry = new EventEntry(serializedEntry);

                // Add it to the list of entries
                entries.Add(entry);
            }

            return entries;
        }

        public static byte[] ReadFile()
        {
            byte[] fileData = null;

            // Read the encrypted file and return the data to the caller
            fileData = File.ReadAllBytes(path);

            return fileData;
        }
    }
}
