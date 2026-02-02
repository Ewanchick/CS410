using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    public class Command(CommandWord commandWord, string? secondWord)
    {
        private readonly CommandWord commandWord = commandWord;
        private readonly string? secondWord = secondWord;

        /**
         * Return the command word (the first word) of this command. If the
         * command was not understood, the result is null.
         * @return The command word.
         */
        public CommandWord GetCommandWord()
        {
            return commandWord;
        }

        /**
         * @return The second word of this command. Returns null if there was no
         * second word.
         */
        public string? GetSecondWord()
        {
            return secondWord;
        }

        /**
         * @return true if this command was not understood.
         */
        public bool IsUnknown()
        {
            return (commandWord == CommandWord.UNKNOWN);
        }

        /**
         * @return true if the command has a second word.
         */
        public bool HasSecondWord()
        {
            return (secondWord != null);
        }
    }
}
