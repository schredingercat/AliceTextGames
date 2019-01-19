using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi
{
    public class Response
    {
        public string text { get; set; }
        public string tts { get; set; }
        public bool end_session { get; set; }

        public Response()
        {
            text = string.Empty;
            tts = string.Empty;
        }
    }
}
