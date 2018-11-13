using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Packets
{
    public class DisconnectPacket : Packet
    {
        public override byte PacketID => 0x10;

        public string Reason { get; set; }

        protected override void EncodeBody()
        {
            WriteString(Reason);
        }

        protected override void DecodeBody()
        {
            Reason = ReadString();
        }
    }
}
