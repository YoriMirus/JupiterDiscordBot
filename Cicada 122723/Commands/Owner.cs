using Discord.Commands;
using Discord.WebSocket;
using Jupiter.Helpers;
using System;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Owner : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;

        public Owner(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("activity")]
        [RequireOwner]
        public async Task Activity(string msg = null)
        {
            Helper.ColorWrite("the author is redlord", ConsoleColor.Red);

            if (msg == null)
            {
                Helper.ColorWrite("the activity is empty", ConsoleColor.Red);
                await ReplyAsync("please specify what activity I should play");
                return;
            }
            else
            {
                Helper.ColorWrite("setting up the activity...", ConsoleColor.Red);
                await _client.SetGameAsync(msg, null, Discord.ActivityType.Playing);
                return;
            }
        }
    }
}
