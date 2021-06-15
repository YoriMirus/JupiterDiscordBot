using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jupiter.Repository;
using Jupiter.Repository.Models;

namespace Jupiter.Commands
{
    public static class SetTimeZoneCommand
    {
        public static async Task SetTimeZone(SocketMessage msg)
        {
            string timezone = msg.Content.Replace("$set-tz ", "");

            var repo = new UserTimeRepository();
            var foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == msg.Author.Mention);
            UserTimeModel userModel;

            if (!foundUsers.Any())
            {
                //Add an entry and then get all of its info especially ID.
                await repo.AddEntry(new UserTimeModel() { Username = msg.Author.Username, Mention = msg.Author.Mention, TimeZone = "UTC" });
                foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == msg.Author.Mention);
                userModel = foundUsers.First();

                /*userModel = data_access.GetUser($"INSERT INTO discord_users_info(username,tag,tz) VALUES ( '{msg.Author.Username}', '{msg.Author.Mention}', '{timezone}')");
                emb.WithColor(Discord.Color.LightOrange);
                emb.WithTitle("New time zone set for: " + msg.Author.Username);
                await msg.Channel.SendMessageAsync("", false, emb.Build());
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("new time zone {0} for {1} tag = {2}", timezone, msg.Author.Username, msg.Author.Mention);
                Console.ForegroundColor = ConsoleColor.White;*/
            }
            else
                userModel = foundUsers.First();

            string argument = msg.Content.Replace("$set-tz", "").Trim().ToLower();
            var emb = new EmbedBuilder();

            //Check if timezone is parsable
            if (UserTimeModel.ValidateTimeZoneString(argument))
            {
                userModel.TimeZone = argument.Replace("utc", "");
                await repo.EditEntry(userModel);
            }
            else
            {
                emb.WithTitle("Failed to parse.");
                await msg.Channel.SendMessageAsync(null, false, emb.Build());
                return;
            }

            emb.WithTitle("Timezone changed.");
            emb.WithDescription("Current time set is: " + (DateTime.UtcNow + userModel.GetTimeSpan()).ToLongTimeString() + ".");

            await msg.Channel.SendMessageAsync(null, false, emb.Build());

            /*else
            {
                userModel = data_access.GetUser($"SELECT * FROM discord_users_info WHERE tag = '{msg.Author.Mention}'");
                foreach (UserModel user in data_access.getting_info)
                {
                    string old_tz = user.TimeZone;
                }
                userModel = data_access.GetUser($"UPDATE discord_users_info SET tz = '{timezone}' WHERE tag = '{msg.Author.Mention}'");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Timezone updated for user {0} with tag {1} the old timezone: {2} the new timezone {3}", msg.Author.Username, msg.Author.Mention, old_tz, timezone);
                Console.ForegroundColor = ConsoleColor.White;
                emb.WithColor(Discord.Color.LightOrange);
                emb.WithTitle("Timezone for " + msg.Author.Username + " updated");
                await msg.Channel.SendMessageAsync("", false, emb.Build());
            }*/


        }
    }
}
