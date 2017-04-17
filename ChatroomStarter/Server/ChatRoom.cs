using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ChatRoom
    {
        private string id;
        private string roomName;
        private string topic;
        private int userCount;
        private bool open;
        private long timeStarted;

        public string Id { get { return id; } set { id = value; } }
        public string RoomName { get { return roomName; } set { roomName = value; } }
        public string Topic { get { return topic; } set { topic = value; } }
        public int UserCount { get { return userCount; } set { userCount = value; } }
        public bool Open { get { return open; } set { open = value; } }
        public long TimeStarted { get { return timeStarted; } set { timeStarted = value; } }

        public ChatRoom(string RoomName, string Topic)
        {
            id = SetRoomId();
            roomName = RoomName;
            topic = Topic;
            userCount = 0;
            open = true;
            TimeStamp timeStarted = new TimeStamp();
        }

        private string SetRoomId()
        {
            return id;
        }
    }
}
