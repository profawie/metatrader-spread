using System;
using System.Collections.Generic;
using Manager;
using log4net;
using MT4Price.Server.Model;

namespace MT4Price.Server
{
    class MT4Service
    {
        private MeTatraderManager mt4 = null;
        private RabbitmqService rabbitmqService = null;
        private Dictionary<string, SymbolFliter> symbolFliters = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(MT4Service));

        public MT4Service(RabbitmqService rabbitmqService)
        {
            mt4 = new MeTatraderManager();
            this.rabbitmqService = rabbitmqService;
        }

        public bool IsConnected()
        {
            if(mt4 == null)
            {
                return false;
            }
            return mt4.IsConnected();
        }

        public Response Connect(PltPlatform platform, Dictionary<string, SymbolFliter> symbolFliters)
        {
            Response response = new Response();

            //Valid Platform Type
            if(platform.PlatformType != "MT4")
            {
                response.Status = (int)RetValues.RET_ERROR;
                response.Message = "Only Platform Type MT4 allow";
                return response;
            }

            //Valid mtmanapi64.dll
            if (!mt4.IsValid())
            {
                response.Status = (int)RetValues.RET_MALFUNCTION;
				String message = string.Format("Loading manager API failed, MT5APIManager64.dll required ({0})", (int)RetValues.RET_MALFUNCTION);
                response.Message = "Loading manager API failed,  mtmanapi64.dll required ({0})";
                return response;
            }

            //Connect
            RetValues res = mt4.Connect(platform.Host + ":" + platform.Port);
            if (res != RetValues.RET_OK)
            {
                response.Status = ((int)res);
                response.Message = "MT4 Connect Fail - " + mt4.ErrorDescription(res);
                return response;
            }

            //Login
            res = mt4.Login(platform.Login, platform.Password);
            if (res != RetValues.RET_OK)
            {
                response.Status = ((int)res);
                response.Message = "MT4 Login Fail - " + mt4.ErrorDescription(res);
            }

            //Start Pumping
            this.symbolFliters = symbolFliters;
            mt4.PumpingSwitchEx(CBFunc, 0, 0);

            //Success
            response.Status = ((int)res);
            response.Message = "";
            return response;


        }

        public void Diconnected()
        {
            if (mt4 != null)
            {
                mt4.Disconnect();
            }
            mt4 = null;
        }

        ~MT4Service()
        {
            Diconnected();
        }


        private void CBFunc(PumpingMessages code, TransTypes type, IntPtr data, IntPtr param)
        {
            //if expired, ignore
            if(ScheduleService.Expired)
            {
                return;
            }
            if (code == PumpingMessages.PUMP_UPDATE_BIDASK)
            {
                List<SymbolInfo> symbolInfo = mt4.SymbolInfoUpdated();
                if (symbolInfo.Count > 0)
                {
                    SymbolFliter symbol = new SymbolFliter();
                    bool result = symbolFliters.TryGetValue(symbolInfo[0].symbol, out symbol);
                    if(result)
                    {
                        Price price = new Price
                        {
                            name = symbol.SymbolName,
                            symbol = symbolInfo[0].symbol,
                            comm_type = symbolInfo[0].comm_type,
                            commission = symbolInfo[0].commission,
                            low = symbolInfo[0].low,
                            high = symbolInfo[0].high,
                            ask = symbolInfo[0].ask,
                            bid = symbolInfo[0].bid,
                            lasttime = symbolInfo[0].lasttime,
                            datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                            updateflag = symbolInfo[0].updateflag,
                            spreadBalance = symbolInfo[0].spread_balance,
                            spread = symbolInfo[0].spread,
                            point = symbolInfo[0].point,
                            type = symbolInfo[0].type,
                            visible = symbolInfo[0].visible,
                            count = symbolInfo[0].count,
                            digits = symbolInfo[0].digits,
                            direction = symbolInfo[0].direction
                        };
                        rabbitmqService.Send(price);
                    }
                }
            }
        }
    }
}
