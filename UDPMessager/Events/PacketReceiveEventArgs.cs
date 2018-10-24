using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessager.Events
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
