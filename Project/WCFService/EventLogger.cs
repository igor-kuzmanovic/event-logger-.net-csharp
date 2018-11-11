using Helpers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace WCFService
{
    internal static class EventLogger
    {
        private static readonly string source = ConfigHelper.GetString("EventLogSource");
        private static readonly string logName = ConfigHelper.GetString("EventLogName");
        private static readonly int attemptLimit = int.Parse(ConfigHelper.GetString("AttemptLimit"));
        private static readonly int attemptTimeSpan = int.Parse(ConfigHelper.GetString("AttemptTimeSpan"));

        private static readonly ConcurrentDictionary<int, int> attempts;
        private static readonly Timer timer;

        static EventLogger()
        {
            if (!EventLog.SourceExists(source))
            {
                // If the event source doesn't exist, create it (Requires admin privileges!)
                EventLog.CreateEventSource(source, logName);
            }

            // Create a thread-safe dictionary for storing information about failed attemps to modify individual entries
            attempts = new ConcurrentDictionary<int, int>();

            // Start a timer that executes the DecreaseAttemps function every 'attemptTimeSpan' seconds
            timer = new Timer(DecreaseAttempts, null, attemptTimeSpan, attemptTimeSpan);
        }

        private static void DecreaseAttempts(object state)
        {
            // Go through the list of entries
            foreach (int entryID in attempts.Keys)
            {
                if (attempts[entryID] > 0)
                {
                    // Decrease the amount of failed attemps on that entry by one
                    attempts[entryID]--;
                }
            }
        }

        public static void IncreaseAttemps(int entryID)
        {
            // If the entry is not in the dictionary, add it
            attempts.TryAdd(entryID, 0);

            // Increase the number of failed attemps on that entry by one
            attempts[entryID]++;

            if (attempts[entryID] >= attemptLimit)
            {
                // If the number of failed attempts is over the limit invoke the alarm action
                Alarm(entryID);

                // Reset the failed attempt counter to zero
                attempts[entryID] = 0;
            }
        }

        public static void Alarm(int entryID)
        {
            using (IDSServiceClient client = new IDSServiceClient())
            {
                // Form an alarm message for the specified entry
                string alarm = string.Format(ResourceHelper.GetString("Alarm"), entryID);

                // Send the alarm message to the Intrusion Detection Service
                client.Alarm(alarm);
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
            string authorizationFailure = string.Format(ResourceHelper.GetString("AuthorizationFailurePermission"), username, action, permission);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(authorizationFailure, EventLogEntryType.FailureAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action)
        {
            // Form an authentication failure message
            string authorizationFailure = string.Format(ResourceHelper.GetString("AuthorizationFailureKey"), username, action);

            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                // Write the entry into the windows log
                log.WriteEntry(authorizationFailure, EventLogEntryType.FailureAudit);
            }
        }
    }
}
