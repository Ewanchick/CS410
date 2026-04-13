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
using ZuulRemake.Classes.Helper_Classes;

namespace ZuulRemake.Classes
{
    public class Game
    {
        private readonly Parser parser;
        public Player Player => player!;
        private Player? player;        
        private Room? entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;
        private CommandHandler? ch;
        private NavigationManager nav = new NavigationManager();

        public static void Main(string[] args)
        {
            var game = new Game();
            game.Play();            
        }

        public Game()
        {
            parser = new Parser();
        }
        //Added for testing purposes, allows for dependency injection of a player object with custom stats
        public Game(Player player)
        {
            parser = new Parser();
            this.player = player;
            InitializeGame(player);
        }

        private void InitializeGame(Player player)
        {                       
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
            player = MainMenu();
            InitializeGame(player);
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

        private Player MainMenu()
        {
            Console.WriteLine("Welcome to the World of Zuul!\n");
            Console.WriteLine("Please choose an option: ");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Continue");
            Console.WriteLine("3. Quit\n");

            string x = Console.ReadLine()!;
            var p = new Player("Player");

            switch (x)
            {
                case "1":
                    string? name = null;
                    while (string.IsNullOrWhiteSpace(name))
                    {
                        Console.Write("\nPlease enter your name: ");
                        name = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(name))
                            Console.WriteLine("Invalid input. Please enter your name:");
                    }
                    p.Name = name.Trim();
                    return p;

                case "2":
                    var data = SaveManager.Load();

                    if (data == null)
                    {
                        Console.WriteLine("\n(!) No save file found.\n");
                        return MainMenu();
                    }

                    p.LoadSaveData(data.Name!, data.HP, data.Level, data.MaxWeight);

                    Console.WriteLine("\nSave loaded.");
                    return p;

                case "3":
                    Environment.Exit(0);
                    return p;

                default:
                    Console.WriteLine("Invalid option.\n");
                    return MainMenu();
            }
        }

        private void PrintWelcome()
        {           
            Console.WriteLine($"\nGreetings, {player.Name}!\n");
            Thread.Sleep(1000);

            Console.WriteLine("You have awoken in a very dark castle with no memory of how you got here.");
            Thread.Sleep(2000);

            Console.Write("\nUpon attempting to leave, you find that the front door is locked");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Thread.Sleep(2000);

            Console.Write("\nYou need to find its key in order to escape");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Thread.Sleep(500);
            Console.Write(" but beware!\n");
            Thread.Sleep(1500);
            Console.WriteLine("Danger lurks around every corner.\n");
            Thread.Sleep(2000);
            Console.WriteLine("(Type 'help' at any time to display available commands.) \n");
            Thread.Sleep(2000);

            try
            {
                Console.WriteLine(player.GetCurrentRoom().GetLongDescription() + "\n");
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