using System;
using System.Windows.Forms;
using BoxeeStarter.Model;
using BoxeeStarter.View;

namespace BoxeeStarter
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var listener = new PortListener();
            var mainForm = new SettingsForm();
            ViewObserver.Observe(mainForm, listener);

            Application.Run();
        }
    }
}