using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using Jupiter.Helpers;

namespace Jupiter.Commands
{
    public class MusicCommands : ModuleBase<SocketCommandContext>
    {
        private static LavalinkQueue queue = new LavalinkQueue();
        private readonly IAudioService _audioService;
        private readonly DiscordSocketClient _client;

        public MusicCommands(IAudioService audioService,
            DiscordSocketClient client)
        {
            _audioService = audioService;
            _client = client;
        }

        [Command("scplay")]
        // TODO always returns null
        public async Task SoundCloudPlay([Remainder]string song)
        {
            Helper.ColorWriteLine($"this is a text", ConsoleColor.DarkCyan);
            if (Context.Channel.Name.Trim() != "music🎵")
            {
                await ReplyAsync("Please Use This Command Only In The Text Channel #music🎵");
            }
            else if ((Context.Message.Author as SocketGuildUser).VoiceChannel.Name != "Music")
            {
                await ReplyAsync("Please Stream Music Only To The `Music` Voice Channel");
            }
            else
            {
                string scQuery = song;
                if (string.IsNullOrEmpty((Context.Message.Author as SocketGuildUser).VoiceChannel?.Name))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user wasnt connected to a channel");
                    Console.ForegroundColor = ConsoleColor.White;
                    await ReplyAsync("You Are Not Connected To a Voice Channel");
                }
                else if (string.IsNullOrEmpty(scQuery) || string.IsNullOrWhiteSpace(scQuery))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user didnt specify song name");
                    Console.ForegroundColor = ConsoleColor.White;
                    await ReplyAsync("Please Specify What You Would Like To Listen To");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("searching for song by the name: '{0}'", scQuery);
                    Console.ForegroundColor = ConsoleColor.White;

                    var channel_id = (Context.Message.Author as SocketGuildUser).VoiceChannel.Id;

                    LavalinkPlayer player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640) ?? await _audioService.JoinAsync(780666678811033640, channel_id);

                    var track = await _audioService.GetTrackAsync(scQuery, SearchMode.SoundCloud);

                    if(track == null)
                    {
                        await ReplyAsync("track was null...");
                        return;
                    }

                    Console.WriteLine(player + " : " + track.Duration + " : " + player.VoiceChannelId + " : " + track.Title);
                    double end_time = track.Duration.TotalMilliseconds + 60000;
                    await player.PlayAsync(track, null, TimeSpan.FromMilliseconds(end_time), false);
                    await Context.Channel.SendMessageAsync("Streaming " + "`" + track.Title + "`" + " To: " + "`" + (Context.Message.Author as SocketGuildUser).VoiceChannel.Name + "`");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Streaming {0} to the channel {1}. duration: {2}", track.Title, (Context.Message.Author as SocketGuildUser).VoiceChannel.Name, track.Duration);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(track.Author);
                }
            }
        }

        [Command("play")]
        public async Task Play([Remainder]string song)
        {
            LavalinkPlayer player;
            LavalinkTrack track;

            if (Context.Channel.Name.Trim() != "music🎵" && Context.Channel.Name.Trim() != "bot-spam🤖")
            {
                await Context.Channel.SendMessageAsync("Please Use This Command Only In The <#781069176668160041> Text Chat");
            }
            else if ((Context.Message.Author as SocketGuildUser).VoiceChannel?.Name != "Music" && (Context.Message.Author as SocketGuildUser).VoiceChannel?.Name != "Music 2")
            {
                await Context.Channel.SendMessageAsync("Please Stream Music Only To The `Music` or `Music 2` Voice Channel");
            }
            else
            {
                string yotubeQuery = song;
                if (string.IsNullOrEmpty((Context.Message.Author as SocketGuildUser).VoiceChannel?.Name))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user wasnt connected to a channel");
                    Console.ForegroundColor = ConsoleColor.White;
                    await ReplyAsync("You Are Not Connected To a Voice Channel");
                }
                else if (string.IsNullOrEmpty(yotubeQuery) || string.IsNullOrWhiteSpace(yotubeQuery))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("user didnt specify song name");
                    Console.ForegroundColor = ConsoleColor.White;
                    await ReplyAsync("Please Specify What You Would Like To Listen To");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("searching for song by the name: '{0}'", yotubeQuery);
                    Console.ForegroundColor = ConsoleColor.White;
                    var channel_id = (Context.Message.Author as SocketGuildUser).VoiceChannel.Id;
                    player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
                    if (string.IsNullOrEmpty(player?.VoiceChannelId.ToString()))
                    {
                        await _audioService.JoinAsync(780666678811033640, channel_id);
                        player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                    track = await _audioService.GetTrackAsync(yotubeQuery, SearchMode.YouTube);
                    if (player.CurrentTrack == null)
                    {
                        queue.Add(track);
                    }
                    Console.WriteLine(player + " : " + track.Duration + " : " + player.VoiceChannelId + " : " + track.Title);
                    await _client.SetGameAsync(yotubeQuery.ToString(), null, Discord.ActivityType.Playing);
                    await ReplyAsync("Streaming " + "`" + track.Title + "`" + "  " + "🎵");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Streaming {0} to the channel {1}. duration: {2}", track.Title, (Context.Message.Author as SocketGuildUser).VoiceChannel.Name, track.Duration);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(track.Author);
                    await player.PlayAsync(track);
                }
            }
        }

        [Command("stop")]
        public async Task Stop()
        {
            var player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            var channel_id = (Context.Message.Author as SocketGuildUser).VoiceChannel.Id;
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

        [Command("resume")]
        public async Task Resume()
        {
            LavalinkPlayer player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            if (string.IsNullOrEmpty(player.State.ToString()) || string.IsNullOrWhiteSpace(player.State.ToString()))
            {

            }
            else
            {
                await player.ResumeAsync();
            }
        }

        [Command("pause")]
        public async Task Pause()
        {
            LavalinkPlayer player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640);
            if (string.IsNullOrEmpty(player.State.ToString()) || string.IsNullOrWhiteSpace(player.State.ToString()))
            {

            }
            else
            {
                await player.PauseAsync();
            }
        }

        [Command("quit")]
        public async Task Quit(SocketMessage msg)
        {
            if (string.IsNullOrEmpty((msg.Author as SocketGuildUser).VoiceChannel?.Name))
            {
                await msg.Channel.SendMessageAsync("You Are Not Connected To a Voice Channel");
            }
            else
            {
                var channel_id = (msg.Author as SocketGuildUser).VoiceChannel.Id;
                LavalinkPlayer player = _audioService.GetPlayer<LavalinkPlayer>(780666678811033640) ?? await _audioService.JoinAsync(780666678811033640, channel_id);
                await player.StopAsync(disconnect: true);
            }
        }
    }
}
