using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi.Modules
{
    public class Nlu
    {
        public List<string> tokens { get; set; }
        public List<object> entities { get; set; }
    }
}
