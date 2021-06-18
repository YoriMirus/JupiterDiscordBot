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

            //var audioService = new LavalinkNode(new LavalinkNodeOptions
            //{
            //    RestUri = "http://localhost:8080/",
            //    WebSocketUri = "ws://localhost:8080/",
            //    Password = "youshallnotpass"
            //}, new DiscordClientWrapper(socketClient));

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
                    //Password = "youshallnotpass"
                    Password = "notarealpassword"
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
