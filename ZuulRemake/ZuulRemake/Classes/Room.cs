using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    /**
     * This class represents a Room to which the Player can travel. A Room has a Name, 
     * ShortDescriptions, and LongDescriptions. A Room can contain Items and Monsters, 
     * and also contains a collection of Exits which are associated with directions.
     */
    public class Room
    {
        public string Name { get; }
        public string ShortDescription { get; }
        public string LongDescription { get; }

        private readonly List<Item> Items = new();
        private readonly List<Monster> Monsters = new();
        private readonly List<Exit> Exits = new();

        public Room(string name, string shortDescription, string longDescription)
        {
            Name = name;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
        }


        /* NOTES:
         * Try to retreive an object before removing: 
         * public bool TryTakeItem(string name, out Item? item)
         * ^ out = return if found + true, return false if not found
         * - lets you search by name.
         * 
         * Maybe do GetAll___() and use those in the information methods
         * instead of doing that logic in those methods (GetItems(), etc)
         * 
         * Handle all traversal logic in separate class.
         */


        /* ------------------------------ INFORMATION ------------------------------ */
        /**
         * Return a short string informing the Player of the room's Name and ShortDescription.
         */
        override public string ToString()
        {
            return $"You are standing in the {Name}: {ShortDescription}";
        }

        /**
         * Return a longer message detailing the room's Name and ShortDescription, as well as 
         * any Monsters, Items, or Exits in the room.
         */
        public string GetLongDescription()
        {
            return ToString() + "\n" +
            GetItems() + "\n" +
            GetMonsters() + "\n" +
            GetExits();
        }

        /**
         * Check if the room contains any Items. If not, print a message explaining so. 
         * Otherwise, list each Item in the room on a new line and return as a string.
         */
        public string GetItems()
        {
            if (Items.Count == 0)
                return "It seems there are no items in this room...";

            IEnumerable<string>? ItemNames = Items.Select(i => i.Name);
            string itemstr = "Items in this room: " +
            string.Join("\n", ItemNames);
            return itemstr;
        }

        /**
         * Check if the room contains any Monsters. If not, print a message explaining so. 
         * Otherwise, list each Monster in the room on a new line and return as a string.
         */
        public string GetMonsters()
        {
            if (Monsters.Count == 0)
                return "Thankfully, there are no monsters in this room! ...yet.";

            IEnumerable<string>? MonsterNames = Monsters.Select(m => m.Name);
            string monstr = "Monsters in this room: " +
            string.Join("\n", MonsterNames);
            return monstr;
        }

        /**
         * Check if the room contains any Exits. If not, print a message explaining so. Otherwise, 
         * list the Direction of each Exit in the room on a new line and return as a string.
         */
        public string GetExits()
        {
            if (Exits.Count == 0)
                return "This room appears to have no exits...";

            IEnumerable<string>? ExitsAvailable = Exits.Select(i => i.Direction);
            string exitstr = "Exits: " +
            string.Join("\n", ExitsAvailable);
            return exitstr;
        }

        /* ----------------------- EXITS ----------------------- */

        /**
         * Add an exit to this room, including its Direction, the Room it leads to, 
         * and whether or not it is locked.
         */
        public void AddExit(string direction, Room targetRoom, bool isLocked = false)
        {
            if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentException("Direction required.");
            Exits.Add(new Exit(direction.ToLower(), targetRoom, isLocked));
        }

        public Exit? GetExit(string direction)
        {
            return Exits.FirstOrDefault(e => e.Direction.Equals(direction, StringComparison.OrdinalIgnoreCase));
        }

        /* ----------------------- ITEMS ----------------------- */

        /**
         * Add an item to this room.
         */
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        /**
         * Remove an item from this room.
         */
        public void RemoveItem(Item item)
        {
            Items.Remove(item); 
        }

        /**
         * Retrieve an item from this room.
         */
        public Item? GetItem(string name)
        {
            return Items.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /* ---------------------- MONSTERS ---------------------- */

        /**
         * Add a monster to this room.
         */
        public void AddMonster(Monster monster)
        {
            Monsters.Add(monster);
        }

        /**
         * Remove a monster from this room.
         */
        public void RemoveMonster(Monster monster)
        {
            Monsters.Remove(monster);
        }

        /**
         * Retrieve a monster from this room.
         */
        public Monster? GetMonster(string name)
        {
            return Monsters.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /* ---------------------------------------- EXIT CLASS ---------------------------------------- */

        /**
         * This is a nested class for Exit objects. Exits are properties of a Room and are associated with 
         * a direction and the rooms they connect to. Exits may be locked, and can be unlocked with a key.
         */
        public class Exit
        {
            public string Direction { get; }
            public Room TargetRoom { get; }
            public bool IsLocked { get; private set; }

            public Exit(string direction, Room targetRoom, bool isLocked = false)
            {
                Direction = direction;
                TargetRoom = targetRoom;
                IsLocked = isLocked;
            }

            public void Unlock()
            {
                IsLocked = false;
            }
        }
    }
}
