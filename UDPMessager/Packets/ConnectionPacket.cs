using UDPMessenger.Attributes;

namespace UDPMessenger.Packets
{
    public class ConnectionPacket : Packet
    {
        public override byte PacketID => 0x01;

        public ConnectionType Type { get; set; }
        public byte[] PublicKey { get; set; }
        public int Version { get; set; }

        [ClientSide, ServerSide]
        public override void EncodeBody()
        {
            WriteByte((byte) Type);
            switch (Type)
            {
                case ConnectionType.Connecting:
                    WriteLInt((uint) Version);
                    WriteBytes(PublicKey);//64byte
                    break;

                case ConnectionType.ConnectingResponse:
                    WriteBytes(PublicKey);//64byte
                    break;

                case ConnectionType.Connected:

                    break;
            }
        }

        [ClientSide, ServerSide]
        public override void DecodeBody()
        {
            Type = (ConnectionType) ReadByte();
            switch (Type)
            {
                case ConnectionType.Connecting:
                    WriteLInt((uint) Version);
                    WriteBytes(PublicKey);//64byte
                    break;

                case ConnectionType.ConnectingResponse:
                    WriteBytes(PublicKey);//64byte
                    break;

                case ConnectionType.Connected:

                    break;
            }
        }

        public enum ConnectionType : byte
        {
            Connecting,
            ConnectingResponse,
            Connected
        }
    }
}
