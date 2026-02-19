using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    public class Player
    {
        private readonly BackPack Backpack = new BackPack();

        private readonly Stack<Room> PreviousRooms = new Stack<Room>();
        private Room? ChargeRoom { get; set; }

        public string Name { get; } = "Player";
        public int HP { get; private set; } = 100;
        public int Level { get; private set; } = 10;
        public int CarryWeight { get; private set; } = 0;
        public int MaxWeight { get; private set; } = 2;

        public Player(string name)
        {
            Name = name;
        }

        /* ------------------------------ HP ------------------------------ */
        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) { HP = 0; }
        }

        public void AddHP(int hp)
        {
            HP += hp;
        }

        /* ------------------------------ LEVEL ------------------------------ */
        public void LevelUp(int lvl)
        {
            Level += lvl;
        }

        /* ------------------------------ WEIGHT ------------------------------ */
        public void AddWeight(int weight)
        {
            CarryWeight += weight;
        }

        public void RemoveWeight(int weight)
        {
            CarryWeight -= weight;
        }

        public void AddMaxWeight(int weight)
        {
            MaxWeight += weight;
        }

        public void RemoveMaxWeight(int weight)
        {
            MaxWeight -= weight;
        }

        /* ------------------------------ ROOM NAVIGATION ------------------------------ */
        
        /**
         * Return the current room. If it is null, throw an exception.
         */
        public Room GetCurrentRoom()
        {
            if (CurrentRoom == null)
            {
                throw new InvalidOperationException("CurrentRoom is null.");
            }
            return CurrentRoom;
        }

        /**
         * Navigate the Player to a new room and udpate its CurrentRoom value. 
         * If the Player is already in a room, push this room onto the Stack 
         * of previous rooms before updating Player's location.
         */
        public void GoNewRoom(Room newRoom)
        {
            if (CurrentRoom != null)
            {
                PreviousRooms.Push(CurrentRoom);
            }

            CurrentRoom = newRoom;
        }

        /**
         * If there are previous rooms to return to, return true. If the 
         * player has yet to visit any rooms before this one, return false.
         */
        public bool CanGoBack()
        {
            return PreviousRooms.Count > 0;
        }

        /**
         * If the player is able to go back to a previous room, update the CurrentRoom 
         * value to the most recently visited room. If there are no rooms to return to, 
         * throw an exception.
         */
        public Room? GoBack()
        {
            if (!CanGoBack()) 
            {
                throw new InvalidOperationException("No previous rooms to go back to.");
            }

            CurrentRoom = PreviousRooms.Pop();
            return CurrentRoom;
        }

        /* ------------------------------ CHARGE BEAMER ------------------------------ */

        /**
         * The Beamer is a mechanism which teleports the player to a room specified by 
         * ChargeRoom. To teleport, the ChargeRoom must first be set to a Room value.
         */
        public void ChargeBeamer(Room room)
        {
            ChargeRoom = room;
        }

        /**
         * If ChargeRoom has been set, return true.
         * If ChargeRoom is null, return false.
         */
        public bool CanFireBeamer()
        {
            return ChargeRoom != null;
        }

        /**
         * If we cannot fire the beamer, throw an exception. 
         * Otherwise, ChargeRoom becomes the new CurrentRoom.
         */
        public Room? FireBeamer()
        {
            if (!CanFireBeamer())
            {
                throw new InvalidOperationException("ChargeRoom is null.");
            }

            GoNewRoom(ChargeRoom);
            return CurrentRoom;
        }

        /**
         * Check for available exits to the current room. 
         * If there is no room, there are no exits.
         */
        public string ExitsAvailable()
        {
            if (CurrentRoom == null)
            {
                return "CurrentRoom is null. There are no exits, because you are not in a room!";
            }
            return CurrentRoom.GetExitString();
        }

        /* ------------------------------ INVENTORY ------------------------------ */






        // everything below here still needs worked on

        /**
         * takes item out of room and places it into the Backpack as long as
         * you can carry it.
         */
        public bool AddToBackPack(Item item)
        {
            // check if the item can be carried
            if (item.Weight + CarryWeight > MaxWeight)
            {
                return false; // too heavy
            }

            // remove from current room
            CurrentRoom.RemoveItem(item.Name);

            // add to Backpack
            Backpack.AddItem(item);

            return true;
        }


        /**
         * this returns the current room that you are in.
         */
        public string EnterRoom(Room nextRoom)
        {
            string returnString = "";
            CurrentRoom = nextRoom;

            returnString += CurrentRoom.ToString();
            return returnString;
        }

        /**
         * if the next room isnt empty or locked it takes you to the next room using
         * commandword go and the direction you want to go to depending on the 
         * given exits. then it pushes the room in the stack.
         */
        public string GoNewRoom(string direction)
        {
            if (!CurrentRoom.TryGetExit(direction, out Room nextRoom)) return "there is no door (or it is locked).";
            // Try to leave current room.
            PreviousRooms.Push(CurrentRoom);
            CurrentRoom = nextRoom;
            return CurrentRoom.ToString();           
        }

        /**
         * displays the toString from the room class.
         */
        public string GetRoomDescription()
        {
            return CurrentRoom.ToString();
        }

        /**
         * if the player is able to add the item to the Backpack then the player
         * will take the item. if not the game will tell the player it is too
         * heavy.
         */
        // Player should not be able to type in name of item if not in room
        // Make yes or no prompt instead
        public string TakeItem(string name)
        {
            string returnString = "";
            Item item = CurrentRoom.GetItem(name);
            if (item == null)
            {
                returnString += "that item isnt in the room";
            }
            else
            {
                if (AddToBackPack(item))
                {
                    returnString += "took: " + item.ToString();
                }
                else
                {
                    returnString += name + " is too heavy to carry";
                }
            }
            return returnString;
        }

        /**
         * checks to see if the item is in the Backpack, if the Backpack is
         * empty, it will tell you, otherwise it will remove the item from
         * the Backpack and add it to the room.
         */
        public string DropItem(string name)
        {
            string returnString = "";
            Item itemRemove = GetItemFromBackpack(name);

            if (itemRemove == null)
            {
                returnString += "this item isnt in your Backpack";
            }
            else
            {
                RemoveFromBackpack(name);
                CurrentRoom.SetItem(name, itemRemove);
                returnString += name + " dropped";
            }
            return returnString;
        }

        /**
         * removes the item from the Backpack.
         */
        public void RemoveFromBackpack(string itemRemove)
        {
            Backpack.RemoveItem(itemRemove);
        }

        /**
         * returns an item from the Backpack if it is available.
         */
        public Item GetItemFromBackpack(string item)
        {
            return Backpack.GetItem(item);
        }


        /**
         * if the weight of the Backpack is less than the maximum weight
         * and the item is less than the remaining weight available
         * then the player can pick up the item. if not the player is unable
         * to.
         */
        private bool CanCarry(Item item)
        {
            bool canCarry = true;
            int totalWeight = Backpack.GetTotalWeight() + item.Weight;
            if (totalWeight > MaxWeight)
            {
                canCarry = false;
            }
            return canCarry;
        }

        public void EquipItem()
        {
            string returnString = "";
            HP += 101;
            returnString += HP;
        }

        /**
         * Reduces HP of the Player based on damage taken.
         */
        public int TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
            return HP;
        }

        /**
         * Increases Player Level (damage dealt)
         */
        public void LevelUp(int levels)
        {
            Level += levels;
        }

        // simplified ^
        
        // Player level determines damage dealt to monster
        // All player does is deal damage
        // weapons increase level


        // MOVE LOTS OF THIS LOGIC TO GAME CLASS
        public string attack(string name)
        {
            string returnString = "";
            Monster monster = CurrentRoom.GetMonster(name);
            if (monster == null)
            {
                returnString += "that monster is no monster in this room";
            }
            else if (GetInventoryString().Contains("sword"))
            {
                HP -= 100;
                returnString += "\nyou attacked the monster" + "\nmonster HP: " + monster.HP;
                returnString += "\nthe monster hit you back";
                monster.TakeDamage(50);
                returnString += "\n" + "your HP: " + HP;
            }
            else
            {
                returnString += " you dont have anything to attack" + name + "with";
            }

            return returnString;
        }

        // MOVE TO GAME CLASS
        /**
         * Ends the game if player HP reaches 0
         */
        public bool gameOver()
        {
            return HP == 0;
        }

        /**
         * Returns items in the inventory
         */
        public string GetInventoryString()
        {
            int totalWeight = Backpack.GetTotalWeight();
            return Backpack.InventoryToString() + "\nweight: " + totalWeight + "/" + MaxWeight + "\nHP:" + HP;
        }

        /**
         * Charges the beamer to memorize the current room
         */
        public string BeamerCharge()
        {
            string returnstring = "";
            ChargeRoom = CurrentRoom;
            returnstring += "charged beamer";
            return returnstring;
        }

        /**
         * fires the beamer to take you to the charge room
         */
        public string BeamerFire()
        {
            string returnstring = "";
            if (ChargeRoom != null)
            {
                EnterRoom(ChargeRoom);
                returnstring += "fired beamer:" + "\n" + GetRoomDescription();
            }
            else
            {
                returnstring += "you have to charge the beamer first";
            }
            return returnstring;
        }
    }
}
