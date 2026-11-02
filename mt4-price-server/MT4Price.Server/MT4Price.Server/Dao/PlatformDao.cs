using MT4Price.Server.Model;
using System;
using MySql.Data.MySqlClient;
using log4net;

namespace MT4Price.Server.Dao
{
    class PlatformDao : MySQLService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PlatformDao));

        /**
         * Singleton
         */
        private static readonly Lazy<PlatformDao> lazy = new Lazy<PlatformDao>(() => new PlatformDao());
        public static PlatformDao Instance { get { return lazy.Value; } }
        private PlatformDao() {}


        /*
         *  Get Account Code
         */
        public Response findByAccountCode(string accountCode, out PltPlatform plt)
        {
            //default
            plt = new PltPlatform();
            plt.Host = null;
            plt.Port = 0;
            plt.Login = 0;
            plt.Password = null;
            plt.PlatformType = null;

            Response response = new Response();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT b.* FROM acc_account a ";
                    query += "inner JOIN plt_platform b ON a.account_id = b.platform_id ";
                    query += "WHERE a.account_code = @accountCode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Prepare();
                    using (MySqlDataReader rs = cmd.ExecuteReader())
                    {
                        if (rs.Read())
                        {
                            plt.Host = rs.GetString("host");
                            plt.Port = rs.GetInt32("port");
                            plt.Login = rs.GetInt32("login");
                            plt.Password = rs.GetString("password");
                            plt.PlatformType = rs.GetString("platform_type");

                            response.Status = 1;
                            response.Message = "";
                        }
                        else
                        {
                            response.Status = 0;
                            response.Message = accountCode + " Not Found";
                        }
                    }
                }
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.Status = 0;
            }
            return response;
        }
    }
}
