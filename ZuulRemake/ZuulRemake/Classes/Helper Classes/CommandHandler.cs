using System;
using System.Net.NetworkInformation;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Classes
{
    /// <summary>
    /// Handles all player commands entered at the console.
    /// Every command branch is wrapped in exception handling so a bad command never crashes the game.
    /// </summary>
    internal class CommandHandler
    {
        private  Player _player;
        private readonly Parser _parser;
        private readonly NavigationManager _navigationManager = new();
        private  Room _entryway;
        private  Room _kitchen;
        private int ringCount;
        private readonly Room _exit;

        /// <exception cref="ArgumentNullException">If any required argument is null.</exception>
        public CommandHandler(Player player, Parser parser, Room entryway, Room kitchen, Room exit)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _entryway = entryway ?? throw new ArgumentNullException(nameof(entryway));
            _kitchen = kitchen ?? throw new ArgumentNullException(nameof(kitchen));
            _exit = exit ?? throw new ArgumentNullException(nameof(exit));
        }

        /// <summary>
        /// Processes a single command. Returns true if the player has requested to quit.
        /// A top-level catch ensures no unhandled exception escapes to the game loop.
        /// </summary>
        public bool ProcessCommand(Command command)
        {
            if (command == null)
            {
                Console.WriteLine("No command received.");
                return false;
            }

            try
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
                        Console.WriteLine(_navigationManager.MovePlayer(_player, command.GetSecondWord()!));
                        return false;

                    case CommandWord.QUIT:
                        return Quit(command);

                    case CommandWord.LOOK:
                        Look();
                        return false;

                    case CommandWord.INVENTORY:
                        Inventory();
                        return false;

                    case CommandWord.BACK:
                        GoBack(command);
                        return false;

                    case CommandWord.TAKE:
                        Take(command);
                        return false;

                    case CommandWord.DROP:
                        Drop(command);
                        return false;

                    case CommandWord.USE:
                        UseItem(command);
                        return false;

                    case CommandWord.ATTACK:
                        Attack(command);
                        return false;

                    default:
                        Console.WriteLine("Unrecognised command.");
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Last-resort safety net — no unhandled exception should escape the game loop
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        /* ------------------------------ HELP ------------------------------ */

        private void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander around the castle.");
            Console.WriteLine();
            Console.WriteLine("Your command words are:");
            _parser.ShowCommands();
        }

        /* ------------------------------ NAVIGATION ------------------------------ */

        /// <summary>
        /// Previously called Console.WriteLine(player.GoBack()) directly, which would crash
        /// with an unhandled exception if there were no previous rooms. Now catches
        /// NoRoomHistoryException and prints a friendly message instead.
        /// </summary>
        private void GoBack(Command command)
        {
            if (command.HasSecondWord())
            {
                Console.WriteLine("Back where? Just type 'back'.");
                return;
            }

            try
            {
                Room previous = _player.GoBack()!;
                Console.WriteLine($"You go back to the {previous.Name}.");
                Console.WriteLine(previous.GetLongDescription());
            }
            catch (NoRoomHistoryException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Look()
        {
            try
            {
                Console.WriteLine(_player.GetCurrentRoom().GetLongDescription());
            }
            catch (NoCurrentRoomException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /* ------------------------------ INVENTORY ------------------------------ */

        private void Inventory()
        {
            Console.WriteLine("You are currently holding: " + _player.ReadInventory());
        }

        /// <summary>
        /// Now null-checks the result and gives
        /// the player a clear message if the item isn't in the room.
        /// </summary>
        
        private void Take(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("Take what? Please specify an item name.");
                return;
            }
            string name = command.GetSecondWord()!;

            Item item = _player.CurrentRoom!.GetItem(name)!;
            if (item == null)
            {
                Console.WriteLine($"There is no '{name}' in this room.");
                return;
            }
            bool added = _player.AddItem(item);
            if (added)
            {
                try
                {
                    _player.GetCurrentRoom().RemoveItem(item);
                    Console.WriteLine($"You take the {item.Name}.");
                }
                catch (NoCurrentRoomException ex)
                {
                    Console.WriteLine($"Could not take item: {ex.Message}");
                }

            }
            else if (!_player.CanCarry(item))
                {
                Console.WriteLine("You're carrying too many items! Use 'drop' followed by an item you no longer need.");
            }
        }

        /// <summary>
        /// Now null-checks the result and gives
        /// the player a clear message if the item isn't in their inventory.
        /// </summary>
        private void Drop(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("Drop what? Please specify an item name.");
                return;
            }

            string name = command.GetSecondWord()!;
            Item? item = _player.GetItem(name);

            if (item == null)
            {
                Console.WriteLine($"You don't have a '{name}' to drop.");
                return;
            }

            bool removed = _player.RemoveItem(item);
            if (removed)
            {
                try
                {
                    _player.GetCurrentRoom().AddItem(item);
                    Console.WriteLine($"You drop the {item.Name}.");
                }
                catch (NoCurrentRoomException ex)
                {
                    Console.WriteLine($"Could not drop item: {ex.Message}");
                }
            }
        }
        /// <summary>
        ///  Sleep command will do a roll based on the rooms danger level
        /// Then do a roll to see if the sleep goes off successfully
        /// If the sleep roll fails they are attacked by a nightmare
        /// </summary>
        /// <param name="command"></param>
        private void Sleep()
        {
            Random rn = new Random();
            int numberInRange = rn.Next(1, 51);
            var ring = new Item("Ring", "It doesn't seem to do anything...", 0, 0);
            var nightmare = new Monster("nightmare", 100, 30, ring);
            if (numberInRange <= 20)
            {
                Console.WriteLine($"As your eyes drift to sleep you start to hear the fient sound of laughing... \n");
                Thread.Sleep(2000);
                Console.WriteLine("With each passing moment the laughter grows louder and more malicious... \n");
                Thread.Sleep(3000);
                Console.WriteLine("What started as an impish giggle turned into a maniancal bloodcurling cackle as you see the feint outline of a humanlike figure standing in front of you... \n");
                Thread.Sleep(3000);
                Console.WriteLine("It creeps closer, you are able to see a figure fully covered in robes and a face covered by a featureless white mask... \n");
                Thread.Sleep(2000);
                Console.WriteLine("The figure attacks!!! \n");
                Thread.Sleep(3000);
                CombatManager.StartBattle(_player, nightmare);
                Console.WriteLine("The figure fades away... \n");
            }
            else
            {
                Console.WriteLine($"As your eyes drift to sleep you start to hear the fient sound of laughing... \n");
                Thread.Sleep(2000);
                Console.WriteLine("With each passing moment the laughter grows louder and more malicious... \n");
                Thread.Sleep(3000);
                Console.WriteLine("You bolt awake feeling a sense of panic but you just realize it was all a bad dream... You are fully healed. \n");
                Thread.Sleep(3000);
                _player.AddHP(100 - _player.HP);
            }
        }
        /// <summary>
        /// All item lookups are now null-checked before use.
        /// </summary>
        private void UseItem(Command command)
        {
            if (!command.HasSecondWord())
            {
                Console.WriteLine("Use what? Please specify an item name.");
                return;
            }

            switch (command.GetSecondWord()!.ToLower().Trim())
            {
                case "key": UseKey(); break;
                case "lantern": UseLantern(); break;
                case "armour": UseArmour(); break;
                case "potion": UsePotion(); break;
                default:
                    Console.WriteLine($"You don't know how to use '{command.GetSecondWord()}'.");
                    break;
            }
        }      
        private void UseKey()
        {
            Item? key = _player.GetItem("key");
            if (key == null) { Console.WriteLine("You don't have a key."); return; }

            try
            {
                if (_player.GetCurrentRoom() != _entryway)
                {
                    Console.WriteLine("You can't use the key here.");
                    return;
                }
                Exit? south = _player.GetCurrentRoom().GetExit("south");
                if (south == null) { Console.WriteLine("There's no locked exit to unlock here."); return; }
                south.Unlock();
                Console.WriteLine("You unlocked the door. Go south to leave.");
            }
            catch (NoCurrentRoomException ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        private void UseLantern()
        {
            Item? lantern = _player.GetItem("lantern");
            if (lantern == null) { Console.WriteLine("You don't have a lantern."); return; }

            try
            {
                if (_player.GetCurrentRoom() != _kitchen)
                {
                    Console.WriteLine("You can't use the lantern here.");
                    return;
                }
                Console.WriteLine("You light the lantern — a sword is revealed on the ground!");
                _kitchen.AddItem(new Item("Sword", "Heavy and sharp, capable of slaying the mightiest beast.", 1, 50));
                _kitchen.UpdateNarrativeDescription("Still damp and dim, but now visibly filthy... yuck.");
            }
            catch (NoCurrentRoomException ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        private void UseArmour()
        {
            Item? armour = _player.GetItem("armour");
            if (armour == null) { Console.WriteLine("You don't have any armour."); return; }
            int boost = armour.StatIncrease ?? 0;
            _player.RemoveItem(armour);
            _player.AddHP(boost);
            Console.WriteLine($"You put on the armour. It grants you {boost} HP. HP is now {_player.HP}.");
        }

        private void UsePotion()
        {
            Item? potion = _player.GetItem("potion");
            if (potion == null) { Console.WriteLine("You don't have a potion."); return; }
            int heal = potion.StatIncrease ?? 0;
            _player.RemoveItem(potion);
            _player.AddHP(heal);
            Console.WriteLine($"You drink the potion and restore {heal} HP. HP is now {_player.HP}.");
        }
        private void UseRing()
        {

            Item? ring = _player.GetItem("ring");
            if (ring == null) { Console.WriteLine("You don't have a ring."); return; }
            if (ringCount < 1)
            {
                Console.WriteLine($"You put the ring on and nothing happens... Maybe if you rub it?");
                ringCount++;
            }
           if(ringCount < 3)
            {
                Console.WriteLine($"You rub the ring, nothing happens... But maybe if you rub it again?");
                ringCount++;
            }
           if(ringCount == 4)
            {
                Console.WriteLine($"The ring's luster starts to tarnish from all the rubbing, but you see the start of a message hidden under the rings gold plating..." +
                $"Though all you are able to see right now is the letter L.");
                ringCount++;
            }
           if(ringCount == 5)
            {
                Console.WriteLine($"As you keep picking away at the plating another letter starts to appear... The letter O is now visible.");
                ringCount++;
            }
           if(ringCount == 6)
            {
                Console.WriteLine($"As the last part of the gold plating falls away you are greeted with the last letter of the message, L." +
                    $" L...O...L \n");
                Console.WriteLine(".");
                Thread.Sleep(1000);
                Console.WriteLine(".");
                Thread.Sleep(1000);
                Console.WriteLine(".");
                Thread.Sleep(1000);
                Console.WriteLine($"You don't know what this message means but for some reason you feel extremely annoyed. You take 1 psychic Damage.");
                _player.TakeDamage(1);
            }
        }

        /* ------------------------------ COMBAT ------------------------------ */
       /// <summary>
        ///null-checks the result and gives a clear message if the monster isn't present.
        /// </summary>
        private void Attack(Command command)
        {
            if (!command.HasSecondWord())
            {
               Console.WriteLine("Attack what? Please specify a monster name.");
                return;
            }
            try
            {
                string monsterName = command.GetSecondWord()!;
                Room currentRoom = _player.GetCurrentRoom();
                Monster? monster = currentRoom.GetMonster(monsterName);

                if (monster == null)
                {
                    Console.WriteLine($"There is no {monsterName} here!");
                    return;
                }
                CombatManager.StartBattle(_player, monster);


                if (!monster.IsAlive)
                {
                    currentRoom.RemoveMonster(monster);
                }
            }
            catch (NoCurrentRoomException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
           }

        /* ------------------------------ QUIT ------------------------------ */

        private bool Quit(Command command)
        {
            if (command.HasSecondWord()) { Console.WriteLine("Please only type 'quit' to quit game."); return false; }
            return true;
        }
    }
}