using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AliceRequest
    {
        public Meta meta { get; set; }
        public Request request { get; set; }
        public SessionRequest session { get; set; }
        public string version { get; set; }
    }
}
