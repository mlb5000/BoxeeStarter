using System;
using System.Windows.Forms;
using BoxeeStarter.Model;
using BoxeeStarter.Utilities;
using BoxeeStarter.Utilities.Directories;
using BoxeeStarter.Utilities.Logging;
using BoxeeStarter.Utilities.Processes;
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

            var listener = new BoxeeRemoteListener
                               {
                                   ProcFinder = new ProcessFinder(), 
                                   Logger = new EventLogger(),
                                   ProcStarter = new ProcessStarter(),
                                   DirHelper = new DirectoryHelper(),
                                   Listener = new UdpListener()
                               };
            var mainForm = new SettingsForm();
            ViewObserver.Observe(mainForm, listener);

            Application.Run();
        }
    }
}