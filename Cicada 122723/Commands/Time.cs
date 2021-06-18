using Discord;
using Discord.Commands;
using Jupiter.Repository;
using Jupiter.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Time : ModuleBase<SocketCommandContext>
    {
        private readonly UserTimeRepository _userTimeRepository;

        public Time(UserTimeRepository userTimeRepository)
        {
            _userTimeRepository = userTimeRepository;
        }

        [Command("utc")]
        public async Task UtcTime()
        {
            Console.WriteLine("sending utc time");
            DateTime UTC = DateTime.UtcNow;
            var emb = new EmbedBuilder();
            emb.WithTitle("**UTC time and date: **" + UTC.ToString());
            await Context.Channel.SendMessageAsync("", false, emb.Build());
        }

        [Command("time")]
        public async Task TimeCommand(string text = null)
        {
            if (string.IsNullOrEmpty(text))
                await MsgAuthorTime();
            else if (text.ToLower() == "everyone")
                await TimeEveryone();
        }

        private async Task MsgAuthorTime()
        {
            var repo = new UserTimeRepository();
            IEnumerable<UserTimeModel> foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == Context.Message.Author.Mention);

            var emb = new EmbedBuilder();

            //If collection found nothing, add to database
            if (!foundUsers.Any())
            {
                emb.WithTitle("Listing time for " + Context.Message.Author.Username + ".");
                emb.WithDescription(DateTime.UtcNow.ToLongTimeString());

                await repo.AddEntry(new UserTimeModel() { Mention = Context.Message.Author.Mention, TimeZone = "UTC", Username = Context.Message.Author.Username });
            }
            else if (foundUsers.Count() == 1)
            {
                UserTimeModel user = foundUsers.First();
                emb.WithTitle("Listing time for " + Context.Message.Author.Username);

                emb.WithDescription((DateTime.UtcNow + user.GetTimeSpan()).ToLongTimeString());
            }

            await Context.Message.Channel.SendMessageAsync(null, false, emb.Build());
        }

        private async Task TimeEveryone()
        {
            var repo = new UserTimeRepository();
            List<UserTimeModel> users = (await repo.GetAllEntries()).ToList();

            var emb = new EmbedBuilder();
            emb.WithTitle("List of recorded users:");

            string output = "";

            foreach (UserTimeModel u in users)
            {
                output += u.Username + " - " + u.TimeZone + "\n";
            }

            emb.WithDescription(output);

            await Context.Message.Channel.SendMessageAsync(null, false, emb.Build());
        }

        [Command("set-tz")]
        public async Task SetTimeZone(string timezone = null)
        {
            if (timezone == null)
            {
                await ReplyAsync("Please provide a UTC offset");
            }

            var repo = new UserTimeRepository();
            var foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == Context.Message.Author.Mention);
            UserTimeModel userModel;

            if (!foundUsers.Any())
            {
                //Add an entry and then get all of its info especially ID.
                await repo.AddEntry(new UserTimeModel() { Username = Context.Message.Author.Username, Mention = Context.Message.Author.Mention, TimeZone = "UTC" });
                foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == Context.Message.Author.Mention);
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

            var emb = new EmbedBuilder();

            //Check if timezone is parsable
            if (UserTimeModel.ValidateTimeZoneString(timezone))
            {
                userModel.TimeZone = timezone.Replace("utc", "");
                await repo.EditEntry(userModel);
            }
            else
            {
                emb.WithTitle("Failed to parse.");
                await Context.Message.Channel.SendMessageAsync(null, false, emb.Build());
                return;
            }

            emb.WithTitle("Timezone changed.");
            emb.WithDescription("Current time set is: " + (DateTime.UtcNow + userModel.GetTimeSpan()).ToLongTimeString() + ".");

            await Context.Channel.SendMessageAsync(null, false, emb.Build());

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
