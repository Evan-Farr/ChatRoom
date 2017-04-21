using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ChatLog
    {
        public static void Main()
        {
            File.AppendAllText("ChatRoomLog.txt", "hello World");
        }
    }
}
