using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client : ISubscriber
    {
        NetworkStream stream;
        TcpClient client;
        private string userName;  

        public string UserName { get { return userName; } set { userName = value; } }


        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserName = GetUserName();
        }

        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }

        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            Message message = new Message(null, recievedMessageString);
            Server.messageQueue.Enqueue(message);
            Console.WriteLine(recievedMessageString);
        }

        public string GetUserName()
        {
            Console.WriteLine("Enter your desired display name for this chat...");
            string userNameChoice = Console.ReadLine().ToUpper();
            for(int i = 0; i < Server.chatMembers.Count; i++)
            {
                if (userNameChoice.Equals(i))
                {
                    Console.WriteLine("That name is not available. Please try again.");
                    Console.WriteLine();
                    GetUserName();
                }
            }
            return UserName;
        }  
    }
}
