using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    /**
     * This class represents a Monster and inherits from the Entity class. Any Monster 
     * has the unique attribute Drop, which represents an Item dropped by the Monster 
     * when it has been defeated.
     */
    public class Monster : Entity
    {
        public Item? Drop { get; set; }

        /**
         * Constructor for objects of class Monster.
         */
        public Monster(string name, int hp, int level)
        : base(name, hp, level)  // no starting items
        {
        }

        public Monster(string name, int hp, int level, Item drop)
            : base(name, hp, level)
        {
            Drop = drop;
        }

        /**
         * Provide detailed description of Monster.
         * If Monster can drop loot, display posessed item.
         */
        public override string ToString()
        {
            string result = $"{Name}\nHP: {HP}\nLVL: {Level}";
            if (Inventory != null && Inventory.GetTotalWeight() > 0)
                result += $"\nLoot: {Inventory.InventoryToString()}";
            return result;
        }
    }
}
