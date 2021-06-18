using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    public class TicTacToe : ModuleBase<SocketCommandContext>
    {
        static int move;
        static string winner = "";
        static Discord.Rest.RestUserMessage Message;

        static string TttMessageId;

        public static string[] tictactoe = { "empty", "empty", "empty", "empty", "empty", "empty", "empty", "empty", "empty" };
        //                              1,a      1,b      1,c      2,a      2,b      2,c      3,a      3,b      3,c
        //                               0        1        2        3        4        5        6        7        8

        [Command("tictactoe")]
        [Alias("ttt")]
        public async Task TicTacToeCommand(string content = null)
        {
            var msg = Context.Message;
            if (msg.Author.Id != 782261217334001674 && winner == "")
            {
                //var X = ":x:";
                //var O = ":o:";
                var final_message = "";
                var empty = "             ";
                //var content = msg.Content.Replace("$ttt", "");
                if (string.IsNullOrEmpty(content))
                {
                    await msg.Channel.SendMessageAsync("Grid:" + "\n" + "A,B,C" + "\n" + "1 " + "\n" + "2 " + "\n" + "3 ");
                    Message = await msg.Channel.SendMessageAsync("You can start playing now!" + "\n" + "" + "\n" + "" + "\n" + "");

                    TttMessageId = Message.Id.ToString();
                    return;
                }
                else
                {
                    if (content.Trim().ToLower() == "1,a" || content.Trim().ToLower() == "a,1")
                    {
                        tictactoe[0] = "x";
                    }
                    else if (content.Trim() == "1,b" || content.Trim() == "b,1")
                    {
                        tictactoe[1] = "x";
                    }
                    else if (content.Trim() == "1,c" || content.Trim() == "c,1")
                    {
                        tictactoe[2] = "x";
                    }
                    else if (content.Trim() == "2,a" || content.Trim() == "a,2")
                    {
                        tictactoe[3] = "x";
                    }
                    else if (content.Trim() == "2,b" || content.Trim() == "b,2")
                    {
                        tictactoe[4] = "x";
                    }
                    else if (content.Trim() == "2,c" || content.Trim() == "c,2")
                    {
                        tictactoe[5] = "x";
                    }
                    else if (content.Trim() == "3,a" || content.Trim() == "a,3")
                    {
                        tictactoe[6] = "x";
                    }
                    else if (content.Trim() == "3,b" || content.Trim() == "b,3")
                    {
                        tictactoe[7] = "x";
                    }
                    else if (content.Trim() == "3,c" || content.Trim() == "c,3")
                    {
                        tictactoe[8] = "x";
                    }


                    Random random = new Random();
                    bool emptySpace = false;
                    while (emptySpace == false)
                    {
                        move = random.Next(0, 9);
                        if (tictactoe[move] == "empty")
                        {
                            Helper.ColorWrite("placed at " + move, ConsoleColor.Red);
                            tictactoe[move] = "o";
                            emptySpace = true;
                        }
                    }
                    winner = "";


                    if (tictactoe[0] == tictactoe[1] && tictactoe[1] == tictactoe[2]) // first row
                    {
                        Helper.ColorWrite("1 has been called" + tictactoe[0] + " - " + tictactoe[1] + " - " + tictactoe[2], ConsoleColor.Red);
                        TicTacToeWinner(msg, 1, move);
                    }
                    else if (tictactoe[3] == tictactoe[4] && tictactoe[4] == tictactoe[5]) // second row
                    {
                        Helper.ColorWrite("2 has been called" + tictactoe[3] + " - " + tictactoe[4] + " - " + tictactoe[5], ConsoleColor.Red);
                        TicTacToeWinner(msg, 4, move);
                    }
                    else if (tictactoe[6] == tictactoe[7] && tictactoe[7] == tictactoe[8]) // third row
                    {
                        Helper.ColorWrite("3 has been called" + tictactoe[6] + " - " + tictactoe[7] + " - " + tictactoe[8], ConsoleColor.Red);
                        TicTacToeWinner(msg, 7, move);
                    }
                    else if (tictactoe[0] == tictactoe[3] && tictactoe[3] == tictactoe[6]) // first column
                    {
                        Helper.ColorWrite("4 has been called" + tictactoe[0] + " - " + tictactoe[3] + " - " + tictactoe[6], ConsoleColor.Red);
                        TicTacToeWinner(msg, 3, move);
                    }
                    else if (tictactoe[1] == tictactoe[4] && tictactoe[4] == tictactoe[7]) // second column
                    {
                        Helper.ColorWrite("5 has been called" + tictactoe[1] + " - " + tictactoe[4] + " - " + tictactoe[7], ConsoleColor.Red);
                        TicTacToeWinner(msg, 4, move);
                    }
                    else if (tictactoe[2] == tictactoe[5] && tictactoe[5] == tictactoe[8]) // third column
                    {
                        Helper.ColorWrite("6 has been called" + tictactoe[2] + " - " + tictactoe[5] + " - " + tictactoe[8], ConsoleColor.Red);
                        TicTacToeWinner(msg, 3, move);
                    }
                    else if (tictactoe[0] == tictactoe[4] && tictactoe[4] == tictactoe[8]) // left to right diagonal
                    {
                        Helper.ColorWrite("7 has been called" + tictactoe[0] + " - " + tictactoe[4] + " - " + tictactoe[8], ConsoleColor.Red);
                        TicTacToeWinner(msg, 4, move);
                    }
                    else if (tictactoe[2] == tictactoe[4] && tictactoe[4] == tictactoe[6]) // right to left diagonal
                    {
                        Helper.ColorWrite("8 has been called" + tictactoe[2] + " - " + tictactoe[4] + " - " + tictactoe[6], ConsoleColor.Red);
                        TicTacToeWinner(msg, 4, move);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        if (tictactoe[i] == "empty")
                        {
                            final_message += "";
                        }
                        else if (tictactoe[i] == "x")
                        {
                            final_message += "X";
                        }
                        else if (tictactoe[i] == "o")
                        {
                            final_message += "O";
                        }

                        if (i == 2 || i == 5)
                        {
                            final_message += "\n";
                        }

                    }

                    if (winner == "")
                    {
                        await Message.ModifyAsync(msg => msg.Content = final_message);
                    }
                    else if (winner == "bot")
                    {
                        await Message.ModifyAsync(msg => msg.Content = final_message + "\n" + "Bot wins!");
                        for (int x = 0; x < 9; x++)
                        {
                            tictactoe[x] = "empty";
                            winner = "";
                        }
                    }
                    else if (winner == "user")
                    {
                        await Message.ModifyAsync(msg => msg.Content = final_message + "\n" + "User wins!");
                        for (int x = 0; x < 9; x++)
                        {
                            tictactoe[x] = "empty";
                            winner = "";
                        }
                    }
                }

                // check for winning: if win send ONLY the last move of the user and end the game ------ if not: send also the next move of the bot

            }
        }

        private static void TicTacToeWinner(SocketMessage msg, int o, int move)
        {
            var random = new Random();
            if (tictactoe[o] == "x")
            {
                winner = "user";
                tictactoe[move] = "empty";
            }
            else if (tictactoe[o] == "o")
            {
                bool emptySpace = false;
                while (emptySpace == false)
                {
                    move = random.Next(0, 9);
                    if (tictactoe[move] == "empty")
                    {
                        tictactoe[move] = "o";
                        emptySpace = true;
                    }
                }
                winner = "bot";
            }
            else
            {

            }
        }
    }
}
