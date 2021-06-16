using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Jupiter.Repository;
using Jupiter.Repository.Models;

namespace Jupiter.Commands
{
    class TimeCommands
    {
        public static async Task UtcTime(SocketMessage msg)
        {
            Console.WriteLine("sending utc time");
            DateTime UTC = DateTime.UtcNow;
            var emb = new EmbedBuilder();
            emb.WithTitle("**UTC time and date: **" + UTC.ToString());
            await msg.Channel.SendMessageAsync("", false, emb.Build());
        }

        public static async Task React(SocketMessage msg)
        {
            string text = msg.Content.Replace("$time", "").Trim();

            if (string.IsNullOrEmpty(text))
                await MsgAuthorTime(msg);
            else if (text.ToLower() == "everyone")
                await TimeEveryone(msg);
        }

        private static async Task MsgAuthorTime(SocketMessage msg)
        {
            var repo = new UserTimeRepository();
            IEnumerable<UserTimeModel> foundUsers = (await repo.GetAllEntries()).Where(x => x.Mention == msg.Author.Mention);

            var emb = new EmbedBuilder();

            //If collection found nothing, add to database
            if (!foundUsers.Any())
            {
                emb.WithTitle("Listing time for " + msg.Author.Username + ".");
                emb.WithDescription(DateTime.UtcNow.ToLongTimeString());

                await repo.AddEntry(new UserTimeModel() { Mention = msg.Author.Mention, TimeZone = "UTC", Username = msg.Author.Username });
            }
            else if (foundUsers.Count() == 1)
            {
                UserTimeModel user = foundUsers.First();
                emb.WithTitle("Listing time for " + msg.Author.Username);

                emb.WithDescription((DateTime.UtcNow + user.GetTimeSpan()).ToLongTimeString());
            }

            await msg.Channel.SendMessageAsync(null, false, emb.Build());
        }

        private static async Task TimeEveryone(SocketMessage msg)
        {
            var repo = new UserTimeRepository();
            List<UserTimeModel> users = (await repo.GetAllEntries()).ToList();

            var emb = new EmbedBuilder();
            emb.WithTitle("List of recorded users:");

            string output = "";

            foreach(UserTimeModel u in users)
            {
                output += u.Username + " - " + u.TimeZone + "\n";
            }

            emb.WithDescription(output);

            await msg.Channel.SendMessageAsync(null, false, emb.Build());
        }
    }
}
