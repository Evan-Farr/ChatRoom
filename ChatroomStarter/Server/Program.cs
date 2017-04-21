using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatLog log = new ChatLog();
            new Server(log).Run();
            Console.ReadLine();
        }
    }
}
