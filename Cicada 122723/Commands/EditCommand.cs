using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class EditCommand
    {
        public static async Task React(SocketMessage msg)
        {
            var message = await msg.Channel.SendMessageAsync("test");
            var messageToSend = msg.Content.Replace("$edit", "");
            await message.ModifyAsync(msg => msg.Content = messageToSend);
        }
    }
}
