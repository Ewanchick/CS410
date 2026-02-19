using System;
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
        private readonly Inventory Backpack = new Inventory();

        private readonly Stack<Room> PreviousRooms = new Stack<Room>();
        private Room? CurrentRoom { get; set; }
        private Room? ChargeRoom { get; set; }

        public int CarryWeight { get; private set; } = 0;
        public int MaxWeight { get; private set; } = 2;

        public Player(string name) : base(name, hp: 100, level: 10)
        {

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
    }
}
