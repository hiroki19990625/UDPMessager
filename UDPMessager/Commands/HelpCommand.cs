using System;

namespace UDPMessenger.Commands
{
    public class HelpCommand : Command
    {
        public override string Name => "help";
        public override string Description => "コマンドの一覧を表示。";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            if (args.Length == 0)
            {
                Command[] cmds = Application.Instance.GetCommands();
                Console.WriteLine(">>>   コマンドの一覧   <<<");
                foreach (Command cmd in cmds)
                {
                    Console.WriteLine("{0} << {1}", cmd.Name, cmd.Description);
                }
                Console.WriteLine();
            }

            return ExecuteResult.Success;
        }
    }
}
