using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DSharpPlus;

using Jupiter.Commands;

namespace Jupiter
{
    public static class DiscordBot
    {
        public static DiscordSocketClient SocketClient;
        public static DiscordClient Client;

        private static Process LavalinkServerProcess;

        public static ulong BotID { get => Client.CurrentUser.Id; }

        public static readonly ulong GeneralChannelID = 780678378457923595;

        public static async Task InitializeAsync(string token)
        {
            //Why are we using two clients at once?

            //Get SocketClient configuration and initialize
            var config = new DiscordSocketConfig { MessageCacheSize = 100 };
            SocketClient = new DiscordSocketClient(config);

            //Initialize DiscordClient
            var Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = DSharpPlus.TokenType.Bot,
            });

            //Set events for Client
            Client.GuildMemberRemoved += async (e) =>
            {
                await Task.Delay(100);
                await Log(new LogMessage(LogSeverity.Info, null, e.Member.Nickname + " left"));
            };

            await Client.ConnectAsync();

            //Set events for SocketClient
            SocketClient.Log += Log;

            SocketClient.UserLeft += UserLeft;
            SocketClient.UserJoined += UserJoined;

            SocketClient.Connected += Connected;
            SocketClient.Disconnected += Disconnected;
            SocketClient.Ready += Ready;

            SocketClient.MessageReceived += MessageReceived;
            SocketClient.MessageDeleted += MessageDeleted;
            SocketClient.ReactionAdded += ReactionAdded;

            //Login SocketClient
            await SocketClient.LoginAsync(Discord.TokenType.Bot, token);
            await SocketClient.StartAsync();
        }

        private static async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            //Check, if the reaction wasn't on a help menu
            if (reaction.UserId != BotID)
            {
                await HelpCommand.OnReaction(reaction);
            }
            //Check if it wasn't a rock paper scissors game
            //Warning, access restriction violation happens here! Will refactor later.
            if (RockPaperScissors.GameOver == true && reaction.UserId != BotID)
            {
                if (reaction.MessageId.ToString().Trim() == RockPaperScissors.RpcMessageId.Trim().ToString())
                {
                    await RockPaperScissors.OnUserInput(reaction, originChannel);
                }
            }
        }
        private static async Task MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            return;
        }
        private static async Task MessageReceived(SocketMessage msg)
        {
            await MessagePrinting(msg);

            if (msg.Content.StartsWith("$jupiter say")) { await JupiterSayCommand.React(msg); return; }


            else if (msg.Content.Trim() == "$users") { await UsersCommand.React(msg); return; }


            else if (msg.Content.Contains("<@!782261217334001674>") || msg.Content.Contains("<@782261217334001674>")) { await TagCommand.TagBot(msg); return; }  //bot tag

            else if (msg.Content.StartsWith("$time")) { await TimeCommands.React(msg); return; }


            else if (msg.Content.Trim().StartsWith("$play")) { await MusicCommands.Play(msg, SocketClient); }


            else if (msg.Content.ToLower().Trim().StartsWith("$quit")) { await MusicCommands.Quit(msg); return; }


            else if (msg.Content.Trim().StartsWith("$scplay")) { await MusicCommands.SoundCloudPlay(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$stop")) { await MusicCommands.Stop(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$resume")) { await MusicCommands.Resume(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$pause")) { await MusicCommands.Pause(msg); return; }


            else if (msg.Content.StartsWith("$delete")) { await DeleteCommand.Delete(msg); return; }


            else if (msg.Content == "$registered_users") { await RegisteredUsersCommand.Display(msg); return; }


            else if (msg.Content.StartsWith("$set-tz")) { await SetTimeZoneCommand.SetTimeZone(msg); return; }


            else if (msg.Content.ToLower().Trim() == "$utc") { await TimeCommands.UtcTime(msg); return; }


            else if (msg.Content.Trim().ToLower().Contains("$warn")) { await WarnCommand.Warn(msg); return; }


            else if (msg.Content.Trim().ToLower() == "$help" || msg.Content.Trim().ToLower() == "$menu") { await HelpCommand.DisplayHelpMenu(msg); return; }


            else if (msg.Content.Trim().StartsWith("$activity")) { await ActivityCommand.Activity(msg, SocketClient); return; }

            else if (msg.Content.Trim().ToLower() == "$joke") { await JokeCommand.React(msg); return; }


            else if (msg.Content.Trim().ToLower() == "$meme") { await MemeCommand.React(msg); return; }


            else if (msg.Content.Trim().Contains("$edit")) { await EditCommand.React(msg); return; }


            else if (msg.Content.Trim().ToLower().Contains("$rps")) { await RockPaperScissors.Start(msg); return; }


            else if (msg.Content.StartsWith("$8ball")) { await Ball.React(msg); return; }


            else if (msg.Content.Trim() == "$rr") { await RussianRoulette.React(msg); return; }


            else if (msg.Content.StartsWith("$rand")) { await RandCommand.React(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$google")) { await GoogleCommand.GoogleResults(msg); return; }


            else if (msg.Content.StartsWith("$ttt")) { await TicTacToe.React(msg); return; }
        }

        static async Task MessagePrinting(SocketMessage msg)
        {
            Helper.ColorWriteLine(msg.Author.Username + " ", ConsoleColor.Cyan);
            Helper.ColorWriteLine("To: ", ConsoleColor.Yellow);
            string channel_name = msg.Channel.Name.Replace("?", "");
            Helper.ColorWrite(channel_name, ConsoleColor.Cyan);
            Helper.ColorWrite(msg.Content, ConsoleColor.White);
        }

        private static async Task Ready()
        {
            if (LavalinkServerProcess == null)
                await Connected();

            await MusicCommands.InitializeLavalink(SocketClient);
        }
        private static async Task Connected()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\Lavalinnk.jar"))
            {
                Console.WriteLine("File exists");
                LavalinkServerProcess = new Process();
                LavalinkServerProcess.StartInfo.UseShellExecute = false;
                LavalinkServerProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                LavalinkServerProcess.StartInfo.FileName = @"java";
                LavalinkServerProcess.StartInfo.Arguments = @"-jar Lavalinnk.jar";
                LavalinkServerProcess.StartInfo.CreateNoWindow = true;
                LavalinkServerProcess.Start();
            }
            else
            {
                Console.WriteLine("file doesnt exists");
            }
        
            await SocketClient.SetGameAsync("Genshin Impact", null, ActivityType.Playing);
        } 
        private static async Task Disconnected(Exception arg)
        {
            await Task.Run(() => LavalinkServerProcess.Kill());
        }

        private static async Task UserJoined(SocketGuildUser user)
        {
            Console.WriteLine("NEW user joined");
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync("Everyone Say Hello To " + user.Mention + ", We Hope You Will Enjoy Our Server!");
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync("Hello " + user.Username + ", Nice to Meet You! My name is Jupiter, see full list of what I am capable of by typing `$help` or `$menu`");
            await user.SendMessageAsync(@$"Welcome to 'CodeVerse'. Remember to read the rules before you perform any actions in the server!");
        }
        private static async Task UserLeft(SocketGuildUser user)
        {
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync(user.Username + "#" + user.Discriminator + " Has left the server...");
        }

        private static Task Log(LogMessage arg)
        {
            switch (arg.Severity)
            {
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(arg.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;

            return Task.CompletedTask;
        }
    }
}
