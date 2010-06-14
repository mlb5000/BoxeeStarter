using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace BoxeeStarter.Model
{
    public class PortListener : IPortListener
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
        private bool _shutdown;

        public Thread WorkerThread { get; set; }

        public void Start()
        {
            var start = new ParameterizedThreadStart(ThreadProc);
            WorkerThread = new Thread(start);
            WorkerThread.Start(this);
        }

        public void Stop()
        {
            StopListening();
            Client.Close();
            WorkerThread.Abort();
        }

        private static void ThreadProc(object param)
        {
            ((IPortListener)param).Listen();
        }

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

            string message = ListenForUdpPacket();

            WriteLog("Client Connected!");
            if (BoxeeProcessAlreadyStarted())
            {
                WriteLog("Boxee is already running!");
                return;
            }

            WriteLog(String.Format("Received Message: {0}", message));

            if (!MessageIsFromIphoneRemote(message))
            {
                return;
            }

            string boxeeDir = GetBoxeeProgramDir();
            if (boxeeDir == null)
            {
                WriteLog(String.Format("Boxee directory not found in the default location: {0}", boxeeDir));
                return;
            }

            StartWindowedProcess(boxeeDir + "\\BOXEE.exe", "-p", boxeeDir);
        }

        protected virtual void StartWindowedProcess(string fileName, string arguments, string workingDir)
        {
            var info = new ProcessStartInfo(fileName, arguments)
            {
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir,
                UseShellExecute = false
            };

            Process.Start(info);
        }

        private static bool MessageIsFromIphoneRemote(string message)
        {
            var xml = new XmlDocument();
            xml.LoadXml(message);

            XmlNodeList list = xml.GetElementsByTagName("bdp1");
            if (list.Count == 0)
            {
                WriteLog("BDP1 element missing from XML document.");
                return false;
            }

            XmlNode node = list[0];

            XmlAttribute command = node.Attributes["cmd"];
            XmlAttribute app = node.Attributes["application"];
            XmlAttribute challenge = node.Attributes["challenge"];

            if (command.Value != "discover" || app.Value != "iphone_remote" || challenge.Value != "alohathere!")
                return false;

            return true;
        }

        private string ListenForUdpPacket()
        {
            var sender = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = Client.Receive(ref sender);
            Client.Close();

            string message = Encoding.ASCII.GetString(receiveBytes);
            return message;
        }

        private static void WriteLog(string message)
        {
#if DEBUG
            EventLog.WriteEntry("BoxeeStarter", message);
#endif
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
    }
}