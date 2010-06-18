using System;

namespace BoxeeStarter.View
{
    public interface ISettingsView
    {
        bool RunAtStartupSelected { get; set; }
        string ApplicationPath { get; }
        event EventHandler OnFormLoad;
        event EventHandler OnMinimized;
        event EventHandler OnFormExit;
        event EventHandler OnTrayShowWindow;
        event EventHandler OnTrayLoadStartup;
        event EventHandler OnRightClickMenuOpened;

        void ShowWindow();
        void HideWindow();
        void FocusOnWindow();
    }
}