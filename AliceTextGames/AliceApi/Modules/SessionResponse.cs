using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi
{
    public class SessionResponse
    {
        public string session_id { get; set; }
        public int message_id { get; set; }
        public string user_id { get; set; }
    }
}
