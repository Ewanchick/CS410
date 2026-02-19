using System;
using System.Collections;
using System.Collections.Generic;
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
            player = new Player("Player");
            CreateRooms();
        }

        // attack monster goes here -> deal damage and take damage methods
        // move these / reorganize methods

        public void AttackMonster(string monsterName)
        {
            // fix
            Monster monster = player.GetCurrentRoom().GetMonster(monsterName);
            if (monster == null)
            {
                Console.WriteLine("There is no such monster here.");
                return;
            }
            
            monster.TakeDamage(player.Level);

            Console.WriteLine($"You attack the {monster.Name}!");
            Console.WriteLine($"{monster.Name} HP: {monster.HP}");


            if (!monster.IsAlive)
            {
                Console.WriteLine($"You defeated the {monster.Name}!");

                if (monster.Drop != null)
                {
                    player.GetCurrentRoom().SetItem(monster.Drop.Name.ToLower(), monster.Drop);
                    Console.WriteLine($"{monster.Name} dropped a {monster.Drop.Name}!");
                }
                player.GetCurrentRoom().RemoveMonster(monsterName);
            }
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
                player.GetCurrentRoom().SetItem(m.Drop.Name.ToLower(), m.Drop);
                Console.WriteLine($"{m.Name} dropped a {m.Drop.Name}!");
            }
            player.GetCurrentRoom().RemoveMonster(m.Name);
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
            sword = new Item("sword", "heavy sword, might be used to kill the dragon", 1, 10);
            lantern = new Item("lantern", "used to light the dark rooms of the castle", 1, 0);
            armour = new Item("armour", "protect yourself from the mighty dragon", 1, 20);
            potion = new Item("Potion", "use this to increase your health!", 1, 50);
            key = new Item("key", "used to unlock the way out", 0, 0);

            //initialize items
            dininghall.SetItem("lantern", lantern);
            ballroom.SetItem("armour", armour);

            // start game in the entryway of castle
            player.GoNewRoom(entryway);  


            //create the monsters
            dragon = new Monster("Dragon", 100, 10, potion);
            ghoul = new Monster("Ghoul", 50, 100, key);

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

            while (true)
            {
                Command command = parser.GetCommand();

                bool quitRequested = ProcessCommand(command);

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
            Console.Write("Please enter your name: ");
            string? name = Console.ReadLine();
            do
            {
                Console.Write("Invalid input. Please enter your name: ");
                name = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            player.Name = name;

            Console.WriteLine("Greetings, " + player.Name);
            Console.WriteLine("You have awoken in a very dark castle with no memory of how you got here. \n" +
                              "Upon attempting to leave, you find that the front door is locked. \n" +
                              "You need to find its key in order to escape this place... but beware! \n" +
                              "Danger lurks around every corner.");
            Console.WriteLine("(Type 'help' at any time to display available commands.)");
            Console.WriteLine();
            Console.WriteLine(player.GetCurrentRoom());
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

        /**
         * Given a command, process (that is: execute) the command.
         * @param command The command to be processed.
         * @return true If the command ends the game, false otherwise.
         */
        private bool ProcessCommand(Command command)
        {
            
            switch (command.GetCommandWord())
            {
                case CommandWord.UNKNOWN:
                    Console.WriteLine("I don't know what you mean...");
                    return false;

                case CommandWord.HELP:
                    PrintHelp();
                    return false;

                case CommandWord.GO:
                    GoRoom(command);
                    return false;

                case CommandWord.QUIT:
                    return Quit(command);

                case CommandWord.LOOK:
                    Look(command);
                    return false;

                case CommandWord.TAKE:
                    Take(command);
                    return false;

                case CommandWord.INVENTORY:
                    Inventory();
                    return false;

                case CommandWord.BACK:
                    GoBack(command);
                    return false;

                case CommandWord.DROP:
                    Drop(command);
                    return false;

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
                    return false;

                case CommandWord.ATTACK:
                    Attack(command);
                    return false;

                default:
                    return false;
            }
           
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
            //Console.WriteLine(player.GoNewRoom(direction));
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
            //Console.WriteLine(player.GetRoomDescription());
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
            //To standardize player input
            switch (item.ToLower())
            {
                case "key":
                    if (player.GetInventoryString().Contains("key") && player.GetCurrentRoom() == entryway)
                    {
                        Console.WriteLine("you unlocked the door, go south to leave");
                        entryway.SetExit("south", exit);
                    }
                    else
                    {
                        Console.WriteLine("you cannot use key here");
                    }
                    break;
                case "lantern":
                    if (player.GetInventoryString().Contains("lantern") && player.GetCurrentRoom() == kitchen)
                    {
                        Console.WriteLine("you are in a nasty kitchen and see a sword lying on the ground");
                        kitchen.SetItem("sword", sword);
                    }
                    else
                    {
                        Console.WriteLine("you cannot use the lantern here");
                    }
                    break;
                case "armour":
                    {
                        player.EquipItem();
                        player.RemoveFromBackpack("armour");
                        Console.WriteLine("you are now wearing the armour, this will help you last longer when fighting enemies.");
                        Console.WriteLine(player.GetInventoryString());
                    }
                    break;
                case "potion":
                    {
                        player.EquipItem();
                        player.RemoveFromBackpack("potion");
                        // do the actual health increase
                        Console.WriteLine("you took the potion and have increased your health");
                        Console.WriteLine(player.GetInventoryString());
                    }
                    break;
                    //Could also add logic to check incase a non command word is used
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
#pragma warning disable CS8604 // Possible null reference argument.
            AttackMonster(command.GetSecondWord());
#pragma warning restore CS8604 // Possible null reference argument.




            if (ghoul.HP == 0)
            {
                bathroom.SetItem("potion", potion);
                Console.WriteLine("\nyou killed the ghoul, take the potion to increase your health.\n");
            } 
            else if (dragon.HP == 0)
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
