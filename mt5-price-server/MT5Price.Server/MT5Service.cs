using System;
using System.Collections.Generic;
using log4net;
using MT5Price.Server.Model;
using MetaQuotes.MT5CommonAPI;
using MetaQuotes.MT5ManagerAPI;


namespace MT5Price.Server
{
    class MT5Service
    {
        private CIMTManagerAPI m_manager = null;
        
        public MT5ServiceEvent sink = new MT5ServiceEvent();
        //--- connect timeout in milliseconds
        private uint MT5_CONNECT_TIMEOUT = 30000;
        private bool isInitialize = false;
        private RabbitmqService rabbitmqService = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(MT5Service));

        public MT5Service(RabbitmqService rabbitmqService)
        {
            this.rabbitmqService = rabbitmqService;
        }

        public Response Connect(PltPlatform platform, Dictionary<string, SymbolFliter> symbolFliters)
        {
            Response response = new Response();
            MTRetCode res = MTRetCode.MT_RET_OK_NONE;

            //Valid Platform Type
            if (platform.PlatformType != "MT5")
            {
                response.Status = (int)MTRetCode.MT_RET_ERROR;
                response.Message = "Only Platform Type MT5 allow";
                return response;
            }

            //Valid MT5APIManager64.dll
            if (!isInitialize)
            {
                if ((res = SMTManagerAPIFactory.Initialize(null)) != MTRetCode.MT_RET_OK)
                {
                    String message = string.Format("Loading manager API failed, MT5APIManager64.dll required ({0})", res);
                    response.Status = (int)MTRetCode.MT_RET_ERR_PARAMS;
                    response.Message = message;
                    return response;
                }

                //Create instance m_manager
                m_manager = SMTManagerAPIFactory.CreateManager(SMTManagerAPIFactory.ManagerAPIVersion, out res);
                if ((res != MTRetCode.MT_RET_OK) || (m_manager == null))
                {
                    SMTManagerAPIFactory.Shutdown();
                    String message = string.Format("CreateManager() failed ({0})", (res == MTRetCode.MT_RET_OK ? "Managed API is null" : res.ToString()));
                    response.Status = (int)res;
                    response.Message = message;
                    return response;
                }
                isInitialize = true;
            }

            //Connect
            ulong login = ((ulong)platform.Login);
            res = m_manager.Connect(platform.Host + ":" + platform.Port, login, platform.Password, null, CIMTManagerAPI.EnPumpModes.PUMP_MODE_FULL, MT5_CONNECT_TIMEOUT);
            if (res != MTRetCode.MT_RET_OK)
            {
                String message = string.Format("Connection failed ({0})", res);
                m_manager.LoggerOut(EnMTLogCode.MTLogErr, message);
                response.Status = (int)res;
                response.Message = message;
                return response;
            }

            //Start Pumping
            return Pumping(platform, symbolFliters);

        }


        private Response Pumping(PltPlatform platform, Dictionary<string, SymbolFliter> symbolFliters)
        {
            Response response = new Response();
            MTRetCode res = MTRetCode.MT_RET_OK_NONE;

            //Open webAPI
            MT5WebAPIService mT5WebAPI = new MT5WebAPIService();
            response = mT5WebAPI.Connect(platform);
            if(response.Status != (int)MTRetCode.MT_RET_OK)
            {
                return response;
            }

            //Pre load All Symbol
            foreach (KeyValuePair<string, SymbolFliter> symbol in symbolFliters)
            {
                MetaQuotes.MT5WebAPI.Common.MTTick tick = new MetaQuotes.MT5WebAPI.Common.MTTick();
                response = mT5WebAPI.TickLast(symbol.Key, out tick);
                if (response.Status != (int)MTRetCode.MT_RET_OK)
                {
                    log.Debug(symbol.Key + " failed " + MTRetCode.MT_RET_OK);
                }
                else
                {
                    int digits = symbol.Value.Digits;
                    if(digits == 0)
                    {
                        digits = 2;
                    }
                    Price price = new Price
                    {
                        ask = tick.Ask,
                        bid = tick.Bid,
                        last = tick.Last,
                        datetime_s = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        symbol = symbol.Key,
                        name = symbol.Value.SymbolName,
                        flags = 0,
                        volume = 0,
                        digits = digits,
                        volume_ext = 0

                    };
                    log.Debug(symbol.Key + " added");
                    rabbitmqService.Send(price);
                }
            }

            //Add Symbol before start Pumping 
            foreach (KeyValuePair<string, SymbolFliter> symbol in symbolFliters)
            {
                //update digits
                MetaQuotes.MT5WebAPI.Common.MTConSymbol mTConSymbol = new MetaQuotes.MT5WebAPI.Common.MTConSymbol();
                mT5WebAPI.SymbolGet(symbol.Key, out mTConSymbol);
                symbol.Value.Digits = (int)mTConSymbol.Digits;
                
                res = m_manager.SelectedAdd(symbol.Key);
                if (res != MTRetCode.MT_RET_OK)
                {
                    String message = string.Format("symbol {0} not found ({1})", symbol.Key, res);
                    log.Error(message);
                }
            }


            //Start pumping
            sink.Initialize(m_manager, rabbitmqService, symbolFliters);
            res = m_manager.TickSubscribe(sink);
            if (res != MTRetCode.MT_RET_OK)
            {
                String message = string.Format("TickSubscribe() failed ({0})", res);
                m_manager.LoggerOut(EnMTLogCode.MTLogErr, message);
                response.Status = (int)res;
                response.Message = message;
                return response;
            }



            //Success
            response.Status = ((int)res);
            response.Message = "";
            return response;
        }


        public void Diconnected()
        {
            if (m_manager != null)
            {
                m_manager.Disconnect();
                m_manager.Dispose();
                m_manager = null;
                isInitialize = false;
            }
        }
    }
}
