using System;
using System.Collections.Generic;

namespace ZuulRemake.Classes
{
    internal class BackPack
    {
        // instance variables - replace the example below with your own
        private readonly Dictionary<string, Item> inventory;

        /**
         * Constructor for objects of class Inventory
         */
        public  BackPack()
        {
            inventory = new Dictionary<string, Item>();
        }
        /**
         * adds an item to the inventory
         */
        public void AddItem(Item itemAdded)
        {
            inventory.Add(itemAdded.Name, itemAdded);
        }
        /**
         * removes an item from the hashmap.
         */
        public void RemoveItem(string name)
        {
            inventory.Remove(name);
        }
        /**
         * pulls an item out of the inventory.
         */
        public Item? GetItem(string itemGet)
        {
            inventory.TryGetValue(itemGet, out var item);
            return item;
        }
        /**
         * checks for the room key in the inventory
         */
        public bool KeyCheck()
        {

            return inventory.ContainsKey("key");
        }
        /**
         * displays all of the items in the inventory
         */
        public string InventoryToString()

        {
            if (inventory.Count == 0)
                return "Backpack is empty";

            return string.Join(", ", inventory.Keys);
        }
        /**
         * adds an item to the inventory hashmap by name and item
         */
        public void Add(string name, Item item)
        {
            inventory.Add(name, item);
        }
        /**
         * iterates on the weight of the items in the inventory.
         */
        public int GetTotalWeight()
        {
            int weight = 0;
            foreach (Item item in inventory.Values)
            {
                weight += item.Weight;
            }
            return weight;
        }
    }
}
