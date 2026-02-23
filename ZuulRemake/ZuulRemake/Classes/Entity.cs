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
        public Dictionary<string, Item> Inventory = new Dictionary<string, Item>();
        public bool IsAlive => HP > 0;
        public int CarryWeight { get; protected set; } = 0;
        public int MaxWeight { get; protected set; } = 2;
        protected Room? CurrentRoom { get; set; }
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
            if (item.Weight + CarryWeight > MaxWeight)
            {
                return false; // too heavy
            }

            // remove from current room
            CurrentRoom.RemoveItem(item.Name);

            // add to Backpack
             Inventory.Add(item.Name,item);

            return true;
        }


        /**
         * if the player is able to add the item to the Backpack then the player
         * will take the item. if not the game will tell the player it is too
         * heavy.
         */
        // Player should not be able to type in name of item if not in room
        // Make yes or no prompt instead
        public string TakeItem(string name)
        {
            string returnString = "";
            Item item = CurrentRoom.GetItem(name);
            if (item == null)
            {
                returnString += "that item isnt in the room";
            }
            else
            {
                if (AddToBackPack(item))
                {
                    returnString += "took: " + item.ToString();
                }
                else
                {
                    returnString += name + " is too heavy to carry";
                }
            }
            return returnString;
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
        public Item GetItemFromBackpack(string item)
        {
            Item removedItem = Inventory[item];
            Inventory.Remove(item);
            return removedItem;
        }


        /**
         * if the weight of the Backpack is less than the maximum weight
         * and the item is less than the remaining weight available
         * then the player can pick up the item. if not the player is unable
         * to.
         */
        private bool CanCarry(Item item)
        {
            bool canCarry = true;
            int totalWeight = GetTotalWeight() + item.Weight;
            if (totalWeight > MaxWeight)
            {
                canCarry = false;
            }
            return canCarry;
        }

        public void EquipItem()
        {
            string returnString = "";
            HP += 101;
            returnString += HP;
        }
        public int GetTotalWeight()
        {
            foreach( Item i in Inventory.Values)
            {
                CarryWeight += i.Weight;
            }
            int temp = CarryWeight;
            //Resets CarryWeight so will repeated method calls will not lead to 
            //just adding additional wieght onto the previous call
            CarryWeight = 0;
            return temp;
        }
    }
}

