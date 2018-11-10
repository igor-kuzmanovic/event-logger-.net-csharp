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

        static DatabaseHelper()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }

        private static List<string> ReadFromFile()
        {
            List<string> entries = new List<string>();
            byte[] encryptedEntries = File.ReadAllBytes(path);

            if (encryptedEntries.Any())
            {
                entries.AddRange(AESEncrypter.Decrypt(encryptedEntries, StringConverter.ToString(PrivateKey)).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
            }

            return entries;
        }

        private static void WriteToFile(List<string> entries)
        {
            File.WriteAllBytes(path, AESEncrypter.Encrypt(string.Join("|", entries), StringConverter.ToString(PrivateKey)));
        }

        private static int GetFreeID()
        {
            int freeID = 0;
            List<string> serializedEntries = ReadFromFile();
            HashSet<int> usedIDs = new HashSet<int>();
            EventEntry entry = null;

            foreach (string serializedEntry in serializedEntries)
            {
                entry = new EventEntry(serializedEntry);
                usedIDs.Add(int.Parse(entry.ID));
            }

            for(int i = 1; i < int.MaxValue; i++)
            {
                if (!usedIDs.Contains(i))
                {
                    freeID = i;
                    break;
                }
            }

            return freeID;
        }

        public static void Add(string userID, string content)
        {
            List<string> serializedEntries = ReadFromFile();
            EventEntry entry = new EventEntry(GetFreeID(), userID, DateTime.Now, content);
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

        public static byte[] ReadFile()
        {
            return File.ReadAllBytes(path);
        }
    }
}
