using MT5Price.Server.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MT5Price.Server.Dao
{
    class SymbolFliterDao : MySQLService
    {
        /**
        * Singleton
        */
        private static readonly Lazy<SymbolFliterDao> lazy = new Lazy<SymbolFliterDao>(() => new SymbolFliterDao());
        public static SymbolFliterDao Instance { get { return lazy.Value; } }
        private SymbolFliterDao() { }

        /*
         *  Get Symbol List
         */
        public Response findAllByAccountCode(string accountCode, out Dictionary<string, SymbolFliter> symbolFliters)
        {
            //default
            symbolFliters = new Dictionary<string, SymbolFliter>();

            Response response = new Response();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    string query = "select * from symbol_fliter a ";
                    query += "inner join acc_account b on b.account_id = a.account_id ";
                    query += "where is_active = 1 and b.account_code = @accountCode and platform = 'MT5'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Prepare();
                    using (MySqlDataReader rs = cmd.ExecuteReader())
                    {
                        bool exist = false;
                        while (rs.Read())
                        {
                            exist = true;
                            SymbolFliter sf = new SymbolFliter();
                            sf.Platform = rs.GetString("platform");
                            sf.SymbolCode = rs.GetString("symbol_code");
                            sf.SymbolName = rs.GetString("symbol_name");
                            sf.IsActive = rs.GetInt32("is_active");
                            symbolFliters.Add(sf.SymbolCode, sf);

                            response.Status = 1;
                            response.Message = "";
                        }
                        if (!exist)
                        {
                            response.Status = 0;
                            response.Message = " Not Found";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Status = 0;
            }
            return response;
        }
    }
}
