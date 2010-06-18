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
        public event EventHandler OnTrayLoadStartup;
        public event EventHandler OnRightClickMenuOpened;

        public bool RunAtStartupSelected
        {
            get { return runAtStartupToolStripMenuItem.Checked; }
            set { runAtStartupToolStripMenuItem.Checked = value; }
        }

        public string ApplicationPath
        {
            get { return Application.ExecutablePath; }
        }

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

        private void OnRunAtStartupMenuItem_Click(object sender, EventArgs e)
        {
            OnTrayLoadStartup(this, e);
        }

        private void OnExitMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void OnSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OnTrayShowWindow(this, e);
        }

        private void OnRightClickMenu_Opened(object sender, EventArgs e)
        {
            OnRightClickMenuOpened(this, e);
        }
    }
}