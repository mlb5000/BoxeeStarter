using System;
using System.Diagnostics;
using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Processes
{
    [TestFixture]
    public class ProcessStarterTests
    {
        [Test]
        public void StartProcess_CallsWindowsApi()
        {
            var starter = new ProcessStarter();

            try
            {
                starter.StartProcess(new ProcessStartInfo());
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
                return;
            }
            Assert.Fail("Windows API did not throw an exception as expected.");
        }
    }
}