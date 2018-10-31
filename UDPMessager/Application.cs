using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UDPMessenger.Events;
using UDPMessenger.Packets;
using UDPMessenger.Utils;

namespace UDPMessenger
{
    public class Application
    {
        private static Application _app;

        public static void Start(params String[] args)
        {
            if (_app == null)
            {
                _app = new Application();
                _app.Init();
            }
            else
            {
                throw new InvalidOperationException("既にインスタンスが作成されています。");
            }
        }

        public UDPManager NetworkManager { get; private set; }
        public PacketHandler Handler { get; private set; }
        public RsaKeys Key { get; internal set; }

        public Dictionary<string, Session> Sessions { get; } = new Dictionary<string, Session>();

        private Dictionary<int, Packet> _packets = new Dictionary<int, Packet>();

        private void Init()
        {
            Handler = new PacketHandler();
            Key = EncryptionManager.GenerateKeys();
            NetworkManager = new UDPManager();
            NetworkManager.Start();
            NetworkManager.ReceiveEvent += NetworkManager_ReceiveEvent;
        }

        private void RegisterPackets()
        {
            _packets.Add(1, new ConnectionPacket());
        }

        private Packet GetPacket(byte id)
        {
            if (_packets.ContainsKey(id))
            {
                return (Packet)_packets[id].Clone();
            }

            return null;
        }

        private Packet GetPacket(byte id, byte[] buffer)
        {
            if (_packets.ContainsKey(id))
            {
                Packet packet = (Packet) _packets[id].Clone();
                packet.SetBuffer(buffer);
                return packet;
            }

            return null;
        }

        private void NetworkManager_ReceiveEvent(object sender, PacketReceiveEventArgs e)
        {
            BinaryStream stream = new BinaryStream(e.Buffer);
            byte[] magic = stream.ReadBytes(12);
            if (StructuralComparisons.StructuralEqualityComparer.Equals(magic, Packet.Magic))
            {
                byte id = stream.ReadByte();
                stream.Offset--;

                Packet packet = GetPacket(id, stream.ReadBytes());
                packet.Decode();

                Handler.HandlePacket(this, e.EndPoint, packet);
                packet.Clone();
            }
        }

        public void SendPacket(IPEndPoint endPoint, Packet packet)
        {
            this.NetworkManager.Send(endPoint, packet.ToArray());
            packet.Clone();
        }

        public void AddSession(IPEndPoint endPoint, Session session)
        {
            this.Sessions[endPoint.ToString()] = session;
        }

        public Session GetSession(IPEndPoint endPoint)
        {
            if (this.Sessions.ContainsKey(endPoint.ToString()))
            {
                return this.Sessions[endPoint.ToString()];
            }

            return null;
        }
    }
}
