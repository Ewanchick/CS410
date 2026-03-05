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
     * these unique attributes: List<Item> Inventory, Room CurrentRoom, Room ChargeRoom, 
     * int CarryWeight, and int MaxWeight.
     */
    public class Player : Entity
    {
        protected List<Item> Inventory = new();
        private readonly Stack<Room> PreviousRooms = new();
        private readonly NavigationManager navigationManager = new();

        public int CarryWeight => Inventory.Sum(i => i.Weight);
        public int MaxWeight { get; private set; } = 2;

        public Room? CurrentRoom {  get; private set; }
        private Room? ChargeRoom { get; set; }
        
        /**
         * Constructors for objects of class Player
         */
        public Player(string name) : base(name, hp: 100, level: 100) 
        { }
        public Player(string name, int hp, int level) : base(name, 100, 10) 
        { }
        public Player(string name,int hp, int level, IEnumerable<Item>? startingItems)
            : base(name, hp, level)
        {
            if (startingItems != null)
            {
                foreach (var item in startingItems)
                {
                    if (CanCarry(item))
                        Inventory.Add(item);
                }
            }
        }

        /* ------------------------------ LEVEL HP ------------------------------ */

        /**
        * Increases Player Level (damage dealt)
        */
        public void LevelUp(int lvl)
        {
            Level += lvl;
        }
        public void AddHp(int hp)
        {
            HP += hp;
        }

        /* ------------------------------ WEIGHT & INVENTORY ------------------------------ */

        public void AddMaxWeight(int weight)
        {
            MaxWeight += weight;
        }

        public void RemoveMaxWeight(int weight)
        {
            MaxWeight -= weight;
        }

        public bool CanCarry(Item item)
        {
            int totalWeight = CarryWeight + item.Weight;
            return totalWeight <= MaxWeight;
        }

        public bool AddItem(Item item)
        {
            if (!CanCarry(item)) return false;

            Inventory.Add(item);
            return true;
        }

        public bool RemoveItem(Item item)
        {
            return Inventory.Remove(item);
        }
        public string ReadInventory()
        {
            return Inventory.ToString();
        }
        public Item? GetItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return Inventory.FirstOrDefault(i => i.Name != null && Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Item> GetItems()
        {
            return Inventory;
        }


        /* -------------------------- ROOM NAVIGATION -------------------------- */

        /**
         * Return the current room. If it is null, throw an exception.
         */
        public Room GetCurrentRoom()
        {
            if (CurrentRoom == null) throw new InvalidOperationException("CurrentRoom is null.");
            return CurrentRoom;
        }

        /**
         * Navigate the Player to a new room and udpate its CurrentRoom value. 
         * If the Player is already in a room, push this room onto the Stack 
         * of previous rooms before updating Player's location.
         */
        public string GoNewRoom(Room newRoom)
        {
            if (CurrentRoom != null) PreviousRooms.Push(CurrentRoom);
            CurrentRoom = newRoom;
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

        /**
         * Check for available exits to the current room. 
         * If there is no room, there are no exits.
         */
        public string ExitsAvailable()
        {
            if (CurrentRoom == null) throw new InvalidOperationException("CurrentRoom is null.");
            return CurrentRoom.GetExits();
        }

        /* ------------------------------ BEAMER ------------------------------ */

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

    }
}

