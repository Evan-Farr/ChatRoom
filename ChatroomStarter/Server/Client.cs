using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client /*: IObservable<Message>*/
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        //private List<IObserver<Message>> users;
        //private List<Message> messages;


        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
            //users = new List<IObserver<Message>>();
            //messages = new List<Message>();
        }

        //public IDisposable Subscribe(IObserver<Message> user)
        //{
        //    if(!users.Contains(user))
        //    {
        //        users.Add(user);
        //        foreach(var message in messages)
        //        {
        //            user.OnNext(message);
        //        }
        //    }
        //    return new UserId<Message>(users, user);
        //}

        public void Send(string Message)
        {
            //TimeStamp timeSent = new TimeStamp();
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }

        public void Recieve()
        {
            //TimeStamp timeRecieved = new TimeStamp();
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            Message message = new Message(null, recievedMessageString);
            Server.messageQueue.Enqueue(message);
            Console.WriteLine(recievedMessageString);
        }

        //IDisposable IObservable<Message>.Subscribe(IObserver<Message> observer)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
