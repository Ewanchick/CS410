using System;
using System.Collections.Generic;


namespace ZuulRemake.Classes
{
    public class CommandWords
    {
        // A mapping between a command word and the CommandWord
        // associated with it.
        private readonly Dictionary<string, CommandWord> commands =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "go", CommandWord.GO
                },
                { "help", CommandWord.HELP
                },
                { "look", CommandWord.LOOK
                },
                { "take", CommandWord.TAKE
                },
                { "inventory", CommandWord.INVENTORY
                },
                { "back", CommandWord.BACK
                },
                { "drop", CommandWord.DROP
                },
                { "use", CommandWord.USE
                },
                { "attack", CommandWord.ATTACK
                },
                { "quit", CommandWord.QUIT
                }
            };

        /**
         * Constructor - initialise the command words.
         */


        /**
         * Find the CommandWord associated with a command word.
         * @param commandWord The word to look up.
         * @return The CommandWord correspondng to commandWord, or UNKNOWN
         *         if it is not a valid command word.
         */
        public CommandWord GetCommandWord(string? word)
        {
            if (word == null) return CommandWord.UNKNOWN;
            return commands.TryGetValue(word, out var cmd)
                ? cmd : CommandWord.UNKNOWN;
        }

        /**
         * Check whether a given String is a valid command word. 
         * @return true if it is, false if it isn't.
         */
        public bool IsCommand(string aString)
        {
            return commands.ContainsKey(aString);
        }

        /**
         * Print all valid commands to System.out.
         */
        public void ShowAll()
        {
            foreach (string command in commands.Keys)
            {
                Console.Write(command + "  ");

            }
        }
    }
}
