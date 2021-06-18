using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;
using System.Drawing.Drawing2D;
using Microsoft.Extensions.DependencyInjection;
using Jupiter.Services;

namespace Jupiter
{
    public class Program
    {
        public static string[] insults = { "dumb", "idiot", "stupid", "dumb fuck", "fuck off", "cunt", "pussy", "baka" };

        static async Task Main(string[] args)
        {
            var serviceProvider = Bootstrap.Initialize(args);
            var discordService = serviceProvider.GetRequiredService<DiscordBot>();

            if (discordService != null)
            {
                try
                {
                    await Task.Run(discordService.Start);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unhandeled Exception Caught!");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);

                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }

                    ExitCleanly();
                }
            }
            else
            {
                Console.WriteLine("Failed to start Jupitor!");
                ExitCleanly();
            }

            while (true)
            {
                // If the "Q" key is pressed quit the bot!
                Console.WriteLine("Press 'Q' to quit!");
                var key = Console.ReadKey(true).KeyChar;

                if (char.ToLowerInvariant(key) == 'q')
                {
                    break;
                }
            }
            ExitCleanly();
        }

        /// <summary>
        /// Attempt to kill the bot cleanly.
        /// </summary>
        /// <param name="exitCode">Exit code to pass to the OS</param>
        public static void ExitCleanly(int exitCode = 0)
        {
            Console.WriteLine("Jupitor Bot is quiting!");
            Environment.Exit(exitCode);
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
