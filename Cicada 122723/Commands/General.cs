using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Jupiter.Repository;
using Jupiter.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private static Random _random = new();
        private readonly UserTimeRepository _userTimeRepository;

        public General(UserTimeRepository userTimeRepository)
        {
            _userTimeRepository = userTimeRepository;
        }

        [Command("users")]
        public async Task Users()
        {
            var users = await Context.Channel.GetUsersAsync(CacheMode.AllowDownload).FlattenAsync();
            Console.WriteLine(users.Count());
            string all_users = "";
            string online_users = "";
            string offline_users = "";
            string bots = "";
            foreach (IUser user in users)
            {
                Helper.ColorWriteLine(user.Status.ToString(), ConsoleColor.DarkBlue);
                if (user.IsBot)
                {
                    bots += user.Username + " **(bot)**";
                    bots += "\n";
                }
                else
                {

                    if (string.IsNullOrEmpty((user as SocketGuildUser).Nickname))
                    {
                        if (user.Status.ToString().ToLower() == "offline")
                        {
                            offline_users += user.Username + " 🔴" + "\n"; //red emoji
                        }
                        else
                        {
                            online_users += user.Username + " 🟢" + "\n"; //🟢 green circle                       
                        }
                    }
                    else
                    {
                        if (user.Status.ToString().ToLower() == "offline")
                        {
                            offline_users += user.Username + " (aka: " + ((user as SocketGuildUser).Nickname) + ")" + " 🔴" + "\n"; //red emoji
                        }
                        else
                        {
                            online_users += user.Username + " (aka: " + ((user as SocketGuildUser).Nickname) + ")" + " 🟢" + "\n"; //🟢 green circle                       
                        }
                    }


                    //await user.GetOrCreateDMChannelAsync();
                    //await user.SendMessageAsync($@"hello {user.Username}");
                }

            }
            all_users += "**online** \n" + online_users + "**offline** \n" + offline_users + "**bots** \n" + bots;
            Console.WriteLine(all_users);
            await ReplyAsync(all_users);
        }

        [Command("registered")]
        [Alias("registered_users")]
        public async Task Display()
        {
            var repository = new UserTimeRepository();
            var users = await repository.GetAllEntries();

            var emb = new EmbedBuilder();

            string result = "";

            foreach (UserTimeModel m in users)
            {
                result = result + "\n" + m.Username;
            }
            emb = new EmbedBuilder();
            emb.WithColor(Color.Gold);
            emb.WithTitle("Showing List of Users");
            emb.WithDescription(result);
            await Context.Channel.SendMessageAsync("", false, emb.Build());
        }

        [Command("say")]
        public async Task say(string say = null)
        {
            if(say == null)
            {
                await ReplyAsync("What would you like me to say?");
                return;
            }

            await ReplyAsync(say);
        }

        [Command("random")]
        [Alias("rand")]
        public async Task Random(int low, int high)
        {
            if (low > high)
            {
                await ReplyAsync("Please put the min number befor the max number");
                return;
            }

            //var itemArr = items.Split(",", StringSplitOptions.None);
            await Context.Channel.TriggerTypingAsync();
            //var chosenOne = itemArr[_random.Next(itemArr.Length)];

            await ReplyAsync(_random.Next(low, high + 1).ToString());

            //This is way more complex than needed...
            //string minIntString = "", maxIntString = "";
            //string message = Context.Message.Content.Replace("$rand", "").Replace(" ", "");

            //int minInt;
            //int maxInt;

            //bool passedTheBarrier = false;
            //if (message.Contains(",") == false)
            //{
            //    await Context.Channel.SendMessageAsync("Please include ',' in the command");
            //}
            //else
            //{
            //    foreach (char ch in message)
            //    {
            //        Helper.ColorWrite("I am about to analyze the char: '" + ch.ToString() + "'", ConsoleColor.Green);
            //        if (passedTheBarrier == false)
            //        {

            //            if (ch != ',' && string.IsNullOrWhiteSpace(ch.ToString()) == false)
            //            {
            //                minIntString += ch.ToString();
            //            }
            //            else if (ch == ',')
            //            {
            //                passedTheBarrier = true;
            //            }

            //        }
            //        else
            //        {
            //            if (string.IsNullOrWhiteSpace(ch.ToString()) == false)
            //            {
            //                maxIntString += ch.ToString();
            //            }

            //        }
            //    }

            //    try
            //    {
            //        minInt = int.Parse(minIntString);
            //        maxInt = int.Parse(maxIntString);

            //        if (minInt > maxInt)
            //        {
            //            await Context.Channel.SendMessageAsync("Please enter the minimum number before the maximum number");
            //        }
            //        else
            //        {
            //            Random random = new Random();
            //            var answer = random.Next(minInt, maxInt + 1);
            //            EmbedBuilder randomizer = new EmbedBuilder();
            //            randomizer.WithColor(Discord.Color.Blue);
            //            randomizer.AddField("Random number between " + minInt + " and " + maxInt, answer);
            //            await Context.Channel.SendMessageAsync(answer.ToString());
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        Helper.ColorWrite(e.Message, ConsoleColor.Red);
            //        await Context.Channel.SendMessageAsync("Please input number only");
            //    }
            //}
        }
    }
}
