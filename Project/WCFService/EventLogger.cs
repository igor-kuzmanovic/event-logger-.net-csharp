using Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WCFService
{
    internal static class EventLogger
    {
        private readonly static string source = "WCFService";
        private readonly static string logName = "WCFServiceLog";
        private readonly static int attemptLimit = 5;
        private readonly static TimeSpan attemptTimeSpan = new TimeSpan(0, 0, 10);

        private readonly static ConcurrentDictionary<int, int> attempts;
        private readonly static Timer timer;

        static EventLogger()
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, logName);

            attempts = new ConcurrentDictionary<int, int>();
            timer = new Timer(DecreaseAttempts, null, attemptTimeSpan, attemptTimeSpan);
        }

        private static void DecreaseAttempts(object state)
        {
            foreach (int entry in attempts.Keys)
            {
                if (attempts[entry] > 0)
                {
                    attempts[entry]--;
                }
            }
        }

        public static void Alarm(int entryId)
        {
            if (!attempts.ContainsKey(entryId))
            {
                attempts.TryAdd(entryId, 0);
            }

            attempts[entryId]++;

            if (attempts[entryId] > attemptLimit)
            {
                using (IDSServiceClient client = new IDSServiceClient())
                {
                    client.Alarm(string.Format(ResourceHelper.GetString("Alarm"), entryId));
                }

                attempts[entryId] = 0;
            }
        }

        public static void AuthenticationSuccess(string username)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthenticationSuccess"), username), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationSuccess(string username, string action)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthorizationSuccess"), username, action), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action, string permission)
        {
            using (EventLog log = new EventLog(logName, Environment.MachineName, source))
            {
                log.WriteEntry(string.Format(ResourceHelper.GetString("AuthorizationFailure"), username, action, permission), EventLogEntryType.FailureAudit);
            }
        }
    }
}
