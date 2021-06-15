using System;
using Discord;
using Discord.Commands;
using Discord.Audio;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DSharpPlus;
using Lavalink4NET.Rest;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.DiscordNet;
using Reddit;
using Reddit.Controllers;
//using Reddit.Models;
using Reddit.Things;
using System.Diagnostics;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Drawing.Drawing2D;
using GoogleApi;

using Jupiter.Commands;
using Jupiter.Repository;
using Jupiter.Repository.Models;

namespace Jupiter
{
    public class Program : ModuleBase<SocketCommandContext>
    {
        internal string token = "NzgyMjYxMjE3MzM0MDAxNjc0.X8Jnhw.bhUX8IddIWCIEPJGeFM1l3Ow2jY";
        public static EmbedBuilder emb;

        internal DiscordSocketClient cicada_client;
        public static string ProgramLocation = Environment.CurrentDirectory;
        public static string[] insults = { "dumb", "idiot", "stupid", "dumb fuck", "fuck off", "cunt", "pussy", "baka" };

        public static string timezone;
        public static string username;
        public static string users;

        public static string old_tz;

        static void Main(string[] args)
        {
            // string sqlConection = "Data Source = (LocalDB)/MSSQLLocalDB; AttachDbFilename = C:/Users/Yakov/source/repos/Cicada 122723/Cicada 122723/Database1.mdf; Integrated Security = True";
            string[] content = { "hello\nworld" };
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            File.WriteAllLines(path + @"\content.txt", content);

            emb = new EmbedBuilder();
            emb.WithColor(Discord.Color.DarkBlue);

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig { MessageCacheSize = 100 };
            cicada_client = new DiscordSocketClient(config);

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = DSharpPlus.TokenType.Bot,
            });


            discord.GuildMemberRemoved += async (e) =>
            {
                await Task.Delay(100);
                Console.WriteLine(e.Member.Nickname + " left");
            };

            await discord.ConnectAsync();

