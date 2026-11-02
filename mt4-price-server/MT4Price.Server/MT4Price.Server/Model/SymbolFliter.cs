using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT4Price.Server.Model
{
    class SymbolFliter
    {
        public int SymbolId
        { get; set; }

        public string Platform
        { get; set; }

        public string SymbolCode
        { get; set; }

        public string SymbolName
        { get; set; }

        public int IsActive
        { get; set; }

        public int AccountId
        { get; set; }
    }
}
