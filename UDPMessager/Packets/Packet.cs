using System;
using UDPMessenger.Packets.Types;
using UDPMessenger.Utils;

namespace UDPMessenger.Packets
{
    public abstract class Packet : BinaryStream, ICloneable
    {
        public static int ApplicationProtocolVersion { get; } = 1;

        public static readonly byte[] Magic = new byte[12]
        {
            0xff,
            0xff,
            0x12,
            0x34,
            0x56,
            0x78,
            0x90,
            0xaa,
            0xbb,
            0xcc,
            0xdd,
            0xee
        };

        public virtual byte PacketID { get; }
        public QosType Qos { get; set; }

        public void Encode()
        {
            WriteBytes(Magic);
            WriteByte(PacketID);
            WriteByte((byte)Qos);
            EncodeBody();
        }

        protected abstract void EncodeBody();

        public void Decode()
        {
            ReadByte();
            Qos = (QosType)ReadByte();
            DecodeBody();
        }

        protected abstract void DecodeBody();

        public new object Clone()
        {
            return (Packet) this.MemberwiseClone();
        }
    }
}
