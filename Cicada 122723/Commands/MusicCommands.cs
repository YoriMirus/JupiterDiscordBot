using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Audio;

namespace Jupiter.Commands
{
    static class MusicCommands
    {
        static LavalinkNode audioService;
        static LavalinkQueue queue = new LavalinkQueue();

        public static async Task SoundCloudPlay(SocketMessage msg)
        {
            Helper.ColorWriteLine($"this is a text", ConsoleColor.DarkCyan);
            if (msg.Channel.Name.Trim() != "music🎵")
            {
                await msg.Channel.SendMessageAsync("Please Use This Command Only In The Text Channel #music🎵");
            }
            else if ((msg.Author as SocketGuildUser).VoiceChannel.Name != "Music")
            {
                await msg.Channel.SendMessageAsync("Please Stream Music Only To The `Music` Voice Channel");
            }
            else
            {
                string scQuery = msg.Content.Replace("$scplay", "").Trim();
                if (string.IsNullOrEmpty((msg.Author as SocketGuildUser).VoiceChannel?.Name))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user wasnt connected to a channel");
                    Console.ForegroundColor = ConsoleColor.White;
                    await msg.Channel.SendMessageAsync("You Are Not Connected To a Voice Channel");
                }
                else if (string.IsNullOrEmpty(scQuery) || string.IsNullOrWhiteSpace(scQuery))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user didnt specify song name");
                    Console.ForegroundColor = ConsoleColor.White;
                    await msg.Channel.SendMessageAsync("Please Specify What You Would Like To Listen To");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("searching for song by the name: '{0}'", scQuery);
                    Console.ForegroundColor = ConsoleColor.White;

                    var channel_id = (msg.Author as SocketGuildUser).VoiceChannel.Id;

                    LavalinkPlayer player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640) ?? await audioService.JoinAsync(780666678811033640, channel_id);

                    var track = await audioService.GetTrackAsync(scQuery, SearchMode.SoundCloud);
                    Console.WriteLine(player + " : " + track.Duration + " : " + player.VoiceChannelId + " : " + track.Title);
                    double end_time = track.Duration.TotalMilliseconds + 60000;
                    await player.PlayAsync(track, null, TimeSpan.FromMilliseconds(end_time), false);
                    await msg.Channel.SendMessageAsync("Streaming " + "`" + track.Title + "`" + " To: " + "`" + (msg.Author as SocketGuildUser).VoiceChannel.Name + "`");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Streaming {0} to the channel {1}. duration: {2}", track.Title, (msg.Author as SocketGuildUser).VoiceChannel.Name, track.Duration);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(track.Author);
                }
            }
        }

        public static async Task Play(SocketMessage msg, DiscordSocketClient cicada_client)
        {
            LavalinkPlayer player;
            LavalinkTrack track;

            if (msg.Channel.Name.Trim() != "music🎵" && msg.Channel.Name.Trim() != "bot-spam🤖")
            {
                await msg.Channel.SendMessageAsync("Please Use This Command Only In The <#781069176668160041> Text Chat");
            }
            else if ((msg.Author as SocketGuildUser).VoiceChannel?.Name != "Music" && (msg.Author as SocketGuildUser).VoiceChannel?.Name != "Music 2")
            {
                await msg.Channel.SendMessageAsync("Please Stream Music Only To The `Music` or `Music 2` Voice Channel");
            }
            else
            {
                string yotubeQuery = msg.Content.Replace("$play", "").Trim();
                if (string.IsNullOrEmpty((msg.Author as SocketGuildUser).VoiceChannel?.Name))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user wasnt connected to a channel");
                    Console.ForegroundColor = ConsoleColor.White;
                    await msg.Channel.SendMessageAsync("You Are Not Connected To a Voice Channel");
                }
                else if (string.IsNullOrEmpty(yotubeQuery) || string.IsNullOrWhiteSpace(yotubeQuery))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user didnt specify song name");
                    Console.ForegroundColor = ConsoleColor.White;
                    await msg.Channel.SendMessageAsync("Please Specify What You Would Like To Listen To");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("searching for song by the name: '{0}'", yotubeQuery);
                    Console.ForegroundColor = ConsoleColor.White;
                    var channel_id = (msg.Author as SocketGuildUser).VoiceChannel.Id;
                    player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
                    if (string.IsNullOrEmpty(player?.VoiceChannelId.ToString()))
                    {
                        await audioService.JoinAsync(780666678811033640, channel_id);
                        player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                    track = await audioService.GetTrackAsync(yotubeQuery, SearchMode.YouTube);
                    if (player.CurrentTrack == null)
                    {
                        queue.Add(track);
                    }
                    Console.WriteLine(player + " : " + track.Duration + " : " + player.VoiceChannelId + " : " + track.Title);
                    await cicada_client.SetGameAsync(yotubeQuery.ToString(), null, Discord.ActivityType.Playing);
                    await msg.Channel.SendMessageAsync("Streaming " + "`" + track.Title + "`" + "  " + "🎵");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Streaming {0} to the channel {1}. duration: {2}", track.Title, (msg.Author as SocketGuildUser).VoiceChannel.Name, track.Duration);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(track.Author);
                    await player.PlayAsync(track);
                }
            }
        }

        public static async Task Stop(SocketMessage msg)
        {
            var player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            var channel_id = (msg.Author as SocketGuildUser).VoiceChannel.Id;
            Helper.ColorWriteLine($"stop method happened", ConsoleColor.DarkRed);

            if (player.CurrentTrack == null)
            {
                Helper.ColorWriteLine($"null", ConsoleColor.DarkRed);
                await player.StopAsync(disconnect: false);
                //await ReplyAsync("Nothing playing!");
                return;
            }
            else
            {
                await player.StopAsync(disconnect: false);
                Helper.ColorWriteLine($"stopped", ConsoleColor.DarkRed);
            }

            //await player.StopAsync();
            //await player.ConnectAsync(channel_id);
            return;
        }

        public static async Task Resume(SocketMessage msg)
        {
            LavalinkPlayer player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            if (string.IsNullOrEmpty(player.State.ToString()) || string.IsNullOrWhiteSpace(player.State.ToString()))
            {

            }
            else
            {
                player.ResumeAsync();
            }
        }

        public static async Task Pause(SocketMessage msg)
        {
            LavalinkPlayer player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            if (string.IsNullOrEmpty(player.State.ToString()) || string.IsNullOrWhiteSpace(player.State.ToString()))
            {

            }
            else
            {
                player.PauseAsync();
            }
        }

        public static async Task Quit(SocketMessage msg)
        {
            if (string.IsNullOrEmpty((msg.Author as SocketGuildUser).VoiceChannel?.Name))
            {
                await msg.Channel.SendMessageAsync("You Are Not Connected To a Voice Channel");
            }
            else
            {
                var channel_id = (msg.Author as SocketGuildUser).VoiceChannel.Id;
                LavalinkPlayer player = audioService.GetPlayer<LavalinkPlayer>(780666678811033640) ?? await audioService.JoinAsync(780666678811033640, channel_id);
                await player.StopAsync(disconnect: true);
            }
        }

        public static async Task InitializeLavalink(DiscordSocketClient cicada_client)
        {
            audioService = new LavalinkNode(new LavalinkNodeOptions
            {
                RestUri = "http://localhost:2333/",
                WebSocketUri = "ws://localhost:2333/",
                Password = "youshallnotpass",
                DisconnectOnStop = false
            }, new DiscordClientWrapper(cicada_client));
            await audioService.InitializeAsync();
        }



    }
}
