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
    internal class Game
    {
        private readonly Parser parser;
        private readonly Player player;
        private Room entryway, dininghall, ballroom, kitchen, bathroom, dungeon, bedroom, exit;
        private Item sword, lantern, armour, key, potion;
        private Monster dragon, ghoul;
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
            CreateRooms();
        }

        // attack monster goes here -> deal damage and take damage methods
        // move these / reorganize methods

        public void AttackMonster()
        {
            // fix
            Monster monster = CurrentRoom.GetRoomMonsters([1]);
            int level = player.Level;
            player.DealAttack(monster); ;
            monster.TakeDamage(player.Level);
        }



        /**
         * Create all the rooms and link their exits together as well as monsters and items in the rooms.
         */
        private  Room CreateRooms()
        {

            // create the rooms
            entryway = new Room("in the entryway of the castle, the exit to the south is locked");
            dininghall = new Room("in a large dining hall, you see a lantern ot the floor and a ball room to your right");
            ballroom = new Room("in the ball room, you find a helmet and chest piece in the middle of the floor");
            kitchen = new Room("in the kitchen, it is too dark to see anything");
            bathroom = new Room("in the bathoom you see a potion, but there is a nasty ghoul guarding it.");
            dungeon = new Room("in the dungeon, there is a large dragon guarding the key to the exit");
            bedroom = new Room("in a very large bedroom, nothing interesting in here.");
            exit = new Room("You made it, you escaped the castle and are now free!");

            // initialise room exits
            entryway.SetExit("north", dininghall);
            entryway.SetExit("east", kitchen);
            entryway.SetExit("west", bedroom);
            entryway.SetExit("down", dungeon);

            dininghall.SetExit("east", ballroom);
            dininghall.SetExit("south", entryway);
            ballroom.SetExit("west", dininghall);
            kitchen.SetExit("west", entryway);
            dungeon.SetExit("up", entryway);
            bedroom.SetExit("east", entryway);
            bedroom.SetExit("south", bathroom);
            bathroom.SetExit("north", bedroom);

            //create the items
            sword = new Item("sword", "heavy sword, might be used to kill the dragon", 1);
            lantern = new Item("lantern", "used to light the dark rooms of the castle", 1);
            armour = new Item("armour", "protect yourself from the mighty dragon", 1);
            potion = new Item("Potion", "use this to increase your health!", 1);
            key = new Item("key", "used to unlock the way out", 0);

            //initialize items
            dininghall.SetItem("lantern", lantern);
            ballroom.SetItem("armour", armour);

            player.EnterRoom(entryway);  // start game in the entryway of castle


            //create the monsters
            dragon = new Monster("dragon", 100);
            ghoul = new Monster("ghoul", 50);

            dungeon.SetMonster("dragon", dragon);
            bathroom.SetMonster("ghoul", ghoul);

            return entryway;
        }

        /**
         *  Main play routine.  Loops until end of play.
         */
        public void Play()
        {
            PrintWelcome();

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.

            bool finished = false;
            while (!finished)
            {
                Command command = parser.GetCommand();
                finished = ProcessCommand(command);
                if (player.gameOver())
                {
                    PrintGameOver();
                    finished = true;
                }
                if (player.GetCurrentRoom() == exit)
                {
                    Console.WriteLine();
                    finished = true;
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
            player.Name = Console.ReadLine();
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
        private void PrintGameOver()
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

        /**
         * Given a command, process (that is: execute) the command.
         * @param command The command to be processed.
         * @return true If the command ends the game, false otherwise.
         */
        private bool ProcessCommand(Command command)
        {
            bool wantToQuit = false;

            CommandWord commandWord = command.GetCommandWord();

            switch (commandWord)
            {
                case CommandWord.UNKNOWN:
                    Console.WriteLine("I don't know what you mean...");
                    break;

                case CommandWord.HELP:
                    PrintHelp();
                    break;

                case CommandWord.GO:
                    GoRoom(command);
                    break;

                case CommandWord.QUIT:
                    wantToQuit = Quit(command);
                    break;

                case CommandWord.LOOK:
                    Look(command);
                    break;

                case CommandWord.TAKE:
                    Take(command);
                    break;

                case CommandWord.INVENTORY:
                    Inventory();
                    break;

                case CommandWord.BACK:
                    GoBack(command);
                    break;

                case CommandWord.DROP:
                    Drop(command);
                    break;

                // case EAT:
                //     eat(command);
                //   break;

                //case CHARGE:
                // charge();
                //break;

                // case FIRE:
                // fire();
                // break;

                case CommandWord.USE:
                    UseItem(command);
                    break;

                case CommandWord.ATTACK:
                    Attack(command);
                    break;
            }
            return wantToQuit;
        }

        // implementations of user commands:

        /**
         * Print out some help information.
         * Here we print some stupid, cryptic message and a list of the 
         * command words.
         */
        private void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the castle.");
            Console.WriteLine();
            Console.WriteLine("Your command words are:");
            parser.ShowCommands();
        }

        /** 
         * Try to go in one direction. If there is an exit, enter
         * the new room, otherwise print an error message.
         */
        private void GoRoom(Command command)
        {
            if (!command.HasSecondWord())
            {
                // if there is no second word, we don't know where to go...
                Console.WriteLine("Go where?");
                return;
            }
            string direction = command.GetSecondWord();
            Console.WriteLine(player.GoNewRoom(direction));
        }

        /**
         * take player back to previous room they were in.
         */
        private void GoBack(Command command)
        {
            if (command.HasSecondWord())
            {
                Console.WriteLine("Back where?");
                return;
            }
            else
            {
                Console.WriteLine(player.GoBack());
            }
        }

        /**
         * looks in the room and gives description.
         */
        private void Look(Command command)
        {
            Console.WriteLine(player.GetRoomDescription());
        }

        /**
         * prints the take item command from the player class
         */
        private void Take(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("what would you like to take?");
                return;
            }
            string name = command.GetSecondWord();
            Console.WriteLine(player.TakeItem(name));
        }

        /**
         * uses the items in the inventory, if the player has a key they can unlock the door, if they have a lantern they will light the room in the kitchen
         * if they have a potion or armour they can increase their health.
         */
        private void UseItem(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("what item would you like to use?");
            }
            string item = command.GetSecondWord();

            if (item.Equals("key", StringComparison.OrdinalIgnoreCase))
            {
                if (player.GetInventoryString().Contains("key") && player.GetCurrentRoom() == entryway)
                {
                    Console.WriteLine("you unlocked the door, go south to leave");
                    entryway.SetExit("south", exit);
                }
                else
                {
                    Console.WriteLine("you cannot use key here");
                }
            }
            if (item.Equals("lantern"))
            {
                if (player.GetInventoryString().Contains("lantern") && player.GetCurrentRoom() == kitchen)
                {
                    Console.WriteLine("you are in a nasty kitchen and see a sword lying on the ground");
                    kitchen.SetItem("sword", sword);
                }
                else
                {
                    Console.WriteLine("you cannot use the lantern here");
                }
            }
            if (item.Equals("armour"))
            {
                player.EquipItem();
                player.RemoveFromBackpack("armour");
                Console.WriteLine("you are now wearing the armour, this will help you last longer when fighting enemies.");
                Console.WriteLine(player.GetInventoryString());
            }
            if (item.Equals("potion"))
            {
                player.EquipItem();
                player.RemoveFromBackpack("potion");
                // do the actual health increase
                Console.WriteLine("you took the potion and have increased your health");
                Console.WriteLine(player.GetInventoryString());
            }
        }


        /**
         * adds the command to attack the monster
         */
        private void Attack(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("what are you attacking?");
                return;
            }
            string name = command.GetSecondWord();

            Console.WriteLine(player.attack(name));

            
            

            if (!ghoul.IsAlive())
            {
                bathroom.SetItem("potion", potion);
                Console.WriteLine("\nyou killed the ghoul, take the potion to increase your health.\n");
            } 
            else if (!dragon.IsAlive())
            {
                dungeon.SetItem("key", key);
                Console.WriteLine("\nthe dragon has been slain! take the key and escape!");
            }
            
        }
        /**
         * prints the drop item command from the player class
         */
        private void Drop(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("what item would you like to drop?");
                return;
            }

            string name = command.GetSecondWord();

            Console.WriteLine(player.DropItem(name));
        }

        /**
         * 
         
        private void eat(Command command)
        {
            if(!command.hasSecondWord()) {
                Console.WriteLine("what item do you want to eat?");
                return;
            }
            
            String name = command.getSecondWord();
            
            Console.WriteLine(player.eatCookie(name));
            
        }
        */
        /**
         * displays the items in the inventory of the player class using the
         * tostring in the player class.
         */
        private void Inventory()
        {
            Console.WriteLine("you are currently holding: " + player.GetInventoryString());

        }






        /** 
         * "Quit" was entered. Check the rest of the command to see
         * whether we really quit the game.
         * @return true, if this command quits the game, false otherwise.
         */
        private bool Quit(Command command)
        {
            if (command.HasSecondWord())
            {
                Console.WriteLine("Quit what?");
                return false;
            }
            else
            {
                return true;  // signal that we want to quit
            }
        }
    }
}
