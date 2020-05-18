using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class DBConnection
    {
        private static string connectionString = @"Data Source=localhost;
                                                    Initial Catalog=EZ_Dealer;
                                                    Integrated Security=True;
                                                    Connect Timeout=5";

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
