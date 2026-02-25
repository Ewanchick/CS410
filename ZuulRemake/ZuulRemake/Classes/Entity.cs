using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    public abstract class Entity
    {
        public string Name { get; set; }
        public int HP { get; protected set; }
        public int Level { get; protected set; }
        public ArrayList Inventory { get; protected set; }   
        public bool IsAlive => HP > 0;
        public Room? CurrentRoom { get; set; }
        public Entity(string name, int hp, int level)
        {
            Name = name;
            HP = hp;
            Level = level;
        }

        public virtual void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public virtual void AddHP(int amount)
        {
            HP += amount;
        }

        public override string ToString()
        {
            return $"{Name}\nHP: {HP}\nLVL: {Level}";
        }
        /* ------------------------------ INVENTORY ------------------------------ */


        /** Inventory logic can be moved to Backpack class. Also, we should probably 
         * rename all instances of backpack to Inventory, especially because there 
         * are usages of BackPack and Backpack. 
         */



        /**
         * takes item out of room and places it into the Backpack as long as
         * you can carry it.
         */
        public bool AddToBackPack(Item item)
        {
            // check if the item can be carried
           

            // remove from current room
            CurrentRoom.RemoveItem(item.Name);

            // add to Backpack
             Inventory.Add(item);

            return true;
        }

        /**
         * checks to see if the item is in the Backpack, if the Backpack is
         * empty, it will tell you, otherwise it will remove the item from
         * the Backpack and add it to the room.
         */
        public string DropItem(string name)
        {
            string returnString = "";
            Item itemRemove = GetItemFromBackpack(name);

            if (itemRemove == null)
            {
                returnString += "this item isnt in your Backpack";
            }
            else
            {
                RemoveFromBackpack(name);
                CurrentRoom.SetItem(name, itemRemove);
                returnString += name + " dropped";
            }
            return returnString;
        }

        /**
         * removes the item from the Backpack.
         */
        public void RemoveFromBackpack(string itemRemove)
        {
            Inventory.Remove(itemRemove);
        }

        /**
         * returns an item from the Backpack if it is available.
         */
        public Item GetItemFromBackpack(string itemName)
        {
            Item removedItem = Inventory.Cast<Item>().FirstOrDefault(i => i.Name == itemName);
            Inventory.Remove(removedItem);
            return removedItem;
        }

        public void EquipItem()
        {
            string returnString = "";
            HP += 101;
            returnString += HP;
        }
        
    }
}

