using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Jupiter
{
    class Helper
    {
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
