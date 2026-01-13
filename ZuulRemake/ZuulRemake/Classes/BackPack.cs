using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class BackPack
    {
        // instance variables - replace the example below with your own
        private HashMap<String, Item> inventory;

        /**
         * Constructor for objects of class Inventory
         */
        public Backpack()
        {
            inventory = new HashMap<String, Item>();
        }
        /**
         * adds an item to the inventory
         */
        public void addItem(Item itemAdded)
        {
            inventory.put(itemAdded.getName(), itemAdded);
        }
        /**
         * removes an item from the hashmap.
         */
        public void removeItem(String name)
        {
            inventory.remove(name);
        }
        /**
         * pulls an item out of the inventory.
         */
        public Item getItem(String itemGet)
        {
            return inventory.get(itemGet);
        }
        /**
         * checks for the room key in the inventory
         */
        public boolean keyCheck()
        {
            Set<String> keys = inventory.keySet();
            for (String item : keys)
                if (item.equals("key"))
                    return true;
            return false;
        }
        /**
         * displays all of the items in the inventory
         */
        public String inventoryToString()

        {
            String returnString = "";
            Set<String> keys = inventory.keySet();
            for (String item : keys)
            {
                returnString += " " + item;
            }
            if (returnString.equals(""))
            {
                returnString = "backpack is empty";
            }
            return returnString;
        }
        /**
         * adds an item to the inventory hashmap by name and item
         */
        public void add(String name, Item item)
        {
            inventory.put(name, item);
        }
        /**
         * iterates on the weight of the items in the inventory.
         */
        public int getTotalWeight()
        {
            int weight = 0;
            for (Iterator<Item> iter = inventory.values().iterator(); iter.hasNext();)
            {
                weight += iter.next().getWeight();
            }
            return weight;
        }
    }
}
