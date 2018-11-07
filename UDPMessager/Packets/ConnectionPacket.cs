using UDPMessenger.Packets.Types;

namespace UDPMessenger.Packets
{
    public class ConnectionPacket : Packet
    {
        public override byte PacketID => 0x01;

        public ConnectionType Type { get; set; }
        public string PublicKey { get; set; }
        public int Version { get; set; }

        protected override void EncodeBody()
        {
            WriteByte((byte) Type);
            switch (Type)
            {
                case ConnectionType.Connecting://Client
                    WriteLInt((uint) Version);
                    WriteString(PublicKey);
                    break;

                case ConnectionType.ConnectingResponse://Server
                    WriteString(PublicKey);
                    break;

                case ConnectionType.Connected://Client

                    break;
            }
        }

        protected override void DecodeBody()
        {
            Type = (ConnectionType) ReadByte();
            switch (Type)
            {
                case ConnectionType.Connecting://Server
                    Version = (int) ReadLInt();
                    PublicKey = ReadString();
                    break;

                case ConnectionType.ConnectingResponse://Client
                    PublicKey = ReadString();
                    break;

                case ConnectionType.Connected://Server

                    break;
            }
        }
    }
}
