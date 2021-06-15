using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    static class JupiterSayCommand
    {
        public static async Task React(SocketMessage source)
        {
            string say = source.Content.Replace("$jupiter say", "");
            await source.Channel.SendMessageAsync(say);
        }
    }
}
