using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using Dapper;

using Jupiter.Repository.Models;
using Jupiter.Repository.Interfaces;

namespace Jupiter.Repository
{
    abstract class Repository<T> : IRepository<T> where T : DatabaseObject
    {
        public abstract Task AddEntry(T entry);
        public abstract Task EditEntry(T entry);
        public abstract Task RemoveEntry(int ID);
        public abstract Task RemoveEntry(T entry);
        public abstract Task<T> GetEntry(int ID);
        public abstract Task<IEnumerable<T>> GetAllEntries();

        protected string ConnectionString
        {
            get
            {
                var str = new SqliteConnectionStringBuilder();
                str.DataSource = "Database.db";
                return str.ToString();
            }
        }

        /// <summary>
        /// Makes a query to the database. Returns null if the query fails for any reason.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<T>> Query(string sql)
        {
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    return await connection.QueryAsync<T>(sql);
                }
            }
            catch
            {
                return null;
            }
        }

        protected async Task<T> QueryFirstMatching(string sql)
        {
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    return await connection.QueryFirstAsync<T>(sql);
                }
            }
            catch
            {
                return null;
            }
        }

        protected async Task Execute(string sql)
        {
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    await connection.ExecuteAsync(sql);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
