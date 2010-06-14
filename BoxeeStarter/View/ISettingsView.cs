using System;

namespace BoxeeStarter.View
{
    public interface ISettingsView
    {
        event EventHandler OnFormLoad;
        event EventHandler OnMinimized;
        event EventHandler OnFormExit;
        event EventHandler OnTrayShowWindow;

        void ShowWindow();
        void HideWindow();
        void FocusOnWindow();
    }
}