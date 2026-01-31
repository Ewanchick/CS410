using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class Monster
    {
        public string Name { get; set; }
        public int HP { get; set; } = 50;
        public Item? Drop { get; set; }
        public int Level { get; set; } = 50;
        public bool IsAlive => HP > 0;

        /**
         * Constructor for objects of class Monster
         */
        public Monster(string name, int hp, int lvl, Item? drop)
        {
            Name = name;
            HP = hp;
            Level = lvl;
            Drop = drop;
        }

        // don't really need methods for returning drop (drop item method) / level (attack method) since those are public

        /**
         * When Player attacks Monster, this modifies its HP based on damage taken.
         */
        public int TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
            return HP;
        }

        /**
         * Provide detailed description of Monster.
         * If Monster can drop loot, display posessed item.
         */
        public override string ToString()
        {
            string ReturnString;
            ReturnString = $"{Name}\n" +
                           $"HP: {HP}\n" +
                           $"LVL: {Level}\n";
            if (Drop != null) { ReturnString += $"Loot: {Drop}"; }
            return ReturnString;
        }
    }
}
