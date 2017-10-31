using System;
using System.Collections.Generic;
using System.Linq;

namespace TestClient.Util
{
    public static class ConsoleHelper
    {
        private const int enter = 13;
        private const int backspace = 8;
        private const int ctrlBackspace = 127;
        private const int escape = 27;
        private const int tab = 9;
        private const int linefeed = 10;
        //private const int space = 32;

        private static readonly int[] filtered = { 0, escape, tab, linefeed /*, space, if you care */ }; // const

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// Source: https://stackoverflow.com/questions/3404421/password-masking-console-application
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            var pass = new Stack<char>();
            char chr;

            while ((chr = Console.ReadKey(true).KeyChar) != enter)
            {
                if (chr == backspace)
                {
                    if (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                    continue;
                }

                if (chr == ctrlBackspace)
                {
                    while (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                    continue;
                }

                if (filtered.Count(x => chr == x) > 0)
                {
                    continue;
                }

                pass.Push(chr);
                Console.Write(mask);
            }

            Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword()
        {
            return ReadPassword('*');
        }
    }
}
