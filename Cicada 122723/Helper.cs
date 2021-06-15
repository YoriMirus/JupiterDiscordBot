using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Jupiter
{
    class Helper
    {
        public static string getRdsConnection()
        {
            var appConfig = ConfigurationManager.AppSettings;
            string dbName = appConfig["discord"];

            string username = appConfig["theredlord"];
            string password = appConfig["y325485696"];
            string port = appConfig["5432"];
            string hostName = appConfig["discord.cgkmvggpiti5.us-east-2.rds.amazonaws.com"];

            return "Data Source=" + hostName + ";Initial Catalog=" + dbName + ";User ID=" + username + ";Password=" + password + ";";
        }

        public static string connectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }


        public static void ColorWrite(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ColorWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static Random r;

        /// <summary>
        /// Returns a random number
        /// </summary>
        /// <param name="minValue">Lowest number you can get</param>
        /// <param name="maxValue">Highest number you can get + 1</param>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            if (r == null)
                r = new Random();

            return r.Next(minValue, maxValue);
        }

        public static Random GetRandom()
        {
            if (r == null)
                r = new Random();

            return r;
        }
    }
}
