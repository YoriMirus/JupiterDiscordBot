using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Jupiter.Services;
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
                .AddSingleton(socketClient)
                .AddSingleton<CommandHandler>()
                .AddSingleton(commandService)
                .AddSingleton<LoggingService>()
                .AddSingleton<DiscordBot>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Create an instance of these so the event handlers get registered
            serviceProvider.GetRequiredService<CommandHandler>();
            serviceProvider.GetRequiredService<LoggingService>();

            return serviceProvider;
        }
    }
}
