using log4net;
using MT4Price.Server.Dao;
using MT4Price.Server.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace MT4Price.Server
{
    class ScheduleService
    {
        public static bool Expired = false;
        public static bool Running = false;
        private MT4Service mt4Service = null;
        private RabbitmqService rabbitmqService = null;
        private PlatformDao platformDao = PlatformDao.Instance;
        private SymbolFliterDao symbolFliterDao = SymbolFliterDao.Instance;
        private SubscriptionDao subscriptionDao = SubscriptionDao.Instance;
        private static readonly ILog log = LogManager.GetLogger(typeof(ScheduleService));
        private bool firstTime = true;

        public bool IsConnected()
        {
            return mt4Service.IsConnected();
        }


        public void Start()
        {
            //Start
            Running = true;
            log.Info("Connecting to MT4, RabbitMQ & MySQL Service");

            if(firstTime)
            {
                Thread.Sleep(5000); //delay for 5 seconds
                firstTime = false;
            }
            

            //Get Platform Configuration
            string accountCode = ConfigurationManager.AppSettings["Account_Code"];
            if(accountCode == null)
            {
                log.Error("Account_Code required");
                Running = false;
                return;
            }

            //Get Platform
            PltPlatform platform = new PltPlatform();
            Response response  = platformDao.findByAccountCode(accountCode, out platform);
            if (response.Status == 2)
            {
                log.Error("findByAccountCode ["+ accountCode + "] not found");
                Running = false;
                return;
            }
            else if(response.Status == 0)
            {
                log.Error("Fail Connect to MySQL Service - " + response.Message);
                Running = false;
                Thread.Sleep(5000); //delay for 5 seconds
                return;
            }

            //Get Symbol List
            Dictionary<string, SymbolFliter> symbolFliters = new Dictionary<string, SymbolFliter>();
            response = symbolFliterDao.findAllByAccountCode(accountCode, out symbolFliters);
            if (response.Status == 2)
            {
                log.Error("findByAccountCode [" + accountCode + "] not found");
                Running = false;
                return;
            }
            else if (response.Status == 0)
            {
                log.Error("Fail Connect to MySQL Service - " + response.Message);
                Running = false;
                return;
            }

            log.Info("Account, Symbol [" + accountCode +"] Loaded ");

            //Verify Expired
            Expired = subscriptionDao.IsExpired(accountCode);
            log.Info("Expired? " + (Expired ? "Yes" : "No"));

            //RabbitMQ Service
            rabbitmqService = new RabbitmqService();
            response = rabbitmqService.Connect(accountCode);
            if (response.Status == 0)
            {
                log.Error(response.Message);
                Running = false;
                return;
            }

            //MT4 Service
            mt4Service = new MT4Service(rabbitmqService);
            response = mt4Service.Connect(platform, symbolFliters);
            if (response.Status != 0)
            {
                log.Error(response.Message);
                Running = false;
                return;
            }
            log.Info("MT4 Service Started");

            log.Info("RabbtMQ Service [" + rabbitmqService.MQNAME_ACTION + ", "+ rabbitmqService.MQNAME_QUEUE + "] Started");

        }


        public void Stop()
        {
            //Stop
            Running = true;
            log.Info("Stopping MT4, RabbitMQ & MySQL Service");
            if(mt4Service != null)
            {
                mt4Service.Diconnected();
            }
            log.Info("MT4 Stopped");

            if (rabbitmqService != null)
            {
                rabbitmqService.Diconnected();
            }
            log.Info("RabbitMQ Stopped");

            Running = false;
            log.Info("MySQL Stopped");

        }
    }
}
