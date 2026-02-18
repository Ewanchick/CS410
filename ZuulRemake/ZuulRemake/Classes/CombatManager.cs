using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZuulRemake.Classes
{
    /**
     * This class manages combat between a Player and a Monster. It is responsible for starting 
     * and ending battles, coordinating turns, and displaying the consequences of each turn.
     */
    public static class CombatManager
    {/**
      * Initiate combat between a Player and a Monster. Prompt the player to attack, and call the 
      * PlayerAttack method if they choose yes. If they choose anything else, call MonsterAttack.
      * After each move, print both combatants' HP.
      */
        public static void StartBattle(Player p, Monster m)
        {
            while (p.IsAlive || m.IsAlive)
            {
                PrintPlayerAndMonsterStats(p, m);
                Console.WriteLine($"Will you attack the {m.Name}? Y/N");
                string? action = Console.ReadLine()?.ToLower();
                bool attack = action == "y";                
                if (attack)
                {
                    Console.WriteLine("You have chosen to attack.");
                    PlayerAttack(p, m);
                }
                else
                {
                    Console.WriteLine("You have chosen not to attack...");
                }
                MonsterAttack(p, m);
            }
        }

        /**
         * The Player attacks the Monster, dealing damage to the Monster's HP based on the Player's Level.
         */
        public static void PlayerAttack(Player p, Monster m)
        {
            Console.WriteLine($"You attack the {m.Name} for {p.Level} points of damage!");
            m.TakeDamage(p.Level);
        }

        /**
         * The Monster attacks the Player, dealing damage to the Player's HP based on the Monster's Level.
         */
        public static void MonsterAttack(Player p, Monster m)
        {
            Console.WriteLine($"The {m.Name} attacks you! You have taken {m.Level} points of damage!");
            p.TakeDamage(m.Level);
        }

        /**
         * Display the health and level of both combatants.
         */
        public static void PrintPlayerAndMonsterStats(Player p, Monster m)
        {
            Console.WriteLine($"{p.Name}'s HP: {p.HP}");
            Console.WriteLine($"Level: {p.Level} \n");
            Console.WriteLine($"The {m.Name}'s HP: {p.HP}");
            Console.WriteLine($"Level: {m.Level} \n");
        }

        /**
         * End combat between a Player and Monster. If the Player has lost all HP, print a message informing 
         * them of their defeat. If the monster has lost all HP, inform the player of their success. 
         * Print the Player's remaining HP.
         */
        public static void EndBattle(Player p, Monster m)
        {
            if (!p.IsAlive)
            {
                Console.WriteLine("You were defeated in battle...");
                return;
            }
            Console.WriteLine($"You have defeated the {m.Name}!");
            Console.WriteLine($"{p.Name}'s remaining HP: {p.HP}");
        }        
    }
}
