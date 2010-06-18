using System.Net;
using System.Net.Sockets;
using System.Text;
using BoxeeStarter.Utilities.Logging;

namespace BoxeeStarter.Utilities.Network
{
    public class UdpListener
    {
        protected UdpClient Client { get; set; }

        public virtual string ListenForUdpPacket(int port)
        {
            try
            {
                Client = new UdpClient(port);
                var sender = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = Client.Receive(ref sender);
                Client.Close();

                string message = Encoding.ASCII.GetString(receiveBytes);
                return message;
            }
            catch (SocketException)
            {
                var logger = new EventLogger();
                logger.Log(
                    "Socket Closed.  If you closed the application, this is expected.  Otherwise it is likely an error.");
                return "";
            }
        }

        public virtual void InterruptClient()
        {
            Client.Close();
        }
    }
}