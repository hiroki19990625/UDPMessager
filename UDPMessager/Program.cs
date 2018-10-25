using System;

namespace UDPMessenger
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = InteractiveManager.InteractiveMessage("String Input", true);
            Console.WriteLine(str);

            int val = InteractiveManager.InteractiveMessageInt("Int Input", true);
            Console.WriteLine(val);

            bool cond = InteractiveManager.InteractiveMessageYesNo("Yes or No Input");
            Console.WriteLine(cond);
        }
    }
}
