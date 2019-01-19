using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi
{
    public class AliceResponse
    {
        private const string Version = "1.0";

        public Response response { get; set; }
        public SessionResponse session { get; set; }
        public string version { get; set; }

        public AliceResponse()
        {
            response = new Response();
            session = new SessionResponse();
            version = Version;
        }
    }
}
