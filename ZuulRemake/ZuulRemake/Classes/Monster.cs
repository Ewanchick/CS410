using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZuulRemake.Classes
{
    internal class Monster
    {
        // instance variables - replace the example below with your own

        private String name;
        private int attack;
        private int hp;
        private Item item;
        /**
         * Constructor for objects of class monster
         */
        public Monster(String name, int hp)
        {
            this.name = name;
            this.hp = hp;
        }
        public String getName()
        {
            return name;
        }
        public Item getItem()
        {
            return item;
        }

        public int getHp()
        {
            return hp;
        }

        public String toString()
        {
            return "name: " + name + "\nAttack: " + attack + "\nHP: " + hp;
        }
        /**
         * determines if the monster is still alive
         */
        public boolean isAlive()
        {
            return hp > 0;
        }

        public int gotAttacked()
        {
            if (isAlive())
            {
                hp -= 50;
            }
            return hp;
        }

        public int showHp()
        {
            return hp;
        }

        public void attack(Player player)
        {
            if (isAlive())
            {
                player.gotHit();
            }
        }
    }
}
