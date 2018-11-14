using Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WCFService
{
    internal static class EventLogger
    {
        private static readonly string source;
        private static readonly string logName;
        private static readonly int attemptLimit;
        private static readonly int attemptTimeSpan;

        private static readonly ConcurrentDictionary<int, List<TimeSpan>> attempts;

        static EventLogger()
        {
            // Get the source of the event log from the configuration file
            source = ConfigHelper.GetString("EventLogSource");

            // Get the name of the event log from the configuration file
            logName = ConfigHelper.GetString("EventLogName");

            // Get the failed attempt limit from the configuration file
            attemptLimit = int.Parse(ConfigHelper.GetString("AttemptLimit"));

            // Get the time span (in seconds) for the failed attempt limit from the configuration file
            attemptTimeSpan = int.Parse(ConfigHelper.GetString("AttemptTimeSpan"));

            if (!EventLog.SourceExists(source))
            {
                // If the event source doesn't exist, create it (Requires admin privileges!)
                EventLog.CreateEventSource(source, logName);
            }

            // Create a thread-safe dictionary to store timestamps of failed attempts for each entry
            attempts = new ConcurrentDictionary<int, List<TimeSpan>>();
        }

        private static void Alarm(int entryID)
        {
            using (IDSServiceClient client = new IDSServiceClient())
            {
                // Alarm the Intrusion Detection Service by sending the ID of the entry
                client.Alarm(entryID);
            }
        }

        public static void RecordFailedAttempt(int entryID)
        {
            // If the entry is not in the dictionary, add it
            attempts.TryAdd(entryID, new List<TimeSpan>(attemptLimit));

            if (attempts[entryID].Count == attemptLimit)
            {
                // If the failed attempt limit is reached, remove the oldest timestamp
                attempts[entryID].RemoveAt(0);
            }

            // Add the timestamp of the failed attempt
            attempts[entryID].Add(TimeSpan.FromTicks(DateTime.Now.Ticks));

            if (attempts[entryID].Count == attemptLimit)
            {
                // Calculate the difference in seconds (with fractals) between the newest and the oldest failed attempt
                double timestampDifference = attempts[entryID].Last().TotalSeconds - attempts[entryID].First().TotalSeconds;

                if (timestampDifference <= attemptTimeSpan)
                {
                    // If the difference is lower or equal to the attempt time span limit alarm the Intrusion Detection Service
                    Alarm(entryID);

                    // Clear the failed attempt timestamps to prevent flooding the Intrusion Detection Service
                    attempts[entryID].Clear();
                }
            }
        }

        public static void AuthenticationSuccess(string username)
        {
            // Form an authentication success message
            string authenticationSuccess = string.Format(ResourceHelper.GetString("AuthenticationSuccess"), username);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(authenticationSuccess, EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationSuccess(string username, string action)
        {
            // Form an authorization success message
            string authorizationSuccess = string.Format(ResourceHelper.GetString("AuthorizationSuccess"), username, action);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(authorizationSuccess, EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action, string permission)
        {
            // Form an authorization failure message
            string authorizationFailure = string.Format(ResourceHelper.GetString("AuthorizationFailure"), username, action, permission);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(authorizationFailure, EventLogEntryType.FailureAudit);
            }
        }

        public static void FileAccess(string username)
        {
            // Form a file access message
            string fileAccess = string.Format(ResourceHelper.GetString("FileAccess"), username);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(fileAccess, EventLogEntryType.Information);
            }
        }
    }
}
