using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class Ball
    {
        static readonly string[] positiveAnswers = { "It is certain.", "It is decidedly so.", "Without a doubt.", "Yes – definitely.", "You may rely on it.", "As I see it, yes.", "Most likely.", "Outlook good.", "Yes.", "Signs point to yes." };
        static readonly string[] neutralAnswers = { "Reply hazy, try again.", "Ask again later.", "Better not tell you now.", "Cannot predict now.", "Concentrate and ask again." };
        static readonly string[] negativeAnswers = { "Don’t count on it.", "My reply is no.", "My sources say no.", "Outlook not so good.", "Very doubtful." };

        public static async Task React(SocketMessage msg)
        {
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
            await msg.Channel.SendMessageAsync(null, false, BallAnswer.Build());
        }
    }
}
