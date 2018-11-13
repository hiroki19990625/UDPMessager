using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Commands
{
    public class ConfigCommand : Command
    {
        public override string Description => "設定を変更。";

        public override string Name => "conf";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
