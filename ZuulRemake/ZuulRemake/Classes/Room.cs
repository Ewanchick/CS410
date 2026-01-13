using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class Room
    {
        private string description;
        private Dictionary<string, Room> exits;
        private Dictionary<string, Item> items;
        private Dictionary<string, Monster> monsters;
        private bool locked;
        /**
         * Create a room described "description". Initially, it has
         * no exits. "description" is something like "a kitchen" or
         * "an open court yard".
         * @param description The room's description.
         */
        public Room(string description)
        {
            this.description = description;
            exits = new Dictionary<string, Room>();
            items = new Dictionary<string, Item>();
            monsters = new Dictionary<string, Monster>();
        }
        public string tostring()
        {
            return getLongDescription() + "\n" + getRoomItems() + "\n" +
                getRoomMonsters();
        }
        /**
         * Define the exits of this room.  Every direction either leads
         * to another room or is null (no exit there).
         * @param neighbor the room in the given direction
         * @param direction the direction of the exit
         */
        public void setExit(string direction, Room neighbor)
        {
            exits.put(direction, neighbor);
        }
        /**
         * Define the exits of this room.  Every direction either leads
         * to another room or is null (no exit there).
         * @param neighbor the room in the given direction
         * @param direction the direction of the exit
         */
        public void setExit(string direction, Room neighbor, bool isLocked)
        {
            exits.put(direction, neighbor);
            locked = isLocked;
        }
        public bool checkLocked()
        {
            return locked;
        }

        public Room getExit(string direction)
        {
            return exits.get(direction);
        }
        /**
         * return a description of the rooms exits,
         * for ecample, "exits: north west".
         * @return A description of the available exits.
         */
        public string getExitstring()
        {
            string returnstring = "Exits from this room:";
            Set<string> keys = exits.keySet();
            for (string exit : keys)
            {
                returnstring += " " + exit;
            }
            return returnstring;
        }
        public string getRoomItems()
        {
            string returnstring = "Items in this room:";
            Set<string> keys = items.keySet();
            if (keys.isEmpty())
            {
                return "There are no items in this room!";
            }
            for (string item : keys)
            {
                returnstring += " " + item;
            }
            return returnstring;
        }
        public string getRoomMonsters()
        {
            string returnstring = "monsters in room:";
            Set<string> keys = monsters.keySet();
            if (keys.isEmpty())
            {
                return "\nThere are no monsters in here in this room!\n";
            }
            for (string item : keys)
            {
                returnstring += " " + item;
            }
            return returnstring;
        }
        /**
         * @return The description of the room.
         */
        public string getShortDescription()
        {
            return description;
        }
        /**
         * Return a description of the room in the form:
         *  you are in the kitchen.
         *  Exits: north west
         * @return a long description of this room
         */
        public string getLongDescription()
        {
            return "You are " + description + ".\n" + getExitstring();
        }
        /**
         * puts the item in the room
         */
        public void setItem(string itemName, Item item)
        {
            items.put(itemName, item);
        }
        /**
         * gets the item from the room
         */
        public Item getItem(string name)
        {
            return items.get(name);
        }
        /**
         * removes the item from the room
         */
        public Item removeItem(string name)
        {
            return items.remove(name);
        }
        /**
         * adds item to list
         */
        public Item addItem(string name, Item item)
        {
            return items.put(name, item);
        }
        /**
         * adds monster to room
         */
        public Monster setMonster(string name, Monster monster)
        {
            return monsters.put(name, monster);
        }
        public Monster getMonster(string name)
        {
            return monsters.get(name);
        }
        public Monster removeMonster(string name)
        {
            return monsters.remove(name);
        }
    }
}
