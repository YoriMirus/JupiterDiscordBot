using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jupiter.Repository;
using Jupiter.Repository.Models;
using Discord;

namespace Jupiter.Commands
{
    public static class RegisteredUsersCommand
    {
        public static async Task Display(SocketMessage msg)
        {
            var repository = new UserTimeRepository();
            var users = await repository.GetAllEntries();

            var emb = new EmbedBuilder();

            string result = "";

            foreach (UserTimeModel m in users)
            {
                result = result + "\n" + m.Username;
            }
            emb = new EmbedBuilder();
            emb.WithColor(Color.Gold);
            emb.WithTitle("Showing List of Users");
            emb.WithDescription(result);
            await msg.Channel.SendMessageAsync("", false, emb.Build());
        }
    }
}
