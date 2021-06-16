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
        internal DiscordSocketClient cicada_client;
        public static string ProgramLocation = Environment.CurrentDirectory;
        public static string[] insults = { "dumb", "idiot", "stupid", "dumb fuck", "fuck off", "cunt", "pussy", "baka" };

        public static string timezone;
        public static string username;
        public static string users;

        public static string old_tz;

        static async Task Main(string[] args)
        {
            // string sqlConection = "Data Source = (LocalDB)/MSSQLLocalDB; AttachDbFilename = C:/Users/Yakov/source/repos/Cicada 122723/Cicada 122723/Database1.mdf; Integrated Security = True";
            string[] content = { "hello\nworld" };
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            File.WriteAllLines(path + @"\content.txt", content);

            string token;

            if (File.Exists(Environment.CurrentDirectory + "\\BotInfo.txt"))
            {
                string info = File.ReadAllText(Environment.CurrentDirectory + "\\BotInfo.txt");

                token = info.Trim();
            }
            else
            {
                Console.WriteLine("BotInfo.txt file not detected. Enter bot token: ");
                token = Console.ReadLine();
            }

            await DiscordBot.InitializeAsync(token);
            await Task.Delay(-1);
        }

        public static Emoji cat_thumbs_up = new Emoji("<:cat_thumbs_up:797442808470306826>");

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
