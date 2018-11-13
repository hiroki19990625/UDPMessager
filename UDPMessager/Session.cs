using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger
{
    public class Session
    {
        public IPEndPoint EndPoint { get; }
        public string Name { get; }
        public string PublicKey { get; }
        public SessionState State { get; set; } = SessionState.Connecting;

        public Session(IPEndPoint endPoint, string name, string publicKey)
        {
            Name = name;
            PublicKey = publicKey;
        }
    }
}
