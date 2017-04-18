using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client : IChatMembers
    {
        NetworkStream stream;
        public TcpClient client;
        private string userName;

        public string UserName { get { return userName; } set { userName = value; } }

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            userName = SetUserName();
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

        public string SetUserName()
        {
            Send("Enter your desired display name for this chat...");
            string choice = Console.ReadLine();
            for (int i = 0; i < Server.chatMembers.Count; i++)
            {
                if (choice.Equals(i))
                {
                    Send("That name is not available.");
                    Console.WriteLine();
                    SetUserName();
                }
            }
            userName = choice;
            return UserName;
        }

        public void Notify(IChatMembers members)
        {
            Console.WriteLine($"{userName} has joined the chat!\n");
            //Send($"{userName} has joined the chat!\n");
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine(currentDateTime.ToString());
            //Send(currentDateTime.ToString());
        }
    }
}
