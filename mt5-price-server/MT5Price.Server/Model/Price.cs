using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT5Price.Server.Model
{
    class Price
    {
        public string datetime_s
        { get; set; }
        public long datetime
        { get; set; }
        public long datetime_msc
        { get; set; }
        public string symbol
        { get; set; }
        public string name
        { get; set; }
        public double ask
        { get; set; }
        public double bid
        { get; set; }
        public ulong volume
        { get; set; }
        public ulong volume_ext
        { get; set; }
        public double last
        { get; set; }
        public int flags
        { get; set; }
        public string accountCode
        { get; set; }
        public int digits
        { get; set; }
        public string platform
        { get; set; }
    }
}
