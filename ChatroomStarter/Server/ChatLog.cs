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
            string path = @"C:\Users\EvanC\Documents\GitHub\ChatRoom\ChatroomStarter\Server\bin\Debug\ChatRoomLog.txt";

            if (!File.Exists(path))
            {
                string createText = "!!!! Beginning of Log !!!!\n" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            File.AppendAllText(path, DateTime.Now.ToString() + "\n" + message);
        }
    }
}
