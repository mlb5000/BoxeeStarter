using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Processes
{
    [TestFixture]
    public class ProcessFinderTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Finder = new ProcessFinder();
        }

        #endregion

        protected ProcessFinder Finder { get; set; }

        [Test]
        public void ProcessAlreadyStarted_Services_ReturnsTrue()
        {
            //assumes services.exe is running...which it should be
            bool result = Finder.ProcessAlreadyStarted("services");

            Assert.IsTrue(result);
        }

        [Test]
        public void ProcessAlreadyStarter_RandomCharacters_ReturnsFalse()
        {
            //assumes no process with this name is actually running
            bool result = Finder.ProcessAlreadyStarted("fasdfadsf");

            Assert.IsFalse(result);
        }
    }
}