using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class RockPaperScissors
    {
        static Discord.Rest.RestUserMessage message;
        public static string RpcMessageId;
        public static bool GameOver { get; private set; }

        public static async Task Start(SocketMessage msg)
        {
            message = await msg.Channel.SendMessageAsync("Loading game...");
            var Rock = new Discord.Emoji(@"🪨");
            var Scissors = new Discord.Emoji(@"✂️");
            var Paper = new Discord.Emoji(@"🧻");
            await message.AddReactionAsync(Rock);
            await message.AddReactionAsync(Scissors);
            await message.AddReactionAsync(Paper);
            await message.ModifyAsync(msg => msg.Content = "Choose Rock Paper or Scissors!");
            RpcMessageId = message.Id.ToString();
            GameOver = true;
        }

        public static async Task OnUserInput(SocketReaction userInput, ISocketMessageChannel originChannel)
        {
            var user = (originChannel as SocketGuildChannel).GetUser(userInput.UserId);
            var username = user.Username;

            var RockPaperScissors = Helper.GetRandomNumber(1, 3);
            if (userInput.Emote.Name.Trim() == @"🪨")
            {
                if (RockPaperScissors == 1)
                {
                    await message.ModifyAsync(msg => msg.Content = "Choose Rock Paper or Scissors! \n 🪨 vs 🪨 \n TIE!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 2)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n 🪨 vs ✂️ \n {username} You Win!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 3)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n 🪨 vs 🧻 \n {username} You Lost!");
                    GameOver = false;
                }

            }
            else if (userInput.Emote.Name.Trim() == @"✂️")
            {
                if (RockPaperScissors == 1)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n ✂️ vs 🪨 \n {username} You Lost!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 2)
                {
                    await message.ModifyAsync(msg => msg.Content = "Choose Rock Paper or Scissors! \n ✂️ vs ✂️ \n TIE!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 3)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n ✂️ vs 🧻 \n {username} You Win!");
                    GameOver = false;
                }
            }
            else if (userInput.Emote.Name == @"🧻")
            {
                if (RockPaperScissors == 1)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n 🧻 vs 🪨 \n  {username} You Win!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 2)
                {
                    await message.ModifyAsync(msg => msg.Content = $"Choose Rock Paper or Scissors! \n 🧻 vs ✂️ \n {username} You Lost!");
                    GameOver = false;
                }
                else if (RockPaperScissors == 3)
                {
                    await message.ModifyAsync(msg => msg.Content = "Choose Rock Paper or Scissors! \n 🧻 vs 🧻 \n TIE!");
                    GameOver = false;
                }
            }
            else
            {
                Helper.ColorWrite("WRONG", ConsoleColor.Red);
            }

        }
    }
}
