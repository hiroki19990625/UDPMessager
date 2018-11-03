namespace UDPMessenger.Commands
{
    public class HelpCommand : Command
    {
        public override string Name => "help";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            return ExecuteResult.Success;
        }
    }
}
