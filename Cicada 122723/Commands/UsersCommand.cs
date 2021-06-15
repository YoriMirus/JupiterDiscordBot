using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class UsersCommand
    {
        public static async Task React(SocketMessage msg)
        {
            var users = await msg.Channel.GetUsersAsync(CacheMode.AllowDownload).FlattenAsync();
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
            await msg.Channel.SendMessageAsync(all_users);
        }
    }
}
