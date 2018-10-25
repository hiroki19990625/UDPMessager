using System;
using System.Net;

namespace UDPMessenger.Events
{
    public class PacketSendEventArgs : EventArgs
    {
        public IPEndPoint EndPoint { get; }
        public byte[] Buffer { get; }

        public PacketSendEventArgs(IPEndPoint endPoint, byte[] buffer)
        {
            this.EndPoint = endPoint;
            this.Buffer = buffer;
        }
    }
}
