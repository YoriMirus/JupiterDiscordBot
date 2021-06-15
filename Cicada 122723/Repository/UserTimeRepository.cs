using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jupiter.Repository.Models;

namespace Jupiter.Repository
{
    class UserTimeRepository : Repository<UserTimeModel>
    {
        public async override Task AddEntry(UserTimeModel entry)
        {
            await Execute($"INSERT INTO UserTimeModel(Username, Mention, Timezone) VALUES('{entry.Username}', '{entry.Mention}', '{entry.TimeZone}');");
        }

        public async Task AddEntry(string username, string mention, string timeZone)
        {
            await Execute($"INSERT INTO UserTimeModel(Username, Mention, Timezone) VALUES('{username}', '{mention}', '{timeZone}');");
        }

        public async override Task EditEntry(UserTimeModel entry)
        {
            string sql = $"UPDATE UserTimeModel SET " +
                         $"Username = '{entry.Username}', " +
                         $"Mention = '{entry.Mention}', " +
                         $"TimeZone = '{entry.TimeZone}' " +
                         $"WHERE ID = {entry.ID};";

            await Execute(sql);
        }

        public async override Task RemoveEntry(int ID)
        {
            await Execute($"DELETE FROM UserTimeModel WHERE ID = {ID};");
        }

        public async override Task RemoveEntry(UserTimeModel entry)
        {
            string sql = $"DELETE FROM UserTimeModel WHERE" +
                         $"Username = '{entry.Username}'," +
                         $"Mention = '{entry.Mention}'," +
                         $"TimeZone = '{entry.TimeZone}';";

            await Execute(sql);
        }

        public async override Task<UserTimeModel> GetEntry(int ID)
        {
            return await QueryFirstMatching($"SELECT * FROM UserTimeModel WHERE ID = {ID};");
        }

        public async override Task<IEnumerable<UserTimeModel>> GetAllEntries()
        {
            return await Query("SELECT * FROM UserTimeModel");
        }
    }
}
