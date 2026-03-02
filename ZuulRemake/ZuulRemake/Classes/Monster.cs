using System;

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
         * If Monster can drop loot, display possessed item name.
         */
        public override string ToString()
        {
            var result = $"{Name}\nHP: {HP}\nLVL: {Level}";
            if (Drop != null)
            {
                result += $"\nLoot: {Drop.Name}";
            }
            return result;
        }
    }
}
