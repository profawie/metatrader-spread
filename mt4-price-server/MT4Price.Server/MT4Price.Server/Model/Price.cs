using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT4Price.Server.Model
{
    class Price
    {
        public string platform { get; set; }
        public string accountCode { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int comm_type { get; set; }
        public double commission { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double ask { get; set; }
        public double bid { get; set; }
        public int lasttime { get; set; }
        public string datetime { get; set; }
        public int updateflag { get; set; }
        public int spreadBalance { get; set; }
        public int spread { get; set; }
        public double point { get; set; }
        public int type { get; set; }
        public int visible { get; set; }
        public int count { get; set; }
        public int digits { get; set; }
        public int direction { get; set; }
    }
}
