using System;
using BoxeeStarter.Model;
using BoxeeStarter.View;

namespace BoxeeStarter.Presenter
{
    public class SettingsPresenter
    {
        private IPortListener Listener { get; set; }
        private ISettingsView View { get; set;}

        public SettingsPresenter(ISettingsView view, IPortListener listener)
        {
            Listener = listener;
            View = view;
        }

        public void Initialize()
        {
            View.OnFormLoad += OnViewLoad;
            View.OnMinimized += OnViewMinimized;
            View.OnFormExit += OnTrayExit;
            View.OnTrayShowWindow += OnTrayShowWindow;

            Listener.Start();
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
            View.ShowWindow();
            View.FocusOnWindow();
        }

        private void OnViewLoad(object sender, EventArgs e)
        {
        }
    }
}