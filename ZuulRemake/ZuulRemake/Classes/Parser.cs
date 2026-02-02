using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    public class Parser
    {
        private readonly CommandWords commands;  // holds all valid command words
        
        /**
         * Create a parser to read from the terminal window.
         */
        public Parser()
        {
            commands = new CommandWords();
        }

        /**
         * @return The next command from the user.
         */
        public Command GetCommand()
        {
            string? inputLine;   // will hold the full input line
            string? word1 = null;
            string? word2 = null;

            Console.Write("> ");     // print prompt

            inputLine = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(inputLine))
            {
                string[] words = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                    word1 = words[0];
                if (words.Length > 1)

                    word2 = words[1];
            }
            return new Command(commands.GetCommandWord(word1), word2);
        }
            
    /**
     * print out a list of valid command words.
     */
    public void ShowCommands()
        {
            commands.ShowAll();
        }
    }
}
