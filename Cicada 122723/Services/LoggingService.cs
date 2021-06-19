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
using System;
using System.Threading.Tasks;

namespace Jupiter
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;

        public LoggingService(DiscordSocketClient client,
            CommandService commandService)
        {
            _client = client;
            _commandService = commandService;

            client.Log += Log;
            commandService.Log += Log;
        }

        private Task Log(LogMessage message)
        {
            if (message.Exception is CommandException commandException)
            {
                Console.WriteLine($"[Command/{message.Severity}] {commandException.Command.Aliases[0]} failed to execute in {commandException.Context.Guild?.Name ?? "DM"}:{commandException.Context.Channel.Name}.");
                Console.WriteLine("Exception: " + commandException);
            }
            else
            {
                var logMessage = $"[{message.Source}/{message.Severity}]: {message.Message}";

                switch (message.Severity)
                {
                    case LogSeverity.Critical:
                        Console.WriteLine($"Critial: {logMessage}");
                        break;
                    case LogSeverity.Error:
                        Console.WriteLine($"Error: {logMessage}");
                        break;
                    case LogSeverity.Warning:
                        Console.WriteLine($"Warning: {logMessage}");
                        break;
                    case LogSeverity.Info:
                        Console.WriteLine($"Info: {logMessage}");
                        break;
                    case LogSeverity.Verbose:
                        Console.WriteLine($"Verbose: {logMessage}");
                        break;
                    case LogSeverity.Debug:
                        Console.WriteLine($"Debug: {logMessage}");
                        break;
                    default:
                        break;
                }
            }

            if (message.Exception != null)
            {
                System.Console.WriteLine($"Exception: {message.Exception}");
            }

            return Task.CompletedTask;
        }
    }
}