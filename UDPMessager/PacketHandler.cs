using System;
using System.Net;
using UDPMessenger.Packets;
using UDPMessenger.Packets.Types;

namespace UDPMessenger
{
    public class PacketHandler
    {
        public void HandlePacket(Application app, IPEndPoint endPoint, Packet packet)
        {
            if (packet is ConnectionPacket)
            {
                HandleConnectionPacket(app, endPoint, (ConnectionPacket) packet);
            }
            else
            {
                Session session = app.GetSession(endPoint);
                if (session != null && session.State == SessionState.Connected)
                {
                }
                else
                {
                    Console.WriteLine("接続が確立されていない通信 >> " + endPoint.ToString());
                }
            }
        }

        public void HandleConnectionPacket(Application app, IPEndPoint endPoint, ConnectionPacket packet)
        {
            if (packet.Type == ConnectionType.Connecting)
            {
                if (packet.Version == Packet.ApplicationProtocolVersion)
                {
                    app.AddSession(endPoint, new Session(endPoint, packet.PublicKey));

                    ConnectionPacket pk = new ConnectionPacket();
                    pk.Type = ConnectionType.ConnectingResponse;
                    pk.PublicKey = app.Key.PublicKey;
                    app.SendPacket(endPoint, pk);
                }
            }
            else if (packet.Type == ConnectionType.Connected)
            {
                Session session = app.GetSession(endPoint);
                session.State = SessionState.Connected;
            }
        }
    }
}