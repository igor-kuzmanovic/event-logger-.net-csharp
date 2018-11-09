using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WCFServiceCommon
{
    public class EventEntry
    {
        public string EntityID{ get; set; }
        public string UserID { get; set; }
        public string Timestamp { get; set; }
        public string Content { get; set; }

        public EventEntry() { }

        public EventEntry(string serializedEntry)
        {
            string pattern = @"\[([^\[\]]+)\]";
            string[] entityData = null;
            int ctr = 0;
            foreach (Match m in Regex.Matches(serializedEntry, pattern))
            {
                entityData[ctr++] = m.Groups[1].Value;
            }

            EntityID = entityData[0];
            UserID = entityData[1];
            Timestamp = entityData[2];
            Content = entityData[3];
        }

        public EventEntry(string entityID, string userID, string timestamp, string content)
        {
            EntityID = entityID;
            UserID = userID;
            Timestamp = timestamp;
            Content = content;
        }

        public override string ToString()
        {
            return $"[{EntityID}][{UserID}][{Timestamp}][{Content}]";
        }
    }
}
