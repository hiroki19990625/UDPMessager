using System;

namespace UDPMessenger.Commands
{
    public class ConnectionCommand : Command
    {
        public override string Name => "cnt";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            throw new NotImplementedException();
        }
    }
}