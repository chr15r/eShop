using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace eShop.DataStore.SQL.Dapper.Helpers
{
    public class DataAccess : IDataAccess
    {
        private readonly string connectionString;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public T QuerySingle<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.QuerySingle<T>(sql, parameters);
            }
        }

        public T QueryFirst<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirst<T>(sql, parameters);
            }
        }

        public List<T> Query<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<T>(sql, parameters).ToList();
            }
        }

        public void ExecuteCommand<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, parameters);
            }
        }
    }

}
