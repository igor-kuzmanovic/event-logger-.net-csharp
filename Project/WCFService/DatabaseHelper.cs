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
        public static SecureString PrivateKey { get; set; }
        private static readonly string path = "Database.txt";
        private static int entityIDCounter = 0;

        static DatabaseHelper()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }

        private static List<string> ReadFromFile()
        {
            byte[] encryptedEntries = File.ReadAllBytes(path);
            List<string> entries = AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToString(PrivateKey)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return entries;
        }

        private static void WriteToFile(List<string> entries)
        {
            File.WriteAllBytes(path, AESEncrypter.Encrypt(string.Join("|", entries), StringConverter.ToString(PrivateKey)));
        }

        public static void Add(string userID, string content)
        {
            List<string> serializedEntries = ReadFromFile();
            EventEntry entry = new EventEntry(entityIDCounter++, userID, DateTime.Now, content);
            serializedEntries.Add(entry.ToString());
            WriteToFile(serializedEntries);
        }

        public static bool Update(int entryID, string userID, string content)
        {
            List<string> serializedEntries = ReadFromFile();
            EventEntry entry = null;

            foreach (string serializedEntry in serializedEntries)
            {
                entry = new EventEntry(serializedEntry);

                if (entry.ID == entryID.ToString())
                {
                    entry = new EventEntry(int.Parse(entry.ID), userID, DateTime.Now, content);
                    serializedEntries.Remove(serializedEntry);
                    serializedEntries.Add(entry.ToString());
                    WriteToFile(serializedEntries);

                    return true;
                }
            }

            return false;
        }

        public static bool Delete(int entryID)
        {
            List<string> serializedEntries = ReadFromFile();
            EventEntry entry = null;

            foreach (string serializedEntry in serializedEntries)
            {
                entry = new EventEntry(serializedEntry);

                if (entry.ID == entryID.ToString())
                {
                    serializedEntries.Remove(serializedEntry);
                    WriteToFile(serializedEntries);

                    return true;
                }
            }

            return false;
        }

        public static EventEntry Read(int entryID)
        {
            List<string> serializedEntries = ReadFromFile();
            EventEntry entry = null;

            foreach (string serializedEntry in serializedEntries)
            {
                entry = new EventEntry(serializedEntry);

                if (entry.ID == entryID.ToString())
                {
                    return entry;
                }
            }

            return null;
        }

        public static HashSet<EventEntry> ReadAll()
        {
            HashSet<EventEntry> entries = new HashSet<EventEntry>();
            List<string> serializedEntries = ReadFromFile();

            foreach (string serializedEntry in serializedEntries)
            {
                entries.Add(new EventEntry(serializedEntry));
            }

            return entries;
        }
    }
}
