using System;
using System.Net;

namespace UDPMessenger.Events
{
    public class PacketReceiveEventArgs : EventArgs
    {
        public IPEndPoint EndPoint { get; }
        public byte[] Buffer { get; }

        public PacketReceiveEventArgs(IPEndPoint endPoint, byte[] buffer)
        {
            this.EndPoint = endPoint;
            this.Buffer = buffer;
        }
    }
}
