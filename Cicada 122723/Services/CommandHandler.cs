using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(DiscordSocketClient client,
            CommandService commandService,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _commandService = commandService;
            _serviceProvider = serviceProvider;

            _client.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage messageParam)
        {
            if (messageParam.Author.Username == _client.CurrentUser.Username
                && messageParam.Author.Discriminator == _client.CurrentUser.Discriminator)
            {
                return;
            }

            var message = messageParam as SocketUserMessage;
            if (message == null)
            {
                return;
            }

            var channel = message.Channel as SocketGuildChannel;
            var prefix = String.Empty;

            if (channel != null) // Not a DM
            {
                prefix = "$";
            }

            var context = new SocketCommandContext(_client, message);
            int position = 0;

            if (message.HasStringPrefix(prefix, ref position)
                || message.HasMentionPrefix(_client.CurrentUser, ref position))
            {
                if (message.Author.IsBot)
                {
                    return;
                }

                await _commandService.ExecuteAsync(context, position, _serviceProvider);
            }
        }
    }
}
