using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZuulRemake.Classes
{
    public abstract class Entity
    {
        public string Name { get; set; }
        public int HP { get; protected set; }
        public int Level { get; protected set; }
        public Inventory? Inventory { get; protected set; }
        public bool IsAlive => HP > 0;
        public int CarryWeight { get; protected set; } = 0;
        public int MaxWeight { get; protected set; } = 2;
        public Room? CurrentRoom { get; set; }

        public Entity(string name, int hp, int level)
        : this(name, hp, level, Enumerable.Empty<Item>())
        {
            Inventory = new Inventory();
        }

        public Entity(string name, int hp, int level, IEnumerable<Item> startingItems)
        {
            Name = name;
            HP = hp;
            Level = level;

            Inventory = new Inventory();

            if (startingItems.Any())
            {
                Inventory = new Inventory();
                foreach (var item in startingItems)
                {
                    Inventory.AddItem(item);
                }
            }
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




        /**
         * if the player is able to add the item to the Backpack then the player
         * will take the item. if not the game will tell the player it is too
         * heavy.
         */
        // Player should not be able to type in name of item if not in room
        // Make yes or no prompt instead
        public string TakeItem(string name)
        {
            Item? item = CurrentRoom?.GetItem(name);
            if (item == null) return $"{name} isn't in this room";

            if (Inventory == null) Inventory = new Inventory();

            if (Inventory.GetTotalWeight() + item.Weight > MaxWeight)
                return $"{name} is too heavy to carry";
            CurrentRoom?.RemoveItem(item.Name);
            Inventory.AddItem(item);

            return $"Took: {item.Name}";
        }

        /**
         * checks to see if the item is in the Backpack, if the Backpack is
         * empty, it will tell you, otherwise it will remove the item from
         * the Backpack and add it to the room.
         */
        public string DropItem(string name)
        {
            if (Inventory == null) return "Your backpack is empty.";

            Item? itemRemove = Inventory.GetItem(name);
            if (itemRemove == null) return "This item isn't in your backpack.";

            Inventory.RemoveItem(name);
            CurrentRoom?.SetItem(name, itemRemove);

            return $"{name} dropped";
        }

        /**
         * removes the item from the Backpack.
         */
        public void RemoveFromBackpack(string itemRemove)
        {
            Inventory?.RemoveItem(itemRemove);
        }

        /**
         * returns an item from the Backpack if it is available.
         */
        public Item GetItemFromBackpack(string itemName)
        {
            if (Inventory == null) return null;

            Item? item = Inventory.GetItem(itemName);
            if (item != null)
                Inventory.RemoveItem(itemName);

            return item;
        }


        /**
         * if the weight of the Backpack is less than the maximum weight
         * and the item is less than the remaining weight available
         * then the player can pick up the item. if not the player is unable
         * to.
         */
        private bool CanCarry(Item item)
        {
            
                return (Inventory?.GetTotalWeight() ?? 0) + item.Weight <= MaxWeight;
            
        }

        public void EquipItem()
        {
            string returnString = "";
            HP += 101;
            returnString += HP;
        }
        public int GetTotalWeight()
        {
            {
                if (Inventory == null) return 0;
                return Inventory.GetTotalWeight();
            }
        }
    }
}

