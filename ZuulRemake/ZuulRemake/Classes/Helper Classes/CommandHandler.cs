using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    internal class CommandHandler
    {
        private readonly Player player;
        private readonly Parser parser;
        private readonly WorldBuilder worldBuilder = new WorldBuilder();
        private readonly NavigationManager navigationManager = new NavigationManager();

        private readonly Room entryway, kitchen, exit;

        public CommandHandler(Player player, Parser parser,
            Room entryway, Room kitchen, Room exit
            )
        {
            this.player = player;
            this.parser = parser;
            this.entryway = entryway;
            this.kitchen = kitchen;
            this.exit = exit;

        }
        public bool ProcessCommand(Command command)
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
                    Console.WriteLine(navigationManager.MovePlayer(player, command.GetSecondWord()!));
                    return false;

                case CommandWord.QUIT:
                    return Quit(command);

                case CommandWord.LOOK:
                    Look(command);
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
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the castle.");
            Console.WriteLine();
            Console.WriteLine("Your command words are:");
            parser.ShowCommands();
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
            Console.WriteLine(player.GetCurrentRoom().GetLongDescription());
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
                return;
            }

            string item = command.GetSecondWord()!;
            //To standardize player input
            switch (item.ToLower())
            {
                case "key":
                    if (player.ReadInventory().Contains("key") && player.GetCurrentRoom() == entryway)
                    {
                        Console.WriteLine("you unlocked the door, go south to leave");
                        entryway.GetExit("south").Unlock();
                    }
                    else
                    {
                        Console.WriteLine("you cannot use key here");
                    }
                    break;
                case "lantern":
                    if (player.ReadInventory().Contains("lantern") && player.GetCurrentRoom() == kitchen)
                    {
                        Console.WriteLine("you are in a nasty kitchen and see a sword lying on the ground");
                        Item sword = new Item("sword", "heavy sword, might be used to kill the dragon", 1, 10);
                        kitchen.AddItem(sword);
                    }
                    else
                    {
                        Console.WriteLine("you cannot use the lantern here");
                    }
                    break;
                case "armour":
                    if (player.ReadInventory().Contains("armour"))
                    {
                        var i = player.GetItem("armor");
                        player.AddHP(i.StatIncrease ?? 0);
                        player.RemoveItem(i);
                        Console.WriteLine("you are now wearing the armour, this will help you last longer when fighting enemies.");
                        Console.WriteLine(player.ReadInventory());
                    }
                    break;
                case "potion":
                    {
                        var i = player.GetItem("potion")!;
                        player.AddHP(i.StatIncrease ?? 0);
                        player.RemoveItem(i);
                        // do the actual health increase
                        Console.WriteLine("you took the potion and have increased your health");
                        Console.WriteLine(player.ReadInventory());
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
            string monsterName = command.GetSecondWord()!;
            Room currentRoom = player.GetCurrentRoom();
            Monster? monster = currentRoom.GetMonster(monsterName);

            if (monster == null) 
            {
                Console.WriteLine($"there is no {monsterName} here!");
                return;
            }
            CombatManager.StartBattle(player, monster);

            if (!monster.IsAlive)
            {
                currentRoom.RemoveMonster(monster);
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

            string name = command.GetSecondWord()!;
            var i = player.GetItem(name);
            Console.WriteLine(player.RemoveItem(i));
        }
        /*
            * displays the items in the inventory of the player class using the
            * tostring in the player class.
        */
        private void Inventory()
        {
            Console.WriteLine("you are currently holding: " + player.ReadInventory());
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