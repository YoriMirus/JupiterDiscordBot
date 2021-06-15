using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class RussianRoulette
    {
        public static async Task React(SocketMessage msg)
        {
            //Create embed
            EmbedBuilder answer = new EmbedBuilder();
            answer.WithThumbnailUrl(@"https://cdn.britannica.com/95/176195-050-D0FA0BC1/Smith-Wesson-revolver.jpg");
            answer.WithColor(Color.Red);

            //Get random number
            var bullet = Helper.GetRandomNumber(1, 7);

            //Get username
            string username;
            if (string.IsNullOrEmpty((msg.Author as SocketGuildUser).Nickname))
            {
                username = msg.Author.Username;
            }
            else
            {
                username = (msg.Author as SocketGuildUser).Nickname;
            }

            //Check for win/loss
            if (bullet == 6)
            {
                answer.AddField(username + " clicks the trigger and...", "BOOM! " + username + " shot himself right in the head!");
            }
            else
            {
                answer.AddField(username + " clicks the trigger and...", "CLICK! nothing happens and " + username + " stays alive!");
            }

            await msg.Channel.SendMessageAsync(null, false, answer.Build());
            Helper.ColorWrite(bullet.ToString(), ConsoleColor.Red);
        }
    }
}
