using System;
using Microsoft.Win32;

namespace BoxeeStarter.Utilities.Registry
{
    public class RegistryHelper
    {
        public const string StartupRegistryKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public RegistryHelper(IWinRegistry registry)
        {
            Registry = registry;
        }

        protected IWinRegistry Registry { get; set; }

        public virtual void RunProgramAtStartup(string programName, string programPath)
        {
            Registry.SetCuSubKeyValue(StartupRegistryKey, programName, programPath);
        }

        public virtual void RemoveProgramFromStartup(string programName)
        {
            Registry.RemoveCuSubKeyValue(StartupRegistryKey, programName);
        }

        public virtual bool ProgramRunningAtStartup(string programName, string programPath)
        {
            string keyValue = Registry.GetCuSubKeyValue(StartupRegistryKey, programName);
            if (keyValue == null)
                return false;

            if (keyValue.Equals(programPath, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }

    public interface IWinRegistry
    {
        void SetCuSubKeyValue(string key, string name, string value);
        void SetLmSubKeyValue(string key, string name, string value);
        void RemoveCuSubKeyValue(string key, string name);
        void RemoveLmSubKeyValue(string key, string name);
        string GetCuSubKeyValue(string key, string name);
    }

    public class WinRegistry : IWinRegistry
    {
        #region IWinRegistry Members

        public void SetCuSubKeyValue(string key, string name, string value)
        {
            RegistryKey rkApp = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, true);
            if (rkApp == null)
                return;

            rkApp.SetValue(name, value);
        }

        public void RemoveCuSubKeyValue(string key, string name)
        {
            RegistryKey rkApp = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, true);
            if (rkApp == null)
                return;

            rkApp.DeleteValue(key, false);
        }

        public void SetLmSubKeyValue(string key, string name, string value)
        {
            RegistryKey rkApp = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key, true);
            if (rkApp == null)
                return;

            rkApp.SetValue(name, value);
        }

        public void RemoveLmSubKeyValue(string key, string name)
        {
            RegistryKey rkApp = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key, true);
            if (rkApp == null)
                return;

            rkApp.DeleteValue(key, false);
        }

        public string GetCuSubKeyValue(string key, string name)
        {
            RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(key, true);
            if (regKey == null)
                return null;

            RegistryValueKind dataType = regKey.GetValueKind(name);
            if (dataType == RegistryValueKind.String)
                return ((string) regKey.GetValue(name));

            return null;
        }

        #endregion
    }
}