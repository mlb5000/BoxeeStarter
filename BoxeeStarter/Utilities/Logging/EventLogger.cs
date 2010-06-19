using System.Diagnostics;

namespace BoxeeStarter.Utilities.Logging
{
    public class EventLogger : ILogger
    {
        #region ILogger Members

        public void Log(string message)
        {
//#if DEBUG
            EventLog.WriteEntry("BoxeeStarter", message);
//#endif
        }

        #endregion
    }
}