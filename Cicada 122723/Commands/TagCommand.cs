using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jupiter;

namespace Jupiter.Commands
{
    /// <summary>
    /// Class that handles commands made by tagging the bot
    /// </summary>
    static class TagCommand
    {
        public static async Task TagBot(SocketMessage source)
        {
            Helper.ColorWrite(source.Content.Length.ToString(), ConsoleColor.Red);
            Random random = new Random();
            string[] greetings = { "hello", "hi", "hey", "sup" };
            string[] checkup = { "how are you", "hbu", "how about you", "whats up", "what's up", "sup" };
            string[] response_to_checkup = { "How about you?", "And you?", "hbu?", "wbu?" };
            string[] response_to_checkupQuestion = { "I'm Great!", "I'm awesome", "I feel amazing", "I am fine", "I'm good", "I never felt better", "Im' in best mood I ever had" };
            for (int i = 0; i < Program.insults.Length; i++)
            {
                if (source.Content.ToLower().Trim().Contains(Program.insults[i]))
                {
                    Console.WriteLine("insult");
                    string message = source.Content;
                    source.Channel.SendMessageAsync(Program.cat_thumbs_up.ToString());
                }
            }

            if (source.Content.Length < 32)
            {

                for (int x = 0; x < greetings.Length; x++)
                {
                    if (source.Content.ToLower().Contains(greetings[x]))
                    {
                        var include_nickname = random.Next(2);
                        var random_greeting = random.Next(greetings.Length);

                        if (include_nickname == 1)
                        {
                            await source.Channel.SendMessageAsync(greetings[random_greeting] + " " + (source.Author as SocketGuildUser).Nickname);
                        }
                        else
                        {
                            await source.Channel.SendMessageAsync(greetings[random_greeting] + " ");
                        }
                        return;
                    }
                }

                for (int z = 0; z < checkup.Length; z++)
                {
                    if (source.Content.Contains(checkup[z]))
                    {
                        var random_checkup_response = random.Next(response_to_checkupQuestion.Length);
                        var random_checup_followup_question = random.Next(response_to_checkup.Length);
                        await source.Channel.SendMessageAsync(response_to_checkupQuestion[random_checkup_response] + ", " + response_to_checkup[random_checup_followup_question]);
                        return;
                    }
                }
            }
            else if (source.Content.Length > 32)
            {
                string final_messege = "";
                for (int x = 0; x < greetings.Length; x++)
                {
                    if (source.Content.ToLower().Contains(greetings[x].ToLower()))
                    {
                        final_messege += greetings[x] + ", ";
                    }


                }
                for (int z = 0; z < checkup.Length; z++)
                {
                    if (source.Content.ToLower().Contains(checkup[z]))
                    {
                        var random_checkup_response = random.Next(response_to_checkupQuestion.Length);
                        var random_checup_followup_question = random.Next(response_to_checkup.Length);
                        final_messege += response_to_checkupQuestion[random_checkup_response] + ". " + response_to_checkup[random_checup_followup_question];
                        await source.Channel.SendMessageAsync(final_messege);
                        return;
                    }
                }

            }

        }

    }
}
