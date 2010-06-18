using BoxeeStarter.Utilities.Directories;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Directories
{
    [TestFixture]
    public class DirectoryHelperTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Helper = new DirectoryHelper();
        }

        #endregion

        protected DirectoryHelper Helper { get; set; }

        [Test]
        public void GetProgramDirFor_ProgramFoundInNormalProgramFiles_ReturnsExpectedString()
        {
            string programDir = Helper.GetProgramDirFor("Microsoft.NET");

            Assert.AreEqual("C:\\Program Files (x86)\\Microsoft.NET", programDir);
        }

        [Test]
        public void GetProgramDirFor_ProgramNotFoundInNormalProgramFiles_ReturnsNull()
        {
            string programDir = Helper.GetProgramDirFor("sdfds");

            Assert.IsNull(programDir);
        }
    }
}