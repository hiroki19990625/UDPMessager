namespace UDPMessenger.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract ExecuteResult ExecuteCommand(string command, string[] args);
    }
}