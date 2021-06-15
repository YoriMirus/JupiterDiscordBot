using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public static class DeleteCommand
    {
        public static async Task Delete(SocketMessage msg)
        {
            var emb = new EmbedBuilder();

            if (msg.Author.Username == "General Baka")
            {
                emb = new EmbedBuilder();
                emb.WithTitle("lol no");
                await msg.Channel.SendMessageAsync("", false, emb.Build());
            }
            else
            {
                string amount_str = msg.Content.Replace(" ", "");
                amount_str = amount_str.Replace("$delete", "");
                Console.WriteLine(":{0}:", amount_str);
                int amount = int.Parse(amount_str);
                if (amount < 100)
                {
                    var messages = await msg.Channel.GetMessagesAsync(amount + 1).FlattenAsync(); //defualt is 100

                    await (msg.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
                }
                else
                {
                    emb.WithTitle("Please Do Not Delete More Than 100 Messages Per Time");
                    await msg.Channel.SendMessageAsync("", false, emb.Build());
                }

            }

            //await msg.Channel.SendMessageAsync("test: "  );
        }
    }
}
