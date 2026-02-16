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
            string direction = command.GetSecondWord()!;
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
            string name = command.GetSecondWord()!;
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
                return;
            }
            string item = command.GetSecondWord()!;

            if (item.Equals("key", StringComparison.OrdinalIgnoreCase))
            {
                if (player.GetItemFromBackpack("key") != null && player.GetCurrentRoom() == entryway)
                {
                    Console.WriteLine("you unlocked the door, go south to leave");
                    entryway.SetExit("south", exit);
                }
                else
                {
                    Console.WriteLine("you cannot use key here");
                }
            }
            if (item.Equals("lantern", StringComparison.OrdinalIgnoreCase))
            {
                if (player.GetItemFromBackpack("lantern") != null && player.GetCurrentRoom() == kitchen)
                {
                    Console.WriteLine("you are in a nasty kitchen and see a sword lying on the ground");
                    var sword = new Item("sword","heavy sword, might be used to kill the dragon",1);
                    kitchen.SetItem("sword", sword);
                }
                else
                {
                    Console.WriteLine("you cannot use the lantern here");
                }
            }
            if (item.Equals("armour", StringComparison.OrdinalIgnoreCase))
            {
                if (player.GetItemFromBackpack("armour") == null)
                {
                    Console.WriteLine("you do not have armour");
                    return;
                }
                player.EquipItem();
                player.RemoveFromBackpack("armour");
                Console.WriteLine("you are now wearing the armour, this will help you last longer when fighting enemies.");
                Console.WriteLine(player.GetInventoryString());
            }
            if (item.Equals("potion", StringComparison.OrdinalIgnoreCase))
            {
                if (player.GetItemFromBackpack("potion") == null)
                {
                    Console.WriteLine("you do not have a potion");
                    return;
                }
                player.EquipItem();
                player.RemoveFromBackpack("potion");
                // do the actual health increase
                Console.WriteLine("you took the potion and have increased your health");
                Console.WriteLine(player.GetInventoryString());
            }
        }

        public void AttackMonster(string monsterName)
        {
            // fix
            Monster monster = player.GetCurrentRoom().GetMonster(monsterName);
            if (monster == null)
            {
                Console.WriteLine("There is no such monster here.");
                return;
            }
            int damage = player.DealAttack(monster);
            monster.TakeDamage(damage);

            Console.WriteLine($"You attack the {monster.Name} for {damage} damage");
            Console.WriteLine($"{monster.Name} hp: {monster.HP}");

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
         * adds the command to attack the monster
         */
        private void Attack(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("what are you attacking?");
                return;
            }

            AttackMonster(command.GetSecondWord()!);


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
