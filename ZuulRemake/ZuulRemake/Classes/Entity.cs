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
        public List<Item> Inventory { get; protected set; } = new List<Item>();

        public bool IsAlive => HP > 0;

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
    }
}

