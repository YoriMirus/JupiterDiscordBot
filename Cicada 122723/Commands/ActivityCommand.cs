using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    class ActivityCommand
    {
        public static async Task Activity(SocketMessage msg, DiscordSocketClient cicada_client)
        {
            if (msg.Author.Username == "TheRedLord")
            {
                Helper.ColorWrite("the author is redlord", ConsoleColor.Red);
                string Activity = msg.Content.Replace("$activity ", "");
                if (string.IsNullOrEmpty(Activity))
                {
                    Helper.ColorWrite("the activity is empty", ConsoleColor.Red);
                    await msg.Channel.SendMessageAsync("please specify what activity I should play");
                }
                else
                {
                    Helper.ColorWrite("setting up the activity...", ConsoleColor.Red);
                    await cicada_client.SetGameAsync(Activity, null, Discord.ActivityType.Playing);
                }
            }
            else { await msg.Channel.SendMessageAsync("I am sorry " + msg.Author.Username + " But you WILL NOT MANAGE MY TIME AND HOW I DECIDE TO SPEND IT!"); }

        }
    }
}
