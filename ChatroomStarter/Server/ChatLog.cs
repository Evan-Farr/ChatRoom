using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ChatLog : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllText("ChatRoomLog.txt", DateTime.Now.ToString() + "\n" + message);
        }
    }
}
