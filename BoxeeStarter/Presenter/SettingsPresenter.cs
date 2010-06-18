using System;
using BoxeeStarter.Model;
using BoxeeStarter.Utilities.Registry;
using BoxeeStarter.View;

namespace BoxeeStarter.Presenter
{
    public class SettingsPresenter
    {
        private const string ProgramName = "BoxeeStarter";

        public SettingsPresenter(ISettingsView view, IPortListener listener)
        {
            Listener = listener;
            View = view;
        }

        private IPortListener Listener { get; set; }
        private ISettingsView View { get; set; }

        public RegistryHelper RegistryHelper { get; set; }

        public void Initialize()
        {
            View.OnFormLoad += OnViewLoad;
            View.OnMinimized += OnViewMinimized;
            View.OnFormExit += OnTrayExit;
            View.OnTrayShowWindow += OnTrayShowWindow;
            View.OnTrayLoadStartup += OnTrayLoadStartup;
            View.OnRightClickMenuOpened += OnRightClickMenuOpened;

            Listener.Start();
        }

        private void OnRightClickMenuOpened(object sender, EventArgs e)
        {
            View.RunAtStartupSelected = RegistryHelper.ProgramRunningAtStartup(ProgramName, View.ApplicationPath);
        }

        private void OnTrayLoadStartup(object sender, EventArgs e)
        {
            if (View.RunAtStartupSelected)
            {
                RegistryHelper.RunProgramAtStartup(ProgramName, View.ApplicationPath);
                return;
            }
            RegistryHelper.RemoveProgramFromStartup(ProgramName);
        }

        private void OnViewMinimized(object sender, EventArgs e)
        {
            View.HideWindow();
        }

        private void OnTrayExit(object sender, EventArgs e)
        {
            Listener.Stop();
        }

        private void OnTrayShowWindow(object sender, EventArgs e)
        {
            //View.ShowWindow();
            //View.FocusOnWindow();
        }

        private void OnViewLoad(object sender, EventArgs e)
        {
        }
    }
}