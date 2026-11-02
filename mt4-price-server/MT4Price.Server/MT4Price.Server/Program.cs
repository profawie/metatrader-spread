using System;
using System.Configuration;
using System.Threading;
using Figgle;
using log4net;
using log4net.Config;

namespace MT4Price.Server
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ScheduleService));

        static void Main(string[] args)
        {

            XmlConfigurator.Configure();
            string accountCode = ConfigurationManager.AppSettings["Account_Code"];
            if (accountCode == null)
            {
                log.Error("Account_Code required");
                return;
            }
            Console.WriteLine(FiggleFonts.Standard.Render("MT4 Price Server "+accountCode));
            ScheduleService scheduleService = new ScheduleService();
            while (true)
            {   
                if (!ScheduleService.Running)
                {   
                    scheduleService.Start();
                }
                if (ScheduleService.Running && !scheduleService.IsConnected())
                {
                    scheduleService.Stop();
                }
                Thread.Sleep(200);
            }
        }
    }
}
