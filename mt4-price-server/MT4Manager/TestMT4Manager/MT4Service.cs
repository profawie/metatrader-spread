using System;
using System.Collections.Generic;
using Manager;

namespace TestMT4Manager
{

    class MT4Service
    {
        private static MeTatraderManager mt4 = null;
        private OnComplete onComplete;
     
        public MT4Service(OnComplete onComplete)
        {
            this.onComplete = onComplete;
            mt4 = new MeTatraderManager();
            mt4.Connect("127.0.0.1:443");
            mt4.Login(1, "manager-password");
            mt4.TickInfoLast("AUSUSD");            
            mt4.PumpingSwitchEx(CBFunc,0, 0);
        }

        ~MT4Service()
        {
            mt4.Disconnect();
            mt4 = null;
        }

        private void CBFunc(PumpingMessages code, TransTypes type, IntPtr data, IntPtr param)
        {
            if (code == PumpingMessages.PUMP_UPDATE_BIDASK)
            {
                List<SymbolInfo> symbolInfo = mt4.SymbolInfoUpdated();
                if(symbolInfo.Count > 0)
                {
                    Console.WriteLine("symbol: " + symbolInfo[0].symbol+", ask: "+symbolInfo[0].ask+", bid: "+symbolInfo[0].bid);
                }
                
            }
        }
    }
}
