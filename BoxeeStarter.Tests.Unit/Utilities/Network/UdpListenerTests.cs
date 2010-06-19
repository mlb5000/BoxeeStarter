using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BoxeeStarter.Utilities.Network;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Network
{
    [TestFixture]
    public class UdpListenerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Listener = new UdpListener();
        }

        #endregion

        protected UdpListener Listener { get; set; }
        protected const int TestPort = 45678;

        public void ThreadProc()
        {
            Listener.ListenForUdpPacket(TestPort);
        }

        [Test]
        public void ListenForUdpPacket_ReceivesMessage()
        {
            var t = new Thread(ThreadProc);
            UdpClient client = new UdpClient();

            t.Start();
            byte[] datagram = Encoding.ASCII.GetBytes("test message");
            client.Send(datagram, datagram.Length, Dns.GetHostName(), TestPort);
            Thread.Sleep(1000);
            Assert.AreEqual("test message", Listener.Message);
        }

        [Test]
        public void InterruptClient_StopsBlockingCall()
        {
            var t = new Thread(ThreadProc);
            t.Start();
            Thread.Sleep(1000);

            Listener.InterruptClient();
        }
    }
}