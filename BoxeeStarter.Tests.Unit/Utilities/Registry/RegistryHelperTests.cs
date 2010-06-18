using BoxeeStarter.Utilities.Registry;
using NUnit.Framework;
using Rhino.Mocks;

namespace BoxeeStarter.Tests.Unit.Utilities.Registry
{
    [TestFixture]
    public class RegistryHelperTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
            FakeRegistry = Mocks.StrictMock<IWinRegistry>();
            RegistryHelper = new RegistryHelper(FakeRegistry);

            Mocks.BackToRecordAll();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        protected IWinRegistry FakeRegistry { get; set; }
        protected MockRepository Mocks { get; set; }
        protected RegistryHelper RegistryHelper { get; set; }

        [Test]
        public void ProgramRunningAtStartup_KeyIsNotFoundAndMatches_ReturnsTrue()
        {
            Expect.Call(FakeRegistry.GetCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter")).Return(
                "Program Path");

            Mocks.ReplayAll();
            bool result = RegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Program Path");

            Assert.IsTrue(result);
        }

        [Test]
        public void ProgramRunningAtStartup_KeyIsNotFoundAndMatchesButCaseDifferent_ReturnsFalse()
        {
            Expect.Call(FakeRegistry.GetCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter")).Return(
                "pRoGrAm PaTh");

            Mocks.ReplayAll();
            bool result = RegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Program Path");

            Assert.IsTrue(result);
        }

        [Test]
        public void ProgramRunningAtStartup_KeyIsNotFoundAndNotMatches_ReturnsFalse()
        {
            Expect.Call(FakeRegistry.GetCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter")).Return(
                "Not Program Path");

            Mocks.ReplayAll();
            bool result = RegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Program Path");

            Assert.IsFalse(result);
        }

        [Test]
        public void ProgramRunningAtStartup_KeyNotFound_ReturnsFalse()
        {
            Expect.Call(FakeRegistry.GetCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter")).Return(null);

            Mocks.ReplayAll();
            bool result = RegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Program Path");

            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveProgramFromStartup_ValidArgument_CallsRemoveCuSubKeyValue()
        {
            FakeRegistry.RemoveCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter");

            Mocks.ReplayAll();

            RegistryHelper.RemoveProgramFromStartup("BoxeeStarter");
        }

        [Test]
        public void RunProgramAtStartup_ValidArguments_CallsSetCuSubKeyValue()
        {
            FakeRegistry.SetCuSubKeyValue(RegistryHelper.StartupRegistryKey, "BoxeeStarter", "Some Path");

            Mocks.ReplayAll();

            RegistryHelper.RunProgramAtStartup("BoxeeStarter", "Some Path");
        }
    }
}