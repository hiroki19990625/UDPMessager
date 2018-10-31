using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UDPMessenger.Events;

namespace UDPMessenger
{
    public class UDPManager : IDisposable
    {
        public const int APP_PORT = 30001;

        private UdpClient _client;
        private Thread _thread;

        private bool _finish;

        public event EventHandler<PacketSendEventArgs> SendEvent;
        public event EventHandler<PacketReceiveEventArgs> ReceiveEvent;

        public UDPManager()
        {
            _client = new UdpClient(APP_PORT);
            _thread = new Thread(ReceiveLoop);
        }

        public void Start()
        {
            _thread.Start();
        }

        private void OnSendEvent(object sender, PacketSendEventArgs args)
        {
            SendEvent?.Invoke(sender, args);
        }

        private void OnReceiveEvent(object sender, PacketReceiveEventArgs args)
        {
            ReceiveEvent?.Invoke(sender, args);
        }

        private void ReceiveLoop()
        {
            while (!_finish)
            {
                IPEndPoint endPoint = null;
                byte[] buffer = _client.Receive(ref endPoint);
                if (buffer.Length > 0)
                {
                    OnReceiveEvent(this, new PacketReceiveEventArgs(endPoint, buffer));
                }
            }
        }

        public int Send(IPEndPoint endPoint, byte[] buffer)
        {
            OnSendEvent(this, new PacketSendEventArgs(endPoint, buffer));
            return _client.Send(buffer, buffer.Length, endPoint);
        }

        public void Dispose()
        {
            _finish = true;
            _thread.Abort();
            _thread = null;
            _client = null;
        }
    }
}