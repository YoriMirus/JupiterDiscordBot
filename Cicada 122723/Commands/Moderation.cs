using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("warn")]
        public async Task Warn(SocketUser user, [Remainder]string warning = null)
        {
            EmbedBuilder emb = new EmbedBuilder();

            if (Context.Message.Author.Username == "TheRedLord")
            {
                Console.WriteLine("the warning: " + warning);


                //msg.Channel.DeleteMessageAsync(msg);
                if (Context.Message.Content.Contains("for"))
                {
                    /*
                    foreach (char c in command)
                    {
                        if (c != ' ')
                        {
                            warning += c;

                        }
                        else
                        {
                            break;
                        }
                        if (start == true)
                        {
                            warning += c;
                        }
                        if (get_reason == "for")
                        {
                            reason += c;
                        }
                        if (c == '<')
                        {
                            start = true;
                            warning += c;

                        }
                        else if (c == '>')
                        {
                            start = false;
                        }
                        if (c == 'f')
                        {
                            get_reason += c;
                        }
                        else if (get_reason == "f")
                        {
                            get_reason += c;
                        }
                        else if (get_reason == "fo")
                        {
                            get_reason += c;
                        }
                    }

                    Console.WriteLine("reason: " + reasonForWarning);
                    emb = new EmbedBuilder();
                    emb.WithColor(Discord.Color.DarkBlue);
                    emb.AddField("This is a warning", warning + " you have been warned for" + reasonForWarning);
                    await msg.Channel.SendMessageAsync("", false, emb.Build());
                    Console.WriteLine(warning);
                    warning = "";
                    reasonForWarning = "";
                    get_reason = "";
                    */
                }
                else
                {
                    if (warning == null)
                    {
                        await ReplyAsync(Context.Message.Author.Mention + " please specify the reason");
                        return;
                    }

                    emb = new EmbedBuilder();
                    emb.AddField("User", $"{user.Mention}You have been warned by " + Context.Message.Author.Mention);
                    emb.AddField("Warning", $"You have been warned for: {warning}");
                    emb.WithColor(Color.Red);
                    await ReplyAsync("", false, emb.Build());
                }
            }
            else
            {
                await ReplyAsync(@"⛔" + Context.Message.Author.Mention + " you dont have the right role" + @" ⛔");
            }
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

        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
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
