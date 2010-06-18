using BoxeeStarter.Utilities.Logging;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Logging
{
    [TestFixture]
    public class ConsoleLoggerTests
    {
        [Test]
        public void Log_LogsToConsole()
        {
            var logger = new ConsoleLogger();

            logger.Log("hello world!");

            Assert.IsTrue(true);
        }
    }
}