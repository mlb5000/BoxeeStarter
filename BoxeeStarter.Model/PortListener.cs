using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace BoxeeStarter.Model
{
    public class PortListener
    {
        private const int DefaultPort = 2562;

        public PortListener()
            : this(DefaultPort)
        {
        }

        public PortListener(int portNum)
        {
            PortNumber = portNum;
        }

        public int PortNumber { get; set; }
        public IPEndPoint EndPoint { get; set; }
        private UdpClient Client { get; set; }
        private bool _shutdown = false;

        public void Listen()
        {
            while (!_shutdown)
            {
                try
                {
                    if (BoxeeProcessAlreadyStarted()) 
                        continue;

                    Client = new UdpClient(PortNumber);
                    ListenForRemote();
                }
                catch (SocketException)
                {
                    WriteLog("Could not listen on port.  Boxee may already be running.");
                }
            }

            WriteLog("Leaving receive loop.");
            Client.Close();
        }

        public void ListenForRemote()
        {
            WriteLog(String.Format("Now waiting for a connection on port {0}", PortNumber));

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = Client.Receive(ref sender);
            Client.Close();

            WriteLog("Client Connected!");
            if (BoxeeProcessAlreadyStarted())
            {
                WriteLog("Boxee is already running!");
                return;
            }

            string message = Encoding.ASCII.GetString(receiveBytes);
            WriteLog(String.Format("Received Message: {0}", message));
            var xml = new XmlDocument();
            xml.LoadXml(message);

            XmlNodeList list = xml.GetElementsByTagName("bdp1");
            if (list.Count == 0)
            {
                WriteLog("BDP1 element missing from XML document.");
                return;
            }

            XmlNode node = list[0];

            XmlAttribute command = node.Attributes["cmd"];
            XmlAttribute app = node.Attributes["application"];
            XmlAttribute challenge = node.Attributes["challenge"];

            if (command.Value != "discover" || app.Value != "iphone_remote" || challenge.Value != "alohathere!")
                return;

            string boxeeDir = GetBoxeeProgramDir();
            if (boxeeDir == null)
            {
                WriteLog(String.Format("Boxee directory not found in the default location: {0}", boxeeDir));
                return;
            }

            ProcessStartInfo info = new ProcessStartInfo(boxeeDir + "\\BOXEE.exe", "-p");
            info.WindowStyle = ProcessWindowStyle.Normal;
            info.WorkingDirectory = boxeeDir;
            info.UseShellExecute = false;

            Process.Start(info);
        }

        private static void WriteLog(string message)
        {
            EventLog.WriteEntry("BoxeeStarter", message);
        }

        private static bool BoxeeProcessAlreadyStarted()
        {
            Process[] processes = Process.GetProcessesByName("BOXEE");

            WriteLog(String.Format("There are {0} processes with the name Boxee.", processes.Length));

            return processes.Length > 0;
        }

        private static string GetBoxeeProgramDir()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string boxeeDirPath = programFilesPath + "\\Boxee";
            string boxee64BitDirPath = programFilesPath + " (x86)" + "\\Boxee";

            if (Directory.Exists(boxeeDirPath))
            {
                return boxeeDirPath;
            }
            
            return Directory.Exists(boxee64BitDirPath) ? boxee64BitDirPath : null;
        }

        public void StopListening()
        {
            _shutdown = true;
        }

        #region Nested type: UdpState

        public class UdpState
        {
            public IPEndPoint EndPoint { get; set; }
            public UdpClient Client { get; set; }
        }

        #endregion
    }
}