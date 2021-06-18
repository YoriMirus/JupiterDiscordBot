using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Web : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        public async Task GoogleResults(string msg)
        {
            var message = Context.Message.Content.Trim().Replace("$google ", "").Replace(" ", "+");
            EmbedBuilder google = new EmbedBuilder();
            google.WithColor(Discord.Color.Green);
            google.WithTitle("Google results");
            google.WithDescription($"I searched '{message.Replace("+", " ")}' for you: " + "https://www.google.com/search?q=" + message);
            //google.WithUrl();
            await Context.Channel.SendMessageAsync(null, false, google.Build());
        }
    }
}
