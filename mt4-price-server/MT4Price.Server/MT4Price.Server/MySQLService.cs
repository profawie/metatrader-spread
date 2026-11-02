using System.Configuration;

namespace MT4Price.Server
{
    class MySQLService
    {

        private string mysqlHost = null;
        private int mysqlPort = 0;
        private string mysqlLogin = null;
        private string mysqlPassword = null;
        private string mysqlDatabase = null;

        public MySQLService()
        {
            mysqlHost = ConfigurationManager.AppSettings["Mysql_Host"];
            mysqlPort = int.Parse(ConfigurationManager.AppSettings["Mysql_Port"]);
            mysqlLogin = ConfigurationManager.AppSettings["Mysql_Login"];
            mysqlPassword = ConfigurationManager.AppSettings["Mysql_Password"];
            mysqlDatabase = ConfigurationManager.AppSettings["Mysql_Database"];
        }

        public string GetConnectionString()
        {
            return "server=" + mysqlHost + ";user=" + mysqlLogin + ";database=" + mysqlDatabase + ";port=" + mysqlPort + ";password=" + mysqlPassword;
        }
    }
}
