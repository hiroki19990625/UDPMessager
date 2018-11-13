using System;
using System.Net;
using UDPMessenger.Packets;
using UDPMessenger.Packets.Types;

namespace UDPMessenger
{
    public class PacketHandler
    {
        public void HandlePacket(IPEndPoint endPoint, Packet packet)
        {
            if (packet is ConnectionPacket)
            {
                HandleConnectionPacket(endPoint, (ConnectionPacket)packet);
            }
            else if (packet is DisconnectPacket)
            {
                HandleDisconnectPacket(endPoint, (DisconnectPacket)packet);
            }
            else
            {
                Session session = Application.Instance.GetSession(endPoint);
                if (session != null && session.State == SessionState.Connected)
                {
                    if (packet is ChatPacket)
                    {
                        HandleChatPacket(endPoint, (ChatPacket)packet);
                    }
                    else
                    {
                        Console.WriteLine("ハンドルされていないパケットを受信しました。");
                    }
                }
                else
                {
                    Console.WriteLine("接続が確立されていない通信 >> " + endPoint.ToString());
                }
            }
        }

        public void HandleConnectionPacket(IPEndPoint endPoint, ConnectionPacket packet)
        {
            if (packet.Type == ConnectionType.Connecting)//Server
            {
                if (packet.Version == Packet.ApplicationProtocolVersion)
                {
                    Session session = new Session(endPoint, packet.UserName, packet.PublicKey);
                    Application.Instance.AddSession(endPoint, session);

                    session.State = SessionState.Connecting;

                    ConnectionPacket pk = new ConnectionPacket();
                    pk.Type = ConnectionType.ConnectingResponse;
                    pk.PublicKey = Application.Instance.Key.PublicKey;
                    pk.UserName = Application.Instance.UserName;
                    Application.Instance.SendPacket(endPoint, pk);

                    Console.WriteLine("接続中...");
                }
            }
            else if (packet.Type == ConnectionType.ConnectingResponse)//Client
            {
                Application.Instance.AddSession(endPoint, new Session(endPoint, packet.UserName, packet.PublicKey));

                ConnectionPacket pk = new ConnectionPacket();
                pk.Type = ConnectionType.Connected;
                Application.Instance.SendPacket(endPoint, pk);

                Console.WriteLine("接続の確認中...");
            }
            else if (packet.Type == ConnectionType.Connected)//Server
            {
                Session session = Application.Instance.GetSession(endPoint);
                session.State = SessionState.Connected;

                ConnectionPacket pk = new ConnectionPacket();
                pk.Type = ConnectionType.ConnectedResponse;
                Application.Instance.SendPacket(endPoint, pk);

                Console.WriteLine("{0} との接続が確立しました。", endPoint.ToString());
            }
            else if (packet.Type == ConnectionType.ConnectedResponse)//Client
            {
                Session session = Application.Instance.GetSession(endPoint);
                session.State = SessionState.Connected;

                Console.WriteLine("{0} との接続が確立しました。", endPoint.ToString());
            }
        }

        public void HandleChatPacket(IPEndPoint endPoint, ChatPacket packet)
        {
            Session session = Application.Instance.GetSession(endPoint);
            Console.WriteLine("[{0} - {1}] {2}", packet.TimeStamp, session.Name, packet.Message);
        }

        public void HandleDisconnectPacket(IPEndPoint endPoint, DisconnectPacket packet)
        {
            Application.Instance.RemoveSession(endPoint);
            Console.WriteLine("{0} との接続が切断されました。 理由: {1}", endPoint, packet.Reason);
        }
    }
}