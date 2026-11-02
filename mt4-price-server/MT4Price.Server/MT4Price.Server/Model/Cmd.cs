using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT4Price.Server.Model
{
    class Cmd
    {
        public string action 
        { get; set; }

        public Dictionary<string,object> values
        { get; set; }
    }
}