            //cicada_client += heartbeat;
            //cicada_client.Disconnected += disconnected;
            cicada_client.UserJoined += UserJoined;
            cicada_client.UserLeft += UserLeft;
            cicada_client.Log += Log;
            cicada_client.Connected += connected;
            cicada_client.MessageDeleted += MessageDeleted;
            cicada_client.MessageReceived += MessageReceived;
            cicada_client.ReactionAdded += ReactionRecieved;
            cicada_client.Ready += Started;
            await cicada_client.LoginAsync(Discord.TokenType.Bot, token);
            await cicada_client.StartAsync();
            await Task.Delay(-1);
            
        }

        public LavalinkPlayer player;
        public LavalinkTrack track;

        public async Task disconnected()
        {
            return;
        }

        public async Task heartbeat(SocketGuildUser user)
        {
            
        }
        public static ulong general_channel_id = 780678378457923595;
        public async Task UserJoined(SocketGuildUser user)
        {
            Console.WriteLine("NET user joined");
            await user.Guild.GetTextChannel(general_channel_id).SendMessageAsync("Everyone Say Hello To " + user.Mention + ", We Hope You Will Enjoy Our Server!");
            await user.Guild.GetTextChannel(general_channel_id).SendMessageAsync("Hello " + user.Username + ", Nice to Meet You! My name is Jupiter, see full list of what I am capable of by typing `$help` or `$menu`");
            await user.SendMessageAsync(@$"Welcome to 'CodeVerse'. Remember to read the rules before you perform any actions in the server!");
        }
        public async Task UserLeft(SocketGuildUser user)
        {
            //await user.GetOrCreateDMChannelAsync();                                 
            //await user.SendMessageAsync("We Are The Admins of The Server So Sorry You Had to Leave...");
            await user.Guild.GetTextChannel(780666678811033643).SendMessageAsync(user.Username + "#" + user.Discriminator + " Has left the server...");
            //user.Guild.GetTextChannel(general_channel_id).SendMessageAsync("I am sorry to dissapoint but " + user.Username + )
        }

        internal async Task connected()
        {
            if (File.Exists(lavalinkServerPath))
            {
                Console.WriteLine("File exists");
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                p.StartInfo.FileName = @"java";
                p.StartInfo.Arguments = @"-jar Lavalinnk.jar";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
            else
            {
                Console.WriteLine("file doesnt exists");
            }
            await cicada_client.SetGameAsync("Genshin Impact", null, Discord.ActivityType.Playing);

            await cicada_client.GetGuild(809500999832043520).GetTextChannel(809501000305475585).SendMessageAsync("!1!1!1!1!1");
        }

        private static IAudioService audioService;
        
        public static string lavalinkServerPath = "C:/Users/Yakov/source/repos/LavaLink/Lavalinnk.jar";

        internal async Task Started()
        {

            //var botSpamChannel = cicada_client.GetGuild(780666678811033640).GetChannel(781568016516251658);
            //await (botSpamChannel as ISocketMessageChannel).SendMessageAsync("Jupiter loaded and ready to go!");
            /*ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "Lavalinnk.jar";
            info.WorkingDirectory = "C:/Users/Yakov/source/repos/Cicada 122723/Cicada 122723/bin/Debug/net5.0/LavaLink";
            info.Arguments = "-jar";*/

            /*
            Process process = new Process();
            process.StartInfo.FileName = @"F:\ProjektyProgramování\Cicada 122723\Cicada 122723\Cicada 122723\bin\Debug\net5.0\Lavalinnk.jar";
            process.StartInfo.Arguments = "-jar";
            process.Start();*/
            await MusicCommands.InitializeLavalink(cicada_client);

            //Test database
            var repo = new UserTimeRepository();
            await repo.RemoveEntry(2);
        }

        internal Task Log(LogMessage _Msg)
        {
            Console.WriteLine(_Msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MessageDeleted(Cacheable<IMessage, ulong> dmsg, ISocketMessageChannel channel)
        {
           // await channel.SendMessageAsync("message was deleted: ");
            //Console.WriteLine(dmsg);
        }

        public AudioStream audio;
   
        public static Emoji cat_thumbs_up = new Emoji("<:cat_thumbs_up:797442808470306826>");

        async Task MessageReceived(SocketMessage msg)
        {
            await MessagePrinting(msg);

            if (msg.Content.StartsWith("$jupiter say")) { await JupiterSayCommand.React(msg); return; }


            else if (msg.Content.Trim() == "$users") { await UsersCommand.React(msg); return; }


            else if (msg.Content.Contains("<@!782261217334001674>") || msg.Content.Contains("<@782261217334001674>")) { await TagCommand.TagBot(msg); return; }  //bot tag

            //Doesn't work for now, don't use this
            else if (msg.Content.StartsWith("$time")) { await TimeCommands.React(msg); return; }


            else if (msg.Content.Trim().StartsWith("$play")) { await MusicCommands.Play(msg, cicada_client); }


            else if (msg.Content.ToLower().Trim().StartsWith("$quit")) { await MusicCommands.Quit(msg); return; }


            else if (msg.Content.Trim().StartsWith("$scplay")) { await MusicCommands.SoundCloudPlay(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$stop")) { await MusicCommands.Stop(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$resume")) { await MusicCommands.Resume(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$pause")) { await MusicCommands.Pause(msg); return; }


            else if (msg.Content.StartsWith("$delete")) { await DeleteCommand.Delete(msg); return; }


            else if (msg.Content == "$registered_users") { await RegisteredUsersCommand.Display(msg); return; }


            else if (msg.Content.StartsWith("$set-tz")) { await SetTimeZoneCommand.SetTimeZone(msg); return; }


            else if (msg.Content.ToLower().Trim() == "$utc") { await TimeCommands.UtcTime(msg); return; }


            else if (msg.Content.Trim().ToLower().Contains("$warn")) { await WarnCommand.Warn(msg); return; }


            else if (msg.Content.Trim().ToLower() == "$help" || msg.Content.Trim().ToLower() == "$menu") { await HelpCommand.DisplayHelpMenu(msg); return; }


            else if (msg.Content.Trim().StartsWith("$activity")) { await ActivityCommand.Activity(msg, cicada_client); return; }

            else if (msg.Content.Trim().ToLower() == "$joke") { await JokeCommand.React(msg); return; }


            else if (msg.Content.Trim().ToLower() == "$meme") { await MemeCommand.React(msg); return; }


            else if (msg.Content.Trim().Contains("$edit")) { await EditCommand.React(msg); return; }


            else if (msg.Content.Trim().ToLower().Contains("$rps")) { await RockPaperScissors.Start(msg); return; }


            else if (msg.Content.StartsWith("$8ball")) { await Ball.React(msg); return; }


            else if (msg.Content.Trim() == "$rr") { await RussianRoulette.React(msg); return; }


            else if (msg.Content.StartsWith("$rand")) { await RandCommand.React(msg); return; }


            else if (msg.Content.ToLower().Trim().StartsWith("$google")) { await GoogleCommand.GoogleResults(msg); return; }


            else if (msg.Content.StartsWith("$ttt")) { await TicTacToe.React(msg); return; }

        }

        async Task MessagePrinting(SocketMessage msg)
        {
            Helper.ColorWriteLine(msg.Author.Username + " ", ConsoleColor.Cyan);
            Helper.ColorWriteLine("To: ", ConsoleColor.Yellow);
            string channel_name = msg.Channel.Name.Replace("?", "");
            Helper.ColorWrite(channel_name, ConsoleColor.Cyan);
            Helper.ColorWrite(msg.Content, ConsoleColor.White);
        }

        internal void timerEnd()
        {

        }

        public void onTrackEnd(LavalinkPlayer player, LavalinkTrack track, EventArgs e, Object obj)
        {

        }  
    

    

        public static Discord.Rest.RestUserMessage helpMenuMessage;
        public static ulong helpmenuMessageId;

        public async Task ReactionRecieved(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            //Check, if the reaction wasn't on a help menu
            if (reaction.UserId != 782261217334001674)
            {
                await HelpCommand.OnReaction(reaction);
            }
            //Check if it wasn't a rock paper scissors game
            //Warning, access restriction violation happens here! Will refactor later.
            if (RockPaperScissors.GameOver == true && reaction.UserId != 782261217334001674) 
            {
                if (reaction.MessageId.ToString().Trim() == RockPaperScissors.RpcMessageId.Trim().ToString())
                {
                    await RockPaperScissors.OnUserInput(reaction, originChannel);
                }
            }
        }

        async Task<System.Drawing.Image> FetchImage(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            return System.Drawing.Image.FromStream(stream);
        }

        private static Bitmap CropToBanner(System.Drawing.Image image)
        {
            var originalWidth = image.Width;
            var originalHeight = image.Height;
            var destinationSize = new Size(1100, 450);

            var heightRatio = (float)originalHeight / destinationSize.Height;
            var widthRation = (float)originalWidth / destinationSize.Width;

            var ratio = Math.Min(heightRatio, widthRation);

            var heightScale = Convert.ToInt32(destinationSize.Height * ratio);
            var widthScale = Convert.ToInt32(destinationSize.Width * ratio);

            var startX = (originalWidth - widthScale) / 2;
            var startY = (originalHeight - heightScale) / 2;

            var sourceRectangle = new Rectangle(startX, startY, widthScale, heightScale);
            var bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using var g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);

            return bitmap;
        }

        private System.Drawing.Image ClipImageToCircle(System.Drawing.Image image)
        {
            System.Drawing.Image destination = new Bitmap(image.Width, image.Height, image.PixelFormat);
            var radius = image.Width / 2;
            var x = image.Width / 2;
            var y = image.Height / 2;

            using Graphics g = Graphics.FromImage(destination);
            var r = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush brush = new SolidBrush(System.Drawing.Color.Transparent))
            {
                g.FillRectangle(brush, 0, 0, destination.Width, destination.Height);
            }

            var path = new GraphicsPath();
            path.AddEllipse(r);
            g.SetClip(path);
            g.DrawImage(image, 0, 0);

            return destination;
        }

        private System.Drawing.Image CopyRegionIntoImage(System.Drawing.Image source, System.Drawing.Image destination)
        {
            using var grD = Graphics.FromImage(destination);
            var x = (destination.Width / 2) - 110;
            var y = (destination.Height / 2) - 155;

            grD.DrawImage(source, x, y, 220, 220);
            return destination;
        }
    }
}
