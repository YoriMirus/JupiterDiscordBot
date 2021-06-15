using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    class GoogleCommand
    {
        public static async Task GoogleResults(SocketMessage msg)
        {
            var message = msg.Content.Trim().Replace("$google ", "").Replace(" ", "+");
            EmbedBuilder google = new EmbedBuilder();
            google.WithColor(Discord.Color.Green);
            google.WithTitle("Google results");
            google.WithDescription($"I searched '{message.Replace("+", " ")}' for you: " + "https://www.google.com/search?q=" + message);
            //google.WithUrl();
            await msg.Channel.SendMessageAsync(null, false, google.Build());
        }
    }
}
