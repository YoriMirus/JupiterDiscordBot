using Discord;
using Discord.WebSocket;
using ICanHazDadJoke.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class JokeCommand
    {
        public static async Task React(SocketMessage msg)
        {
            Helper.ColorWriteLine(RandomJoke.Jokes.JokeAsync("smtn", "smtn").Result.ToString(), ConsoleColor.Magenta);
            DadJokeClient dad_joke = new DadJokeClient("something here I dont even know what supposed to be here");
            string joke = dad_joke.GetRandomJokeStringAsync().Result;

            var emb = new EmbedBuilder();
            emb.WithTitle("Want a joke? How about this one: ");
            emb.WithDescription(joke);
            emb.WithColor(Color.Blue);

            msg.Channel.SendMessageAsync("", false, emb.Build());
            Helper.ColorWriteLine(joke, ConsoleColor.Red);
        }
    }
}
