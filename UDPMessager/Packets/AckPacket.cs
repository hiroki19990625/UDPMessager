using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDPMessenger.Packets.Types;

namespace UDPMessenger.Packets
{
    public class AckPacket : Packet
    {
        public override byte PacketID => 0x02;

        public AckType Type { get; set; }

        public uint[] MessageIDs { get; set; }

        protected override void EncodeBody()
        {
            WriteByte((byte) Type);
            switch (Type)
            {
                case AckType.Ack:
                    WriteUVarInt((uint) MessageIDs.Length);
                    for (int i = 0; i < MessageIDs.Length; ++i)
                    {
                        WriteUVarInt(MessageIDs[i]);
                    }
                    break;

                case AckType.Nack:
                    WriteUVarInt((uint) MessageIDs.Length);
                    for (int i = 0; i < MessageIDs.Length; ++i)
                    {
                        WriteUVarInt(MessageIDs[i]);
                    }
                    break;
            }
        }

        protected override void DecodeBody()
        {
            List<uint> messageIDs = new List<uint>();
            Type = (AckType) ReadByte();
            uint count = ReadUVarInt();
            switch (Type)
            {
                case AckType.Ack:
                    for (uint i = 0; i < count; ++i)
                    {
                        messageIDs.Add(ReadUVarInt());
                    }
                    break;

                case AckType.Nack:
                    for (uint i = 0; i < count; ++i)
                    {
                        messageIDs.Add(ReadUVarInt());
                    }
                    break;
            }

            MessageIDs = messageIDs.ToArray();
        }
    }
}
