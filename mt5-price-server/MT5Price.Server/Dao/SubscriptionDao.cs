using System;
using MySql.Data.MySqlClient;
using log4net;

namespace MT5Price.Server.Dao
{
    class SubscriptionDao : MySQLService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SubscriptionDao));

        /**
         * Singleton
         */
        private static readonly Lazy<SubscriptionDao> lazy = new Lazy<SubscriptionDao>(() => new SubscriptionDao());
        public static SubscriptionDao Instance { get { return lazy.Value; } }
        private SubscriptionDao() { }


        /*
         *  Is Expired
         */
        public bool IsExpired(string accountCode)
        {
            bool expired = true;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT COUNT(a.txn_id) as cnt FROM txn_subscription a ";
                    query += "INNER JOIN acc_account b ON a.account_id = b.account_id ";
                    query += "WHERE b.account_code = @accountCode AND a.is_active = 1 and a.deleted = 0";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Prepare();
                    using (MySqlDataReader rs = cmd.ExecuteReader())
                    {
                        if (rs.Read())
                        {
                            int cnt = rs.GetInt32("cnt");
                            if (cnt >= 1)
                            {
                                expired = false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                expired = false;
            }
            return expired;
        }
    }
}
