using Discord;
using Discord.WebSocket;
using Reddit;
using Reddit.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class MemeCommand
    {
        public static RedditClient reddit = new RedditClient(appId: "PD5JKTuzXSHPKg", appSecret: "dyOW25zWq6U7R8MJ2yViFhWB6q2zYw", userAgent: "agent", refreshToken: "343096396262-PFKJkpA5b07x7M94c62xktAi1ZwbTg", accessToken: "343096396262-B48ocXKOnv5yAtVxCb1i3MNCI5-4RA");
        public static async Task React(SocketMessage msg)
        {
            //Get posts
            var hotPosts = reddit.Subreddit("Memes").Posts.Hot.ToArray();
            Post post = new Post(hotPosts[1]);

            //Debug
            Console.WriteLine(post.Title.ToString() + "::");
            Helper.ColorWrite(post.URL?.ToString(), ConsoleColor.Red);

            //Create embed
            EmbedBuilder memeBuilder = new EmbedBuilder();
            memeBuilder.WithColor(Color.DarkRed);
            memeBuilder.WithUrl(post.URL);
            memeBuilder.WithDescription("meme");

            await msg.Channel.SendMessageAsync(null, false, memeBuilder.Build());
            Console.WriteLine(hotPosts[1].Score);
        }
    }
}
