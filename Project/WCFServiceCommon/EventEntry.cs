using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WCFServiceCommon
{
    public class EventEntry
    {
        public DateTime Timestamp { get; set; }
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Content { get; set; }

        public EventEntry() { }

        public EventEntry(string serializedEntry)
        {
            // Split the serialized entry into strings ('[Timestamp][ID][UserID][Content]')
            string[] properties = serializedEntry.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            Timestamp = DateTime.Parse(properties[0]);
            ID = int.Parse(properties[1]);
            UserID = properties[2];
            Content = properties[3];
        }

        public EventEntry(DateTime timestamp, int id, string userID, string content)
        {
            Timestamp = timestamp;
            ID = id;
            UserID = userID;
            Content = content;
        }

        public static int GetId(string serializedEntry)
        {
            int id = 0;

            // Split the serialized entry into strings ('[Timestamp][ID][UserID][Content]')
            string[] properties = serializedEntry.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            // Parse the id
            id = int.Parse(properties[1]);

            return id;
        }

        public override string ToString()
        {
            string serializedEntry = string.Empty;

            // Serialize the entry into a string
            serializedEntry = string.Format("[{0}][{1}][{2}][{3}]", Timestamp.ToString(), ID, UserID, Content);

            return serializedEntry;
        }
    }
}
