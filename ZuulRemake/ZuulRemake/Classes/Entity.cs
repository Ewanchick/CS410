using System;
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

        public bool IsAlive => HP > 0;

        protected Entity(string name, int hp, int level)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Entity name cannot be null.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Entity name cannot be empty or whitespace.", nameof(name));
            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be at least 1.");

            Name = name;
            HP = hp;
            Level = level;
        }

        public virtual void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
            HP -= damage;
        }

        public virtual void AddHP(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal amount cannot be negative.");
            HP += amount;
        }

        public override string ToString()
        {
            return $"{Name}\nHP: {HP}\nLVL: {Level}";
        }
    }
}

