﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private string userName;
        private string recievedMessageString;
        public ILogger log;

        public string UserId { get { return userId; } set { userId = value; } }
        public string UserName { get { return userName; } set { userName = value; } }

        public Client(NetworkStream Stream, TcpClient Client, ILogger log)
        {
            stream = Stream;
            client = Client;
            UserId = SetUserId();
            userName = SetUserName();
            this.log = log;
        }

        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            try
            {
                stream.Write(message, 0, message.Count());
            }
            catch
            {
                DateTime currentDateTime = DateTime.Now;
                Console.WriteLine(currentDateTime.ToString());
                Console.WriteLine($"**** {userName} left the chat. ****\n\n");
                Disconnect(client);
                LogDisconnect();
                AlertDisconnect(client);
            }
        }

        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                try
                {
                    stream.Read(recievedMessage, 0, recievedMessage.Length);
                }
                catch
                {
                    DateTime thisCurrentDateTime = DateTime.Now;
                    Console.WriteLine(thisCurrentDateTime.ToString());
                    Console.WriteLine($"**** {userName} left the chat. ****\n\n");
                    Disconnect(client);
                    LogDisconnect();
                    AlertDisconnect(client);
                    break;
                }
                string recievedMessageString = Encoding.ASCII.GetString(recievedMessage).Trim('\0');
                Message message = new Message(this, recievedMessageString);
                Server.messageQueue.Enqueue(message);
                DateTime currentDateTime = DateTime.Now;
                Console.WriteLine(currentDateTime.ToString());
                Console.WriteLine($">> {userName}: " + recievedMessageString);
                Console.WriteLine();
            }
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

        public string SetUserName()
        {
            Send("Enter your desired display name for this chat...");
            byte[] recievedMessage = new byte[256];
            try
            {
                stream.Read(recievedMessage, 0, recievedMessage.Length);
            }
            catch
            {
                DateTime currentDateTime = DateTime.Now;
                Console.WriteLine(currentDateTime.ToString());
                Console.WriteLine("**** Unknown person tried to join the chat but quite before joining. ****\n\n");
                log.Log("**** Unknown person tried to join the chat but quite before joining. ****\n\n");
                Disconnect(client);
            }
            recievedMessageString = Encoding.ASCII.GetString(recievedMessage).Trim('\0');
            foreach (KeyValuePair<IChatMember, string> member in Server.members)
            {
                if (member.Value.Equals(recievedMessageString))
                {
                    Send("That name is not available.");
                    SetUserName();
                }
            }
            userName = recievedMessageString;
            return userName;
        }

        public void Disconnect(TcpClient client)
        {
            foreach (KeyValuePair<IChatMember, string> member in Server.members)
            {
                if (member.Value.Equals(userName))
                {
                    Server.members.Remove(member.Key);
                    break;
                }
            }
            stream.Close();
        }

        public void LogDisconnect()
        {
            log.Log($"---- {userName} left the chat. ----\n\n");
        }

        public void AlertDisconnect(TcpClient client)
        {
            Server.NotifyChatMember(Server.client, "left");
        }

        public void Notify(IChatMember member, string status)
        {
            DateTime currentDateTime = DateTime.Now;
            Send(currentDateTime.ToString());
            Send($"**** {member.UserName} {status} the chat. ****\n");
        }
    }
}
