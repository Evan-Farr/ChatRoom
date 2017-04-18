using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server 
{
    class Server 
    {
        public static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        public static Client client;
        TcpListener server;
        public static Dictionary<IChatMembers, string> chatMembers = new Dictionary<IChatMembers, string>();

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }

        public void Run()
        {
            AcceptClient();
            client.Recieve();
            Respond();
        }

        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine(currentDateTime.ToString());
            Console.WriteLine();
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
            chatMembers.Add(client, client.UserName);
        }

        private void Respond()
        {
            Message message = default(Message);
            if (messageQueue.TryDequeue(out message))
            {
                client.Send(message.Body);
            }
        }

        public void Upload()
        {
            NotifyChatMembers();
        }
        
        public void NotifyChatMembers()
        {
            foreach(KeyValuePair<IChatMembers, string> member in chatMembers)
            {
                member.Key.Notify(member.Key);
            }
        }
    }
}
