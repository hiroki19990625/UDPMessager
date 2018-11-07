using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Packets
{
    public class ChatPacket : Packet
    {
        public override byte PacketID => 0x03;

        protected override void EncodeBody()
        {
            this.ReadString();
            this.ReadString();
            this.ReadLong();
            this.ReadString();
        }

        protected override void DecodeBody()
        {
            
        }
    }
}
