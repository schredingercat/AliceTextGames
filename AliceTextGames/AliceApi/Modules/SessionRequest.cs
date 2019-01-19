using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi
{
    public class SessionRequest
    {
        public bool New { get; set; }
        public string session_id { get; set; }
        public int message_id { get; set; }
        public string skill_id { get; set; }
        public string user_id { get; set; }
    }
}
