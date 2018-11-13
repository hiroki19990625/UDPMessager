using System.Net;
using UDPMessenger.Packets;
using UDPMessenger.Packets.Types;

namespace UDPMessenger.Commands
{
    public class ConnectionCommand : Command
    {
        public override string Name => "cnt";
        public override string Description => "ホストに接続。";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            if (args.Length > 0)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(args[0]), UDPManager.APP_PORT);
                ConnectionPacket packet = new ConnectionPacket();
                packet.Type = ConnectionType.Connecting;
                packet.Version = Packet.ApplicationProtocolVersion;
                packet.UserName = Application.Instance.UserName;
                packet.PublicKey = Application.Instance.Key.PublicKey;

                Application.Instance.SendPacket(endPoint, packet);
                return ExecuteResult.Success;
            }
            else
            {
                return ExecuteResult.Failed;
            }
        }
    }
}