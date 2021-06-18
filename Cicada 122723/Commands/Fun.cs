using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        private readonly string[] positiveAnswers = { "It is certain.", "It is decidedly so.", "Without a doubt.", "Yes – definitely.", "You may rely on it.", "As I see it, yes.", "Most likely.", "Outlook good.", "Yes.", "Signs point to yes." };
        private readonly string[] neutralAnswers = { "Reply hazy, try again.", "Ask again later.", "Better not tell you now.", "Cannot predict now.", "Concentrate and ask again." };
        private readonly string[] negativeAnswers = { "Don’t count on it.", "My reply is no.", "My sources say no.", "Outlook not so good.", "Very doubtful." };

        [Command("8ball")]
        [Summary("Magic 8ball")]
        public async Task EightBall([Remainder] string msg = null)
        {
            if (msg == null)
            {
                await ReplyAsync("You didn't ask a question!");
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
