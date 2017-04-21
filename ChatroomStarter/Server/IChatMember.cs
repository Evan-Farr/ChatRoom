using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IChatMember
    {
        string UserName { get; set; }
        string UserId { get; set; }
        void Notify(IChatMember members, string status);
        void Send(string message);
    }
}
