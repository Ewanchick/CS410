using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class CommandWord
    {
        // A value for each command word along with its
        // corresponding user interface string.
        GO("go"), QUIT("quit"), HELP("help"), UNKNOWN("?"), LOOK("look"), 
    TAKE("take"),INVENTORY("inventory"), BACK("back"), DROP("drop"),
    //EAT("eat"), 
    //CHARGE("charge"), //FIRE("fire"), 
    USE("use"), 
    ATTACK("attack");



        // The command string.
        private String commandString;

        /**
         * Initialise with the corresponding command string.
         * @param commandString The command string.
         */
        CommandWord(String commandString)
        {
            this.commandString = commandString;
        }

        /**
         * @return The command word as a string.
         */
        public String toString()
        {
            return commandString;
        }
      }
    }
