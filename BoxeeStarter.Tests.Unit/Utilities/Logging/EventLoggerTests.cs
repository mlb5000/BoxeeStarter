using System.Diagnostics;
using BoxeeStarter.Utilities.Logging;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Logging
{
    [TestFixture]
    public class EventLoggerTests
    {
        private void LogEventAndCheckInEventLog(string message)
        {
            var logger = new EventLogger();

            //clear the event logs
            ClearEventLog();
            logger.Log(message);

            bool result = MessageFoundInEventLogForSource("BoxeeStarter", message);

#if DEBUG
            Assert.IsTrue(result);
#else
            Assert.IsFalse(result);
#endif
        }

        private void ClearEventLog()
        {
            EventLog[] logs = EventLog.GetEventLogs();
            foreach (EventLog log in logs)
            {
                if (log.LogDisplayName == "Application")
                {
                    log.Clear();
                }
            }
        }

        private bool MessageFoundInEventLogForSource(string source, string message)
        {
            EventLog[] logs = EventLog.GetEventLogs();

            foreach (EventLog log in logs)
            {
                if (log.LogDisplayName == "Application")
                {
                    foreach (EventLogEntry eventLogEntry in log.Entries)
                    {
                        if (eventLogEntry.Source == source &&
                            eventLogEntry.Message == message)
                            return true;
                    }
                }
            }

            return false;
        }

        [Test]
        public void Log_LogsToEventLog_String1()
        {
            LogEventAndCheckInEventLog("EventLoggerTests::Log_LogsToEventLog_String1");
        }

        [Test]
        public void Log_LogsToEventLog_String2()
        {
            LogEventAndCheckInEventLog("EventLoggerTests::Log_LogsToEventLog_String2");
        }
    }
}