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
                char c = (char) Console.Read();
                if (c != '\n' && c!= '\r')
                {
                    if (c == 'Y')
                    {
                        return true;
                    }
                    else if (c == 'n')
                    {
                        return false;
                    }
                    Console.Write("Y/n >> ");
                }
            }
        }
    }
}
