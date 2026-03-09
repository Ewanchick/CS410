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
    public class Game
    {
        private readonly Parser parser;
        private readonly Player player;
        private Room entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;


        private CommandHandler ch;
        private NavigationManager nH = new NavigationManager();
        
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
            player = new Player("Player", 100, 10);
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
         * Initiates a battle between player and monster, continously prompting for input and 
         * returning with attacks until the monster or player is dead.
         */
        public void Battle(Monster m, Player p)
        {
            Console.WriteLine("The " + m.Name + " is hostile! Attack or be attacked!");

            while (m.IsAlive)
            {
                Console.WriteLine("What will you do? ");
                string? action = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(action) || !action.Contains("attack", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("You have chosen not to attack. The " + m.Name + " attacks instead!");
                }
                else
                {
                    m.TakeDamage(p.Level);
                    Console.WriteLine("You have attacked the " + m.Name + "! \n" +
                                      "The " + m.Name + "attacks back!");
                }

                p.TakeDamage(m.Level);
                Console.WriteLine("You have been injured! Your HP is now " + p.HP + ".");
            }

            Console.WriteLine($"You defeated the {m.Name}!");

            if (m.Drop != null)
            {
                player.GetCurrentRoom().AddItem(m.Drop);
                Console.WriteLine($"{m.Name} dropped a {m.Drop.Name}!");
            }
            player.GetCurrentRoom().RemoveMonster(m);
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

        // We can probably get rid of these two methods and just do an if-then in the game loop

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


        // implementations of user commands:

        /**
         * Print out some help information.
         * Here we print some stupid, cryptic message and a list of the 
         * command words.
         */
        private void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander around the castle.");
            Console.WriteLine();
            Console.WriteLine("Your command words are:");
            parser.ShowCommands();
        }
    }
}