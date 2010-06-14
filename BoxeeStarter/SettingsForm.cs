using System;
using System.Windows.Forms;
using BoxeeStarter.View;

namespace BoxeeStarter
{
    public partial class SettingsForm : Form, ISettingsView
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        #region ISettingsView Members

        public event EventHandler OnFormLoad;
        public event EventHandler OnMinimized;
        public event EventHandler OnFormExit;
        public event EventHandler OnTrayShowWindow;

        public void ShowWindow()
        {
            Show();
            FocusOnWindow();
        }

        public void HideWindow()
        {
            Hide();
            Visible = false;
        }

        public void FocusOnWindow()
        {
            Focus();
            BringToFront();
        }

        #endregion

        private void OnFormResize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                OnMinimized(this, e);
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs e)
        {
            OnTrayShowWindow(this, e);
        }

        private void OnTrayIconMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "exitButton")
            {
                ExitApplication();
            }

            OnTrayShowWindow(this, e);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            OnFormLoad(this, e);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            OnFormExit(this, EventArgs.Empty);
            Application.Exit();
        }
    }
}