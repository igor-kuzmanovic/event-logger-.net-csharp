using Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WCFService
{
    internal static class EventLogger
    {
        private readonly static string source = "WCFService";
        private readonly static string logName = "WCFServiceLog";

        static EventLogger()
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, logName);
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
