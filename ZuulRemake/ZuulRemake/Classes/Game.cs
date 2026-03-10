using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    /**
    * --------------------  NOTES:  -------------------- 
    * (!) removed 0 reference methods Battle and PrintHelp
    * 
    * 
    * kitchen does not tell the player that it's too dark to see/does not 
    * hint to them that they should go find a lantern or why that would be worth doing
    *
    * after defeating the ghoul: the "items in this room" is repetitive:
    * Room now contains:
    * Items in this room: Potion    
    * remove that entirely since drop message is enough
    * 
    * fix grammar everywhere
    * shorten line lengths, make sure lines are broken up with \n
    *
    * item drop repetition after defeating dragon too (remove will fix both)
    *
    * Exit room should not print its details since the game is over
    *
    * add sleeps EVERYWHERE, lack of delay feels weird
    *
    * dragon is a lower level than the ghoul?
    * combat (maybe?) should start automatically if a monster is present in the room
    *
    *POSSIBLY:
    * alternate room description/entry message if there is a monster for auto combat
    * same for if the room is dark (!!!)
    * do not hard code lantern putting sword in room 
    * 
    * maybe make descriptions nullable so they don't print at all?
    * basically just REMOVE any places where those print automatically when they shouldnt
    * we should be able to customize what prints for a given room
    */
    public class Game
    {
        private readonly Parser parser;
        private readonly Player player;
        private Room entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;


        private CommandHandler ch;
        private NavigationManager nav = new NavigationManager();

        public static void Main(string[] args)
        {
            var game = new Game();
            game.Play();
        }

        /**
         * Create the game and initialise its internal map. :3
         * I like turtles a lot
         */
        public Game()
        {
            parser = new Parser();
            player = new Player("Player", 100, 50);
            Room startRoom = WorldBuilder.Build(
                out entryway,
                out dininghall,
                out ballroom,
                out kitchen,
                out bathroom,
                out dungeon,
                out bedroom,
                out exit);

            player.GoNewRoom(entryway);

            ch = new CommandHandler(player, parser, entryway, kitchen, exit);
        }

        /**
         *  Main play routine.  Loops until end of play.
         */
        public void Play()
        {
            PrintWelcome();

            while (true)
            {
                Command command = parser.GetCommand();

                bool quitRequested = ch.ProcessCommand(command);

                if (quitRequested)
                {
                    break;
                }

                if (player.HP == 0)
                {
                    GameOver();
                    break;
                }


                if (player.GetCurrentRoom() == exit)
                {
                    PrintWon();
                    break;
                }
            }
            Console.WriteLine("Thank you for playing.  Good bye.");
        }

        /**
         * Print out the opening message for the player.
         */
        private void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul!\n");

            string? name = null;
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Please enter your name: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    Console.WriteLine("Invalid input. Please enter your name:");
            }

            // FIX: apply the typed name to the player
            player.Name = name.Trim();

            Console.WriteLine($"Greetings, {player.Name}!");
            Console.WriteLine("You have awoken in a very dark castle with no memory of how you got here.");
            Console.WriteLine("Upon attempting to leave, you find that the front door is locked.");
            Console.WriteLine("You need to find its key in order to escape... but beware!");
            Console.WriteLine("Danger lurks around every corner.");
            Console.WriteLine("(Type 'help' at any time to display available commands.)");
            Console.WriteLine();

            try
            {
                Console.WriteLine(player.GetCurrentRoom().GetLongDescription());
            }
            catch (NoCurrentRoomException ex)
            {
                Console.WriteLine($"Error loading starting room: {ex.Message}");
            }
        }
        
        /**
         * Print a game over message.
         */
        private void GameOver()
        {
            Console.WriteLine("You have died, please try again!");
        }

        /**
         * Print a victory message when the player leaves the castle.
         */
        private void PrintWon()
        {
            Console.WriteLine("You won, you defeated the dragon and escaped the castle!");
        }
    }
}