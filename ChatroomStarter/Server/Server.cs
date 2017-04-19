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
        public static Dictionary<IChatMember, string> members = new Dictionary<IChatMember, string>();

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
            Console.WriteLine("Listening....");
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
            Console.WriteLine("--------------------------------------");
            Console.WriteLine();
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
            members.Add(client, client.UserId);
            Thread newClientThread = new Thread(new ThreadStart(client.Recieve));
            newClientThread.Start();
            DateTime currentDateTimeJoin = DateTime.Now;
            Console.WriteLine(currentDateTimeJoin.ToString());
            Console.WriteLine($"**** {client.UserId} joined chat. ****");
            Console.WriteLine();
            Thread keepListening = new Thread(new ThreadStart(AcceptClient));
            keepListening.Start();
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
            NotifyChatMember();
        }

        public void NotifyChatMember()
        {
            foreach (KeyValuePair<IChatMember, string> member in members)
            {
                member.Key.Notify(member.Key);
            }
        }
    }
}
