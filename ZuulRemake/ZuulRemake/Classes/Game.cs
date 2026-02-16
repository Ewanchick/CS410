using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    public class Game
    {
        private readonly Parser parser;
        private readonly Player player;
        private readonly CommandHandler commandHandler;
        private Room entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;

        public static void Main(string[] args)
        {
            var game = new Game();
            game.Play();
        }

        /**
         * Create the game and initialise its internal map. :3
         */
        public Game()
        {
            parser = new Parser();
            player = new Player("Player1");


            Room startRoom = WorldBuilder.Build(out entryway,
                out dininghall,
                out ballroom,
                out kitchen,
                out bathroom,
                out dungeon,
                out bedroom,
                out exit);

            player.EnterRoom(startRoom);

            commandHandler = new CommandHandler(player, parser, entryway, kitchen, exit);
        }



        /**
         * Create all the rooms and link their exits together as well as monsters and items in the rooms.
         */


        /**
         *  Main play routine.  Loops until end of play.
         */
        public void Play()
        {
            PrintWelcome();

            while (true)
            {
                Command command = parser.GetCommand();

                bool quitRequested = commandHandler.ProcessCommand(command);

                if (quitRequested)
                {
                    break;
                }

                if (player.gameOver())
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
            Console.WriteLine("Please enter your name: ");
            player.Name = Console.ReadLine() ?? player.Name;
            Console.WriteLine("Greetings, " + player.Name);
            Console.WriteLine("You wake up in a very dark castle. \n" +
                              "You dont know how you got here and the front door is locked. \n" +
                              "You need to find the key to get out of here.");
            Console.WriteLine("Type 'help' to display commands.");
            Console.WriteLine();
            Console.WriteLine(player.GetCurrentRoom());
        }

        // We can probably get rid of these two methods and just do an if-then + console.writeline in the game loop

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
        