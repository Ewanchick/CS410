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
using System.Text.Json;

namespace ZuulRemake.Classes
{
    /**
    * --------------------  NOTES:  --------------------     
    * 
    *
    * Exit room should not print its details since the game is over
    *
    * combat (maybe?) should start automatically if a monster is present in the room
    * alternate room description/entry message if there is a monster for auto combat
    * maybe make descriptions nullable so they don't print at all?
    * we should be able to customize what prints & when for a given room
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

                if (!player.IsAlive)
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
            Console.WriteLine("Thank you for playing. Goodbye!");
        }

        /**
         * Print out the opening message for the player.
         */
        private void PrintWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul!\n");
            Thread.Sleep(500);
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
            Thread.Sleep(1000);
            Console.WriteLine("You have awoken in a very dark castle with no memory of how you got here.");
            Thread.Sleep(2000);
            Console.WriteLine("Upon attempting to leave, you find that the front door is locked.");
            Thread.Sleep(2000);
            Console.WriteLine("You need to find its key in order to escape... but beware!");
            Thread.Sleep(1000);
            Console.WriteLine("Danger lurks around every corner.");
            Thread.Sleep(2000);
            Console.WriteLine("(Type 'help' at any time to display available commands.) \n");
            Thread.Sleep(2000);

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
            Console.WriteLine("YOU DIED, please try again!");
        }

        /**
         * Print a victory message when the player leaves the castle.
         */
        private void PrintWon()
        {
            Console.WriteLine("\nCongratulations, you've won the game!");
            Thread.Sleep(500);
            Console.WriteLine("             ___________");
            Thread.Sleep(50);
            Console.WriteLine("            '._==_==_=_.'");
            Thread.Sleep(50);
            Console.WriteLine("            .-\\:      /-.");
            Thread.Sleep(50);
            Console.WriteLine("           | (|:.     |) |");
            Thread.Sleep(50);
            Console.WriteLine("            '-|:.     |-'");
            Thread.Sleep(50);
            Console.WriteLine("              \\::.    /");
            Thread.Sleep(50);
            Console.WriteLine("               '::. .'");
            Thread.Sleep(50);
            Console.WriteLine("                 ) (");
            Thread.Sleep(50);
            Console.WriteLine("               _.' '._");
            Thread.Sleep(50);
            Console.WriteLine("              `'''''''`\n");
        }
    }
}