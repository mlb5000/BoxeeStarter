using System;

namespace BoxeeStarter.Utilities.Logging
{
    public class ConsoleLogger : ILogger
    {
        #region ILogger Members

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        #endregion
    }
}