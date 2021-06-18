using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        public Moderation()
        {

        }

        [Command("edit")]
        public async Task Edit(string edit = null)
        {
            if(edit == null)
            {
                await ReplyAsync("What should I edit the message to?");
                return;
            }

            var message = await Context.Channel.SendMessageAsync("test");
            var messageToSend = Context.Message.Content.Replace("$edit", "");
            await message.ModifyAsync(x => x.Content = edit);
        }

        [Command("delete")]
        public async Task Delete(int number)
        {
            var emb = new EmbedBuilder();

            if (Context.Message.Author.Username == "General Baka")
            {
                emb = new EmbedBuilder();
                emb.WithTitle("lol no");
                await Context.Channel.SendMessageAsync("", false, emb.Build());
            }
            else
            {
                Console.WriteLine(":{0}:", number);
                if (number < 100)
                {
                    var messages = await Context.Channel.GetMessagesAsync(number + 1).FlattenAsync(); //defualt is 100

                    await (Context.Message.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
                }
                else
                {
                    emb.WithTitle("Please Do Not Delete More Than 100 Messages Per Time");
                    await Context.Channel.SendMessageAsync("", false, emb.Build());
                }

            }

            var message = await ReplyAsync($"Deleted {number} messages");
            await Task.Delay(4000);
            await message.DeleteAsync();
        }
    }
}
