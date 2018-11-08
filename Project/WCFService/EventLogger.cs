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
        private readonly static EventLog log = Initialize();

        private static EventLog Initialize()
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, logName);

            return new EventLog(logName, Environment.MachineName, source);
        }

        public static void AuthenticationSuccess(string username)
        {
            using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Resources.resx"))
            {
                log.WriteEntry(string.Format(resx.GetString("AuthenticationSuccess"), username), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationSuccess(string username, string action)
        {
            using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Resources.resx"))
            {
                log.WriteEntry(string.Format(resx.GetString("AuthorizationSuccess"), username, action), EventLogEntryType.SuccessAudit);
            }
        }

        public static void AuthorizationFailure(string username, string action, string permission)
        {
            using (ResXResourceSet resx = new ResXResourceSet(@"..\..\Resources.resx"))
            {
                log.WriteEntry(string.Format(resx.GetString("AuthorizationFailure"), username, action, permission), EventLogEntryType.FailureAudit);
            }
        }
    }
}
