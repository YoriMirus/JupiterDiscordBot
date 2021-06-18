using Discord;
using Discord.Commands;
using ICanHazDadJoke.NET;
using Reddit;
using Reddit.Things;
using System;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        private static Random _random = new();

        private static readonly string[] positiveAnswers = { "It is certain.", "It is decidedly so.", "Without a doubt.", "Yes – definitely.", "You may rely on it.", "As I see it, yes.", "Most likely.", "Outlook good.", "Yes.", "Signs point to yes." };
        private static readonly string[] neutralAnswers = { "Reply hazy, try again.", "Ask again later.", "Better not tell you now.", "Cannot predict now.", "Concentrate and ask again." };
        private static readonly string[] negativeAnswers = { "Don’t count on it.", "My reply is no.", "My sources say no.", "Outlook not so good.", "Very doubtful." };

        public Fun()
        {
            
        }

        [Command("joke")]
        public async Task Joke()
        {
            Helper.ColorWriteLine(RandomJoke.Jokes.JokeAsync("smtn", "smtn").Result.ToString(), ConsoleColor.Magenta);
            DadJokeClient dad_joke = new DadJokeClient("Jupiter");
            string joke = dad_joke.GetRandomJokeStringAsync().Result;

            var emb = new EmbedBuilder();
            emb.WithTitle("Want a joke? How about this one: ");
            emb.WithDescription(joke);
            emb.WithColor(Color.Blue);

            await Context.Channel.SendMessageAsync("", false, emb.Build());
            Helper.ColorWriteLine(joke, ConsoleColor.Red);
        }

        [Command("meme")]
        public async Task Meme()
        {
             RedditClient reddit = new RedditClient(appId: "PD5JKTuzXSHPKg", appSecret: "dyOW25zWq6U7R8MJ2yViFhWB6q2zYw", userAgent: "agent", refreshToken: "343096396262-PFKJkpA5b07x7M94c62xktAi1ZwbTg", accessToken: "343096396262-B48ocXKOnv5yAtVxCb1i3MNCI5-4RA");
            //Get posts
            var hotPosts = reddit.Subreddit("Memes").Posts.Hot.ToArray();
            Post post = new Post(hotPosts[_random.Next(hotPosts.Length)]);

            //Debug
            Console.WriteLine(post.Title.ToString() + "::");
            Helper.ColorWrite(post.URL?.ToString(), ConsoleColor.Red);

            //Create embed
            EmbedBuilder memeBuilder = new EmbedBuilder();
            memeBuilder.WithTitle(post.Title);
            memeBuilder.WithColor(Color.DarkRed);
            memeBuilder.WithUrl(post.URL);
            memeBuilder.WithDescription("meme");

            await ReplyAsync(null, false, memeBuilder.Build());
            Console.WriteLine(hotPosts[1].Score);
        }
    

        [Command("RussianRoulette")]
        [Summary("Bang bang, man")]
        [Alias("rr")]
        public async Task RussianRoulette()
        {
            //Create embed
            EmbedBuilder answer = new EmbedBuilder();
            answer.WithThumbnailUrl(@"https://cdn.britannica.com/95/176195-050-D0FA0BC1/Smith-Wesson-revolver.jpg");
            answer.WithColor(Color.Red);

            //Get random number
            var bullet = Helper.GetRandomNumber(1, 7);

            //Check for win/loss
            if (bullet == 6)
            {
                answer.AddField(Context.User.Username + " clicks the trigger and...", "BOOM! " + Context.User.Username + " shot himself right in the head!");
            }
            else
            {
                answer.AddField(Context.User.Username + " clicks the trigger and...", "CLICK! nothing happens and " + Context.User.Username + " stays alive!");
            }

            await Context.Channel.SendMessageAsync(null, false, answer.Build());
            Helper.ColorWrite(bullet.ToString(), ConsoleColor.Red);
        }

        [Command("8ball")]
        [Summary("Magic 8ball")]
        public async Task EightBall([Remainder] string msg = null)
        {
            if (msg == null)
            {
                await ReplyAsync("You didn't ask a question!");
                return;
            }

            Random r = Helper.GetRandom();
            int answerKind = r.Next(1, 5);
            var BallAnswer = new EmbedBuilder();

            string answer = "";

            Helper.ColorWrite(answerKind.ToString(), ConsoleColor.Red);

            if (answerKind <= 2)
            {
                BallAnswer.WithColor(Color.Green);
                answer = positiveAnswers[r.Next(0, 9)];
            }
            else if (answerKind == 3 || answerKind == 4)
            {
                BallAnswer.WithColor(Color.Blue);
                answer = negativeAnswers[r.Next(0, 4)];
            }
            else if (answerKind == 5)
            {
                BallAnswer.WithColor(Color.Default);
                answer = neutralAnswers[r.Next(0, 4)];
            }

            BallAnswer.AddField("8 Ball pool says...", answer);
            await ReplyAsync(null, false, BallAnswer.Build());
        }
    }
}
