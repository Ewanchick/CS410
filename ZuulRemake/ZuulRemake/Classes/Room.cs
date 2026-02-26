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
            try
            {
                Exits.Add(new Exit(direction.ToLower(), targetRoom, isLocked));
            }
            catch
            {
                throw new Exception("Failed to add room. :( ");
            }
        }

        /**
         * Remove an exit from this room.
         */
        public void RemoveExit(Exit exit)
        {
            if (exit != null)
            {
                try
                {
                    Exits.Remove(exit);
                }
                catch
                {
                    throw new NullReferenceException("Failed to remove exit.");
                }
            }
            else
            {
                throw new ArgumentNullException("The provided Exit object is null.");
            }
        }

        /**
         * Retrieve an exit from this room.
         */
        public Exit? GetExit(string direction)
        {
            Exit? ex = Exits?.FirstOrDefault(e => e.Direction != null && Equals(direction, StringComparison.OrdinalIgnoreCase));
            if (ex == null)
            {
                throw new InvalidOperationException($"No Exit was found in direction '{direction}', or expected Exit is null.");
            }
            else
            {
                return ex;
            }
        }

        /* ----------------------- ITEMS ----------------------- */

        /**
         * Add an item to this room.
         */
        public void AddItem(Item item)
        {
            if (item != null)
            {
                Items.Add(item);
            }
            else
            {
                throw new ArgumentNullException("Specified item is null.");
            }
        }

        /**
         * Remove an item from this room.
         */
        public void RemoveItem(Item item)
        {
            if (item != null)
            {
                Items.Remove(item);
            }
            else
            {
                throw new ArgumentNullException("Provided Item is null.");
            }
        }

        /**
         * Retrieve an item from this room.
         */
        public Item? GetItem(string name)
        {
            Item? i = Items.FirstOrDefault(i => i.Name != null && Equals(name, StringComparison.OrdinalIgnoreCase));
            if (i == null)
            {
                throw new InvalidOperationException($"No Item was found with the name '{name}'.");
            }
            else
            {
                return i;
            }
        }

        /* ---------------------- MONSTERS ---------------------- */

        /**
         * Add a monster to this room.
         */
        public void AddMonster(Monster monster)
        {
            if (monster != null)
            {   
                Monsters.Add(monster);
            }
            else
            {
                throw new ArgumentNullException("Provided Monster is null.");
            }
                
        }

        /**
         * Remove a monster from this room.
         */
        public void RemoveMonster(Monster monster)
        {
            if (monster != null)
            {
                Monsters.Remove(monster);
            }
            else
            {
                throw new ArgumentNullException("Provided Monster is null.");
            }
        }

        /**
         * Retrieve a monster from this room.
         */
        public Monster? GetMonster(string name)
        {
            Monster m = Monsters.FirstOrDefault(i => i.Name != null && Equals(name, StringComparison.OrdinalIgnoreCase));
            if (m == null)
            {
                throw new InvalidOperationException($"No Monster was found with name '{name}', or expected Monster is null.");
            }
            else
            {
                return m;
            }
        }
    }
}
