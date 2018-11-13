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
        
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid UUID { get; set; }

        protected override void EncodeBody()
        {
            WriteString(Message);
            WriteLong(TimeStamp.ToBinary());
            WriteBytes(UUID.ToByteArray());
        }

        protected override void DecodeBody()
        {
            Message = ReadString();
            TimeStamp = DateTime.FromBinary(ReadLong());
            UUID = new Guid(ReadBytes());
        }
    }
}
