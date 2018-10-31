using UDPMessenger.Attributes;
using UDPMessenger.Packets.Types;

namespace UDPMessenger.Packets
{
    public class ConnectionPacket : Packet
    {
        public override byte PacketID => 0x01;

        public ConnectionType Type { get; set; }
        public string PublicKey { get; set; }
        public int Version { get; set; }

        [ClientSide, ServerSide]
        protected override void EncodeBody()
        {
            WriteByte((byte) Type);
            switch (Type)
            {
                case ConnectionType.Connecting:
                    WriteLInt((uint) Version);
                    WriteString(PublicKey);
                    break;

                case ConnectionType.ConnectingResponse:
                    WriteString(PublicKey);
                    break;

                case ConnectionType.Connected:

                    break;
            }
        }

        [ClientSide, ServerSide]
        protected override void DecodeBody()
        {
            Type = (ConnectionType) ReadByte();
            switch (Type)
            {
                case ConnectionType.Connecting:
                    Version = (int) ReadLInt();
                    PublicKey = ReadString();
                    break;

                case ConnectionType.ConnectingResponse:
                    PublicKey = ReadString();
                    break;

                case ConnectionType.Connected:

                    break;
            }
        }
    }
}
