using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Request
    {
        public string command { get; set; }
        public string original_utterance { get; set; }
        public string type { get; set; }
    }
}
