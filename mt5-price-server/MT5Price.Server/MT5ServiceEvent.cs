using System;
using MetaQuotes.MT5CommonAPI;
using MetaQuotes.MT5ManagerAPI;
using System.Collections.Generic;
using MT5Price.Server.Model;

namespace MT5Price.Server
{
    class MT5ServiceEvent : CIMTTickSink
    {
        private CIMTManagerAPI m_manager;
        private RabbitmqService rabbitmqService;
        private Dictionary<string, SymbolFliter> symbolFliters;

        public MT5ServiceEvent()
        {

        }

        /*
         * Initialize m_manager, rabbitmq and symbol filters
         */
        public void Initialize(CIMTManagerAPI m_manager, RabbitmqService rabbitmqService, Dictionary<string, SymbolFliter> symbolFliters)
        {
            this.m_manager = m_manager;
            this.rabbitmqService = rabbitmqService;
            this.symbolFliters = symbolFliters;
            RegisterSink();
        }
        public override void OnTick(string symbol, MTTickShort tick)
        {
            SymbolFliter outSymbol = new SymbolFliter();
            bool result = symbolFliters.TryGetValue(symbol, out outSymbol);
            if (result)
            {
                Price price = new Price
                {
                    ask = tick.ask,
                    bid = tick.bid,
                    last = tick.last,
                    datetime_s = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    datetime = tick.datetime,
                    datetime_msc = tick.datetime_msc,
                    symbol = symbol,
                    name = outSymbol.SymbolName,
                    flags = (int)tick.flags,
                    volume = tick.volume,
                    digits = outSymbol.Digits,
                    volume_ext = tick.volume_ext

                };
                rabbitmqService.Send(price);
            }
        }
    }
}
