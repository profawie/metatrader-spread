using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMT4Manager
{

    class MT4Thread
    {
        private bool run = false;
        MT4Service mt4Service = null;
        public bool isRun()
        {
            return run;
        }


        public void onComplete(List<SymbolInfo> ticks)
        {
            //run = false;
            //Console.WriteLine("done - " + ticks.Count);
        }

        public void Run()
        {
            mt4Service = new MT4Service(onComplete);
            run = true;
        }


    }
}
