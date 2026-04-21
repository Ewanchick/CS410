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
     * NarrativeDescriptions, and LongDescriptions. A Room can contain Items and Monsters, 
     * and also contains a collection of Exits which are associated with directions.
     */
    public class Room
    {
        public string Name { get; }
        public string NarrativeDescription { get; set; }
        public string LongDescription { get; }

        private readonly List<Item> Items = new();
        private readonly List<Monster> Monsters = new();
        private readonly List<Exit> Exits = new();

        public Room(string name, string narrativeDescription, string longDescription)
        {
            Name = name;
            NarrativeDescription = narrativeDescription;
            LongDescription = longDescription;
        }

        /* ------------------------------ INFORMATION ------------------------------ */
        /**
         * Return a short string informing the Player of the room's Name and NarrativeDescription.
         */
        override public string ToString()
        {
            return $"You are standing in the {Name}. \n{NarrativeDescription}";
        }

        /**
         * Return a longer message detailing the room's Name and NarrativeDescription, as well as 
         * any Monsters, Items, or Exits in the room.
         */
        public string GetLongDescription()
        {
            return ToString() + "\n" +
            GetItems() + "\n" +
            GetMonsters() + "\n" +
            GetExits();
        }

        public void UpdateNarrativeDescription(string narDesc)
        {
            NarrativeDescription = narDesc;
        }

        /**
         * Check if the room contains any Items. If not, print a message explaining so. 
         * Otherwise, list each Item in the room on a new line and return as a string.
         */
        public string GetItems()
        {
            if (Items.Count == 0)
                return "";

            IEnumerable<string>? ItemNames = Items.Select(i => i.Name);
            string itemstr = "Items in this room: \n " +
            string.Join("\n ", ItemNames);
            return itemstr;
        }

        public List<Item> GetItemsOb()
        {
            return Items;
        }

        /**
         * Check if the room contains any Monsters. If not, print a message explaining so. 
         * Otherwise, list each Monster in the room on a new line and return as a string.
         */
        public string GetMonsters()
        {
            if (Monsters.Count == 0) return "";

            IEnumerable<string>? MonsterNames = Monsters.Select(m => m.Name);
            string monstr = "Monsters in this room: \n " +
            string.Join("\n ", MonsterNames);
            return monstr;
        }

        public List<Monster> GetMonstersOb()
        {
            return Monsters;
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
            string exitstr = "Exits: \n > " +
            string.Join("\n > ", ExitsAvailable);
            return exitstr;
        }

        public List<Exit> GetExitsOb()
        {
            return Exits;
        }

        /* ----------------------- EXITS ----------------------- */

        /**
         * Add an exit to this room, including its Direction, the Room it leads to, 
         * and whether or not it is locked.
         */
        public void AddExit(Exit exit)
        {
            if (string.IsNullOrWhiteSpace(exit.Direction)) throw new ArgumentException("Direction required.");
            if (exit.TargetRoom == null) throw new ArgumentNullException(nameof(exit.TargetRoom));

            Exits.Add(exit);
        }

        /**
         * Remove an exit from this room.
         */
        public bool RemoveExit(Exit exit)
        {
            return Exits.Remove(exit);
        }

        /**
         * Retrieve an exit from this room.
         */
        public Exit? GetExit(string direction)
        {
            if (string.IsNullOrWhiteSpace(direction)) return null;
            return Exits?.FirstOrDefault(e => e.Direction != null && e.Direction.Equals(direction, StringComparison.OrdinalIgnoreCase));
        }

        /* ----------------------- ITEMS ----------------------- */

        /**
         * Add an item to this room.
         */
        public void AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Items.Add(item);
        }

        /**
         * Remove an item from this room.
         */
        public bool RemoveItem(Item item)
        {
            return Items.Remove(item);
        }

        /**
         * Retrieve an item from this room.
         */
        public Item? GetItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return Items.FirstOrDefault(i => i.Name != null && i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /* ---------------------- MONSTERS ---------------------- */

        /**
         * Add a monster to this room.
         */
        public void AddMonster(Monster monster)
        {
            if (monster == null) throw new ArgumentNullException(nameof(monster));
            Monsters.Add(monster);
        }

        /**
         * Remove a monster from this room.
         */
        public bool RemoveMonster(Monster monster)
        {
            return Monsters.Remove(monster);
        }

        /**
         * Retrieve a monster from this room.
         */
        public Monster? GetMonster(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return Monsters.FirstOrDefault(m => m.Name != null && m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}