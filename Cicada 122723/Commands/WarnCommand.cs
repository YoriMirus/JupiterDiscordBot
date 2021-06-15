using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class WarnCommand
    {
        static string warning;
        static string get_reason;
        static string reason;
        static string reasonForWarning;
        static bool start;

        public static async Task Warn(SocketMessage msg)
        {
            EmbedBuilder emb = new EmbedBuilder();

            if (msg.Author.Username == "TheRedLord")
            {
                string command = msg.Content.Replace("$warn ", "");

                Console.WriteLine("the warning: " + warning);


                //msg.Channel.DeleteMessageAsync(msg);
                if (msg.Content.Contains("for"))
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
                    msg.Channel.SendMessageAsync(msg.Author.Mention + " please specify the reason");
                    emb = new EmbedBuilder();
                    emb.AddField("[USER MENTION]", "You have been warned by " + msg.Author.Mention);
                    emb.WithColor(Color.Red);
                    msg.Channel.SendMessageAsync("", false, emb.Build());
                }
            }
            else
            {
                await msg.Channel.SendMessageAsync(@"⛔" + msg.Author.Mention + " you dont have the right role" + @" ⛔");
            }
        }

    }
}
