using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class RandCommand
    {
        public static async Task React(SocketMessage msg)
        {
            string minIntString = "", maxIntString = "";
            string message = msg.Content.Replace("$rand", "").Replace(" ", "");

            int minInt;
            int maxInt;

            bool passedTheBarrier = false;
            if (message.Contains(",") == false)
            {
                await msg.Channel.SendMessageAsync("Please include ',' in the command");
            }
            else
            {
                foreach (char ch in message)
                {
                    Helper.ColorWrite("I am about to analyze the char: '" + ch.ToString() + "'", ConsoleColor.Green);
                    if (passedTheBarrier == false)
                    {

                        if (ch != ',' && string.IsNullOrWhiteSpace(ch.ToString()) == false)
                        {
                            minIntString += ch.ToString();
                        }
                        else if (ch == ',')
                        {
                            passedTheBarrier = true;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(ch.ToString()) == false)
                        {
                            maxIntString += ch.ToString();
                        }

                    }
                }

                try
                {
                    minInt = int.Parse(minIntString);
                    maxInt = int.Parse(maxIntString);

                    if (minInt > maxInt)
                    {
                        msg.Channel.SendMessageAsync("Please enter the minimum number before the maximum number");
                    }
                    else
                    {
                        Random random = new Random();
                        var answer = random.Next(minInt, maxInt + 1);
                        EmbedBuilder randomizer = new EmbedBuilder();
                        randomizer.WithColor(Discord.Color.Blue);
                        randomizer.AddField("Random number between " + minInt + " and " + maxInt, answer);
                        msg.Channel.SendMessageAsync(answer.ToString());
                    }

                }
                catch (Exception e)
                {
                    Helper.ColorWrite(e.Message, ConsoleColor.Red);
                    msg.Channel.SendMessageAsync("Please input number only");
                }
            }


        }
    }
}
