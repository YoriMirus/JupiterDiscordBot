﻿using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Jupiter.Commands;
using Lavalink4NET;
using Jupiter.Helpers;

namespace Jupiter.Services
{
    public class DiscordBot
    {
        public static readonly ulong GeneralChannelID = 780678378457923595;

        private static Process LavalinkServerProcess;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IAudioService _audioService;

        public DiscordBot(IServiceProvider serviceProvider,
            DiscordSocketClient client,
            CommandService commandService,
            IAudioService audioService)
        {
            _serviceProvider = serviceProvider;
            _client = client;
            _commandService = commandService;
            _audioService = audioService;

            _client.UserLeft += UserLeft;
            _client.UserJoined += UserJoined;
            _client.Connected += Connected;
            _client.Disconnected += Disconnected;
            _client.Ready += Ready;
            _client.MessageReceived += MessageReceived;
            _client.ReactionAdded += ReactionAdded;
        }

        public async Task Start()
        {
            string token;

            if (File.Exists(Environment.CurrentDirectory + "\\BotInfo.txt"))
            {
                string info = File.ReadAllText(Environment.CurrentDirectory + "\\BotInfo.txt");

                token = info.Trim();
            }
            else
            {
                Console.WriteLine("BotInfo.txt file not detected. Enter bot token: ");
                token = Console.ReadLine();
            }

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
            }
            catch (Discord.Net.HttpException ex)
            {
                if (ex.Reason == "401: Unauthorized")
                {
                    Console.WriteLine("\nToken is incorrect.");
                    Start();
                }
                else
                {
                    Console.WriteLine("An unhandeled HttpException has occured!");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Jupitor is quiting!");

                    Environment.Exit(0);
                }
            }

            await _client.StartAsync();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            //Check, if the reaction wasn't on a help menu
            if (!reaction.User.Value.IsBot)
            {
                await Help.OnReaction(reaction);
            }
            //Check if it wasn't a rock paper scissors game
            //Warning, access restriction violation happens here! Will refactor later.
            if (RockPaperScissors.GameOver == true && !reaction.User.Value.IsBot)
            {
                if (reaction.MessageId.ToString().Trim() == RockPaperScissors.RpcMessageId.Trim().ToString())
                {
                    await RockPaperScissors.OnUserInput(reaction, originChannel);
                }
            }
        }

        private Task MessageReceived(SocketMessage msg)
        {
            MessagePrinting(msg);

            //else if (msg.Content.Contains("<@!782261217334001674>") || msg.Content.Contains("<@782261217334001674>")) { await TagCommand.TagBot(msg); return; }  //bot tag
            return Task.CompletedTask;
        }

        private void MessagePrinting(SocketMessage msg)
        {
            Helper.ColorWriteLine(msg.Author.Username + " ", ConsoleColor.Cyan);
            Helper.ColorWriteLine("To: ", ConsoleColor.Yellow);
            string channel_name = msg.Channel.Name.Replace("?", "");
            Helper.ColorWrite(channel_name, ConsoleColor.Cyan);
            Helper.ColorWrite(msg.Content, ConsoleColor.White);
        }

        private async Task Ready()
        {
            if (LavalinkServerProcess == null)
                await Connected();

            //await MusicCommands.InitializeLavalink(_client);
            await _audioService.InitializeAsync();
        }
        private async Task Connected()
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
        
            await _client.SetGameAsync("Genshin Impact", null, ActivityType.Playing);
        } 
        private async Task Disconnected(Exception arg)
        {
            if (LavalinkServerProcess != null)
            {
                await Task.Run(() => LavalinkServerProcess.Kill());
            }
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            Console.WriteLine("NEW user joined");
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync("Everyone Say Hello To " + user.Mention + ", We Hope You Will Enjoy Our Server!");
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync("Hello " + user.Username + ", Nice to Meet You! My name is Jupiter, see full list of what I am capable of by typing `$help` or `$menu`");
            await user.SendMessageAsync(@$"Welcome to 'CodeVerse'. Remember to read the rules before you perform any actions in the server!");
        }
        private async Task UserLeft(SocketGuildUser user)
        {
            await user.Guild.GetTextChannel(GeneralChannelID).SendMessageAsync(user.Username + "#" + user.Discriminator + " Has left the server...");
        }
    }
}
