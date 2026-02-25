using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    /**
     * This class represents the Player and inherits from the Entity class. The Player has 
     * these unique attributes: BackPack Backpack, Room CurrentRoom, Room ChargeRoom, 
     * int CarryWeight, and int MaxWeight.
     */

    public class Player : Entity
    {


        private readonly Stack<Room> PreviousRooms = new Stack<Room>();
        private Room? ChargeRoom { get; set; }

        public int CarryWeight { get; private set; } = 0;
        public int MaxWeight { get; private set; } = 2;

        public Player(string name) : base(name, hp: 100, level: 10)
        { }

        /* ------------------------------ LEVEL ------------------------------ */
        /**
 * Increases Player Level (damage dealt)
 */
        public void LevelUp(int lvl)
        {
            Level += lvl;
        }
        /* ------------------------------ INVENTORY ------------------------------ */
       public bool PickUp(Item item)
        {
            if (item.Weight + CarryWeight > MaxWeight)
            {
                return false; // too heavy
            }
            else
            {
                AddToBackPack(item);
                return true;
            }
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

        /* -------------------------- ROOM NAVIGATION -------------------------- */

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

        public string SetCurrentRoom(Room nextRoom)
        {
            string returnString = "";
            CurrentRoom = nextRoom;

            returnString += CurrentRoom.ToString();
            return returnString;
        }
        /**
         * Navigate the Player to a new room and udpate its CurrentRoom value. 
         * If the Player is already in a room, push this room onto the Stack 
         * of previous rooms before updating Player's location.
         */
        public string GoNewRoom(string direction)
        {
            if (!CurrentRoom.TryGetExit(direction, out Room nextRoom)) return "there is no door (or it is locked).";
            // Try to leave current room.
            CurrentRoom = nextRoom;
            return CurrentRoom.ToString();

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

        /**
         * displays the toString from the room class.
         */
        public string GetRoomDescription()
        {
            return CurrentRoom.ToString();
        }
        private bool CanCarry(Item item)
        {
            bool canCarry = true;
            if (GetTotalWeight() > MaxWeight)
            {
                canCarry = false;
            }
                return canCarry;
        }


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
            return Inventory.ToString() + "\nweight: " + GetTotalWeight() + "/" + MaxWeight + "\nHP:" + HP;
        }
        public int GetTotalWeight()
        {
            return Inventory.Cast<Item>().Sum(w => w.Weight);
        }
    }
}

