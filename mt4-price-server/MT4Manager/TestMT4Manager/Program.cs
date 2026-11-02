using Manager;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestMT4Manager
{
    class Program
    {
      
        static void Main(string[] args)
        {

            MT4Thread thread = new MT4Thread();
            while(true)
            {
                if(!thread.isRun())
                {
                    thread.Run();
                }
                Thread.Sleep(100);
            }
        }
    }
}
