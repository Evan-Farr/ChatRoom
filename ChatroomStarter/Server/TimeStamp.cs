using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TimeStamp
    {
        private static DateTime currentDateTime;

        public static DateTime CurrentDateTime { get { return currentDateTime; } }

        public TimeStamp()
        {
            DateTime currentDateTime = DateTime.Now;
        }

        internal int Count()
        {
            throw new NotImplementedException();
        }
    }
}
