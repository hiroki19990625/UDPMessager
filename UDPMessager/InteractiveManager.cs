using System;

namespace UDPMessenger
{
    public static class InteractiveManager
    {
        public static string InteractiveMessage(string message, bool question = false)
        {
            Console.WriteLine(message + (question ? "?" : ""));
            Console.Write("String >> ");
            return Console.ReadLine();
        }

        public static int InteractiveMessageInt(string message, bool question = false)
        {
            Console.WriteLine(message + (question ? "?" : ""));
            while (true)
            {
                Console.Write("Int >> ");
                try
                {
                    return int.Parse(Console.ReadLine() ?? "");
                }
                catch
                {

                }
            }
        }

        public static bool InteractiveMessageYesNo(string message, bool question = false)
        {
            Console.WriteLine(message + (question ? "?" : ""));
            Console.Write("Y/n >> ");
            while (true)
            {
                string line = Console.ReadLine();
                bool flag = false;
                foreach (char c in line)
                {
                    if (c != '\n' && c != '\r')
                    {
                        if (c == 'n')
                        {
                            return false;
                        }
                        else if (c == 'Y')
                        {
                            return true;
                        }
                        Console.Write("Y/n >> ");
                        flag = true;
                    }

                    if (c == '\n' && !flag)
                    {
                        Console.Write("Y/n >> ");
                    }
                }

                if (!flag)
                {
                    Console.Write("Y/n >> ");
                }
            }
        }

        public static Enum InteractiveMessageEnum(Enum enumData, string message, bool question = false)
        {
            Console.WriteLine(message + (question ? "?" : ""));
            Console.Write(enumData.GetType().Name + " >> ");

            return enumData;
        }
    }
}
