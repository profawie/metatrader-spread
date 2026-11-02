using System;
using System.Threading;
using Figgle;
using log4net.Config;
using log4net;
using System.Configuration;

namespace MT5Price.Server
{
    class Program
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            string accountCode = ConfigurationManager.AppSettings["Account_Code"];
            if (accountCode == null)
            {
                log.Error("Account_Code required");
                return;
            }
            XmlConfigurator.Configure();
            Console.WriteLine(FiggleFonts.Standard.Render("MT5 Price Server ["+accountCode+"]"));
            ScheduleService scheduleService = new ScheduleService();
            scheduleService.Start();
            Console.ReadLine();
            
        }
    }
}
