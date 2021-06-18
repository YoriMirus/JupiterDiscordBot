/*
MIT License

Copyright(c) 2021 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Jupiter.Repository;
using Jupiter.Services;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Microsoft.Extensions.DependencyInjection;

namespace Jupiter
{
    internal static class Bootstrap
    {
        /// <summary>
        /// Setup Dependency Injection
        /// </summary>
        /// <param name="args">Command Line arguments</param>
        /// <returns>The DI Container</returns>
        internal static ServiceProvider Initialize(string[] args)
        {
            CommandService commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
            });

            DiscordSocketClient socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 500,
                AlwaysDownloadUsers = true,
                ExclusiveBulkDelete = true,
            });

            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IAudioService, LavalinkNode>()
                .AddSingleton(socketClient)
                .AddSingleton<IAudioService, LavalinkNode>()
                .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                .AddSingleton(new LavalinkNodeOptions
                {
                    RestUri = "http://localhost:2333/",
                    WebSocketUri = "ws://localhost:2333/",
                    Password = "youshallnotpass"
                })
                .AddSingleton<CommandHandler>()
                .AddSingleton(commandService)
                .AddSingleton<LoggingService>()
                .AddSingleton<DiscordBot>()
                .AddSingleton<UserTimeRepository>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Create an instance of these so the event handlers get registered
            serviceProvider.GetRequiredService<CommandHandler>();
            serviceProvider.GetRequiredService<LoggingService>();

            return serviceProvider;
        }
    }
}
