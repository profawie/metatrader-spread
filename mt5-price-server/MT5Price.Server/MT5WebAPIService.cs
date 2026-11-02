using System;
using System.Collections.Generic;
using log4net;
using MetaQuotes.MT5WebAPI;
using MetaQuotes.MT5WebAPI.Common;
using MT5Price.Server.Model;

namespace MT5Price.Server
{
    public class MT5WebAPIService
    {

        private MT5WebAPI api;
        private static readonly ILog log = LogManager.GetLogger(typeof(MT5WebAPIService));

        /*
         * Connect
         */ 
        public Response Connect(PltPlatform platform)
        {
            Response response = new Response();
            api = new MT5WebAPI();
            string mt5Host = platform.Host;
            int mt5Port = platform.Port;
            ulong mt5Login = (ulong)platform.Login;
            string mt5Password = platform.Password;

            //connect
            MTRetCode result = api.Connect(mt5Host, mt5Port, mt5Login, mt5Password, MT5WebAPI.EnPumpModes.PUMP_MODE_NONE);
            if (result != MTRetCode.MT_RET_OK)
            {
                response.Status = (int)MTRetCode.MT_RET_ERROR;
                response.Message = "Only Platform Type MT5 allow";
                Console.Write("Fail connect to MT5 API [" + mt5Host + "] error: " + result);
                return response;
            }

            response.Status = (int)MTRetCode.MT_RET_OK;
            response.Message = "";
            return response;
        }


        /*
         * Get Symbol
         */ 
        public Response SymbolGet(string symbol, out MTConSymbol mTConSymbol)
        {
            mTConSymbol = new MTConSymbol();
            Response response = new Response();
            if (api == null)
            {
                response.Status = (int)MTRetCode.MT_RET_ERROR;
                response.Message = "MT5 not initialize";
                return response;
            }
            MTRetCode result = api.SymbolGet(symbol, out mTConSymbol);
            if(result != MTRetCode.MT_RET_OK)
            {
                response.Status = (int)MTRetCode.MT_RET_ERR_NOTFOUND;
                response.Message = "SymbolGet: Symbol not found";
                return response;
            }
            response.Status = (int)MTRetCode.MT_RET_OK;
            response.Message = "";
            return response;

        }


        /**
         * Get TickLast
         */ 
        public Response TickLast(string symbol, out MTTick tick)
        {
            tick = new MTTick();
            Response response = new Response();
            if (api == null)
            {
                response.Status = (int)MTRetCode.MT_RET_ERROR;
                response.Message = "MT5 not initialize";
                return response;
            }
            List<MTTick> stats = new List<MTTick>();
            MTRetCode res = api.TickLast(symbol, out stats);

            if (res != MTRetCode.MT_RET_OK)
            {
                response.Status = (int)res;
                response.Message = "TickLast: Symbol not found";
                return response;
            }
            if (stats.Count == 0)
            {
                response.Status = (int)res;
                response.Message = "TickLast: empty";
                return response;
            }
            tick = stats[0];

            response.Status = (int)MTRetCode.MT_RET_OK;
            response.Message = "";
            return response;
        }

        /*
         * Disconnect
         */ 
        public void Disconnect()
        {
            if (api != null)
            {
                api.Disconnect();
                api = null;
            }
        }
    }
}
