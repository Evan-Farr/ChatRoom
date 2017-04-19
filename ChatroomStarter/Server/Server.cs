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
        private static Semaphore semaphore;
        public static Dictionary<IChatMember, string> members = new Dictionary<IChatMember, string>();

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
            Console.WriteLine("Waiting for connection....");
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine(currentDateTime.ToString());
            Console.WriteLine("--------------------------------------");
            Console.WriteLine();
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
            //Thread newClientThread = new Thread(new ThreadStart(client.Recieve));
            //newClientThread.Start();
            //Thread newClientThread = new Thread(() => client = new Client(stream, clientSocket));
            //newClientThread.Start();
            members.Add(client, client.UserId);
            DateTime currentDateTimeJoin = DateTime.Now;
            Console.WriteLine(currentDateTimeJoin.ToString());
            Console.WriteLine($"**** {client.UserId} joined chat. ****");
            Console.WriteLine();
            Thread newClientThread = new Thread(new ThreadStart(client.Recieve));
            newClientThread.Start();
        }

        private void Respond()
        {
            while (true)
            {
                Message message = default(Message);
                if (messageQueue.TryDequeue(out message))
                {
                    client.Send(message.Body);
                }
            }
            //Message message = default(Message);
            //if (messageQueue.TryDequeue(out message))
            //{
            //    client.Send(message.Body);
            //}
        }

        public void Upload()
        {
            NotifyChatMembers();
        }

        public void NotifyChatMembers()
        {
            foreach (KeyValuePair<IChatMember, string> member in members)
            {
                member.Key.Notify(member.Key);
            }
        }
    }
}
