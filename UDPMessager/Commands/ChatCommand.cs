using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDPMessenger.Packets;

namespace UDPMessenger.Commands
{
    public class ChatCommand : Command
    {
        public override string Name => "chat";
        public override string Description => "チャットを送信";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            if (args.Length > 1)
            {
                Session session = Application.Instance.GetSession(args[0]);
                if (session == null)
                {
                    Console.WriteLine("ユーザーが見つかりません。");
                    return ExecuteResult.Failed;
                }

                ChatPacket packet = new ChatPacket();
                packet.Message = args[1];
                packet.TimeStamp = DateTime.Now;
                packet.UUID = Guid.NewGuid();

                Application.Instance.SendPacket(session.EndPoint, packet, true);
                return ExecuteResult.Success;
            }
            else
            {
                return ExecuteResult.Failed;
            }
        }
    }
}
