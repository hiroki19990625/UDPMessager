using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPMessenger.Commands
{
    public class ScriptCommand : Command
    {
        public override string Description => "スクリプトを実行。";

        public override string Name => "script";

        public override ExecuteResult ExecuteCommand(string command, string[] args)
        {
            if (args.Length > 2)
            {
                if (args[0] == "for")
                {
                    try
                    {
                        Command cmd = Application.Instance.GetCommand(args[2]);
                        for (int i = 0; i < int.Parse(args[1]); ++i)
                        {
                            cmd.ExecuteCommand(args[1], args.Skip(3).ToArray());
                        }

                        return ExecuteResult.Success;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return ExecuteResult.Error;
                    }
                }
                else if (args[0] == "async-for")
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            Command cmd = Application.Instance.GetCommand(args[2]);
                            for (int i = 0; i < int.Parse(args[1]); ++i)
                            {
                                cmd.ExecuteCommand(args[1], args.Skip(3).ToArray());
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    });

                    return ExecuteResult.Success;
                }
                else
                {
                    return ExecuteResult.Pass;
                }
            }
            else
            {
                return ExecuteResult.Failed;
            }
        }
    }
}
