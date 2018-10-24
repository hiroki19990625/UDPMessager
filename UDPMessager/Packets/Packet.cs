using UDPMessager.Utils;

namespace UDPMessager.Packets
{
    public class Packet : BinaryStream
    {
        public byte[] Magic { get; } = new byte[12]
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

        public virtual void EncodeHeader()
        {
            WriteBytes(Magic);
            WriteByte(PacketID);
            EncodeBody();
        }

        public virtual void EncodeBody()
        {

        }

        public virtual void DecodeHeader()
        {
            ReadBytes(12);
            ReadByte();
            DecodeBody();
        }

        public virtual void DecodeBody()
        {

        }
    }
}
