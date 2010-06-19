using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BoxeeStarter.Utilities.Logging;

namespace BoxeeStarter.Utilities.Network
{
    public class UdpListener
    {
        protected UdpClient Client { get; set; }
        protected UdpState State { get; set; }
        protected IAsyncResult Result { get; set; }
        
        public ManualResetEvent PacketReceived;

        public UdpListener()
        {
            PacketReceived = new ManualResetEvent(false);
        }

        public virtual void ListenForUdpPacket(int port)
        {
            try
            {
                Client = new UdpClient(port);

                State = new UdpState
                            {
                                EndPoint = new IPEndPoint(IPAddress.Any, port), 
                                Client = Client
                            };
                Result = Client.BeginReceive(ReceivedCallback, State);

            }
            catch (SocketException)
            {
                new EventLogger().Log(
                    "Asynchronous UdpReceive failed...Port may already be occupied");
            }
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            IPEndPoint endpoint = null;

            try
            {
                Byte[] receiveBytes = Client.EndReceive(ar, ref endpoint);
                Message = Encoding.ASCII.GetString(receiveBytes);
                Client.Close();
            }
            catch (ObjectDisposedException)
            {
                new EventLogger().Log("Client was forcefully closed.");
                Message = String.Empty;
            }
            finally
            {
                PacketReceived.Set();
            }
        }

        public string Message { get; set; }

        public virtual void InterruptClient()
        {
            if (Client == null)
                return;

            Client.Close();
        }
    }

    public class UdpState
    {
        public IPEndPoint EndPoint { get; set; }
        public UdpClient Client { get; set; }
    }
}