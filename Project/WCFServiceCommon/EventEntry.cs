using System;
using System.Text.RegularExpressions;

namespace WCFServiceCommon
{
    public class EventEntry
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string Timestamp { get; set; }
        public string Content { get; set; }

        public EventEntry() { }

        public EventEntry(string serializedEntry)
        {
            string pattern = @"\[([^\[\]]+)\]";
            string[] entityData = new string[4];
            int counter = 0;

            foreach (Match m in Regex.Matches(serializedEntry, pattern))
            {
                entityData[counter++] = m.Groups[1].Value;
            }

            ID = entityData[0];
            UserID = entityData[1];
            Timestamp = entityData[2];
            Content = entityData[3];
        }

        public EventEntry(int id, string userID, DateTime timestamp, string content)
        {
            ID = id.ToString();
            UserID = userID;
            Timestamp = timestamp.ToString("MM.d.yyyy - h:m:s");
            Content = content;
        }

        public EventEntry(string id, string userID, string timestamp, string content)
        {
            ID = id;
            UserID = userID;
            Timestamp = timestamp;
            Content = content;
        }

        public override string ToString()
        {
            return $"[{ID}][{UserID}][{Timestamp}][{Content}]";
        }
    }
}
