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

        public string Name { get; set; }
        public int Attack { get; set; }
        public int Hp { get; set; }
        private Item? item { get; set; }
        /**
         * Constructor for objects of class monster
         */
        public Monster(string name, int hp)
        {
            this.Name = name;
            this.Hp = Hp;
        }
        
        

        public string toString()
        {
            return "name: " + Name + "\nAttack: " + Attack + "\nHP: " + Hp;
        }
        /**
         * determines if the monster is still alive
         */
        public bool isAlive()
        {
            return Hp > 0;
        }

        public int gotAttacked()
        {
            if (isAlive())
            {
                Hp -= 50;
            }
            return Hp;
        }

        

        public void Attacking(Player player)
        {
            if (isAlive())
            {
                player.GotHit();
            }
        }
    }
}
