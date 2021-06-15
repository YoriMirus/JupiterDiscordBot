using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

using Jupiter.Database;

namespace Jupiter
{
    public class DataAccess
    {
        public List<UserModel> getting_info;
        public List<UserModel> GetUser(string sql)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.connectionString("sampleDB")))
            {
                getting_info = connection.Query<UserModel>(sql).ToList();
                return getting_info;
            }
        }
    }
}
