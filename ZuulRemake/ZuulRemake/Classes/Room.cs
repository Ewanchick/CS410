using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    public class Room(string description)
    {
        private readonly string description = description;
        private readonly Dictionary<string, Room> exits = [];
        private readonly Dictionary<string, bool> lockedExits = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Item> items = [];
        private readonly Dictionary<string, Monster> monsters = [];

        override public string ToString()
        {
            return GetLongDescription() + "\n" + GetRoomItems() + "\n" +
                GetRoomMonsters();
        }
        /**
         * Define the exits of this room.  Every direction either leads
         * to another room or is null (no exit there).
         * @param neighbor the room in the given direction
         * @param direction the direction of the exit
         */
        public void SetExit(string direction, Room neighbor)
        {
            if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentException("direction required");
            exits[direction] = neighbor ?? throw new ArgumentNullException(nameof(neighbor));
            lockedExits[direction] = false;
        }
        /**
         * Define the exits of this room.  Every direction either leads
         * to another room or is null (no exit there).
         * @param neighbor the room in the given direction
         * @param direction the direction of the exit
         */
        public void SetExit(string direction, Room neighbor, bool isLocked)
        {
            if (string.IsNullOrWhiteSpace(direction)) throw new ArgumentException("direction required");
            exits[direction] = neighbor?? throw new ArgumentNullException(nameof(neighbor));
            lockedExits[direction] = isLocked;
        }
        public bool IsExitLocked(string direction)
        {
            return lockedExits.TryGetValue(direction, out var locked) && locked;
        }

        public bool TryGetExit(string direction, out Room? nextRoom)
        {
            nextRoom = null;

            if (!exits.TryGetValue(direction, out var room))
                return false;


            if (IsExitLocked(direction))
                return false;

            nextRoom = room;
            return true;
        }

        public Room GetExitRaw(string direction)
        {
            exits.TryGetValue(direction, out var room);
            return room;
        }
        /**
         * return a description of the rooms exits,
         * for ecample, "exits: north west".
         * @return A description of the available exits.
         */
        public string GetExitString()
        {
            if (exits.Count == 0) return "Exits from this room:";
            var parts = exits.Keys
                .Select(d => IsExitLocked(d) ? $"{d} (locked)" : d);

            return "Exits from this room: " + string.Join(", ", parts);
        }
        public string GetRoomItems()
        {
            if (items.Count == 0) return "There are no items in this room!";
            return "Items in this room: " + string.Join (", ", items.Keys);
        }
        public string GetRoomMonsters()
        {
            if (monsters.Count == 0) return "There are no Monsters in this room!";
            return "Monsters in this room: " + string.Join(", ", monsters.Keys);
        }
        /**
         * @return The description of the room.
         */
        public string GetShortDescription()
        {
            return description;
        }
        /**
         * Return a description of the room in the form:
         *  you are in the kitchen.
         *  Exits: north west
         * @return a long description of this room
         */
        public string GetLongDescription()
        {
            return "You are " + description + ".\n" + GetExitString();
        }
        /**
         * puts the item in the room
         */
        public void SetItem(string itemName, Item item)
        {
            if (string.IsNullOrWhiteSpace(itemName)) throw new ArgumentException("itemName required");
            items[itemName] = item ?? throw new ArgumentNullException(nameof(item));
        }
        /**
         * gets the item from the room
         */
        public Item GetItem(string name)
        {
            items.TryGetValue(name, out var item);
            return item;
        }
        /**
         * removes the item from the room
         */
        public void RemoveItem(string name)
        {
            items.Remove(name);
        }
        /**
         * adds item to list
         */
        public Item AddItem(string name, Item item)
        {
            SetItem(name, item);
            return item;
        }
        /**
         * adds monster to room
         */
        public void SetMonster(string name, Monster monster)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name required");
            monsters[name] = monster ?? throw new ArgumentNullException(nameof(monster));
        }
        public Monster GetMonster(string name)
        {
            monsters.TryGetValue(name, out var monster);
            return monster;
        }
        public bool RemoveMonster(string name)
        {
            return monsters.Remove(name);
        }
    }
}
