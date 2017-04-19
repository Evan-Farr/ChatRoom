using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Client : IChatMember
    {
        NetworkStream stream;
        public TcpClient client;
        public string userId;
        private string recievedMessageString;
        //private string userName;

        public string UserId { get { return userId; } set { userId = value; } }
        //public string UserName { get { return userName; } set { userName = value; } }
        public string RecievedMessageString { get { return recievedMessageString; } set { recievedMessageString = value; } }

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = SetUserId();
            //userName = SetUserName();
        }

        public void Send(string Message)
        {
            while (true)
            {
                byte[] message = Encoding.ASCII.GetBytes(Message);
                stream.Write(message, 0, message.Count());
            }
            //byte[] message = Encoding.ASCII.GetBytes(Message);
            //stream.Write(message, 0, message.Count());
        }

        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
                Message message = new Message(this, recievedMessageString);
                Server.messageQueue.Enqueue(message);
                DateTime currentDateTime = DateTime.Now;
                Console.WriteLine(currentDateTime.ToString());
                Console.WriteLine($">> {UserId}: " + recievedMessageString);
            }
            //byte[] recievedMessage = new byte[256];
            //stream.Read(recievedMessage, 0, recievedMessage.Length);
            //string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            //Message message = new Message(this, recievedMessageString);
            //Server.messageQueue.Enqueue(message);
            //Console.WriteLine(recievedMessageString);
        }

        public string SetUserId()
        {
            Random random = new Random();
            int newRandom = random.Next(100000);
            for (int i = 0; i < Server.members.Count; i++)
            {
                if (newRandom.Equals(i))
                {
                    SetUserId();
                }
            }
            UserId = newRandom.ToString();
            return UserId;
        }

        //public string SetUserName()
        //{
        //    Send("Enter your desired display name for this chat...");
        //    byte[] recievedMessage = new byte[256];
        //    stream.Read(recievedMessage, 0, recievedMessage.Length);
        //    string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
        //    for (int i = 0; i < Server.members.Count; i++)
        //    {
        //        if (recievedMessageString.Equals(i))
        //        {
        //            Send("That name is not available.");
        //            Console.WriteLine();
        //            SetUserName();
        //        }
        //    }
        //    userName = recievedMessageString;
        //    return userName;
        //}

        public void Notify(IChatMember member)
        {
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine(currentDateTime.ToString());
            Console.WriteLine($"**** {userId} has joined the chat! ****\n");
            //Send(currentDateTime.ToString());
            //Send($"{userId} has joined the chat!\n");
        }
    }
}
