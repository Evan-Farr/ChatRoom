using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IChatMembers
    {
        void Notify(IChatMembers members);
    }
}
