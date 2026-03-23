using System;

namespace ZuulRemake.Classes
{
    /// <summary>
    /// Manages combat between a Player and a Monster.
    /// Coordinates turns, processes attacks, and displays outcomes.
    /// </summary>
    public static class CombatManager
    {
        /// <summary>
        /// Runs a full turn-based battle. Each turn, the player is prompted to attack or skip.
        /// The monster always retaliates if still alive.
        /// <exception cref="ArgumentNullException">If player or monster is null.</exception>
        public static void StartBattle(Player p, Monster m)
        {
          
            if (p == null) throw new ArgumentNullException(nameof(p), "Player cannot be null.");
            if (m == null) throw new ArgumentNullException(nameof(m), "Monster cannot be null.");

            Console.WriteLine($"\nYou have entered combat with the {m.Name}!");
            Thread.Sleep(1000);
            while (p.IsAlive && m.IsAlive)
            {
                PrintCombatantStats(p, m);

                Console.WriteLine($"Would you like to attack or attempt to flee the {m.Name}? (A/F) \n");
                string? action = Console.ReadLine()?.ToLower();

                if (action == "a".ToLower())
                {
                    PlayerAttack(p, m);
                    if (m.IsAlive)
                    {
                        MonsterAttack(p, m);
                    }
                }
                else if (action == "f".ToLower())
                {
                    bool escaped = Flee(p, m);
                    if (escaped)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine($"You escaped from the {m.Name}!");
                        return;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        MonsterAttack(p, m);
                    }
                    
                }
                else
                {
                    Thread.Sleep(500);
                    Console.WriteLine("You chose not to attack. The monster seizes the opportunity!");
                    Thread.Sleep(500);
                }
            }

            EndBattle(p, m);
        }

        /// <summary>
        /// The Player attacks the Monster, dealing damage equal to the player's Level.
        /// </summary>
        /// <exception cref="ArgumentNullException">If player or monster is null.</exception>
        public static void PlayerAttack(Player p, Monster m)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (m == null) throw new ArgumentNullException(nameof(m));
            Thread.Sleep(500);
            Console.WriteLine("____________________________________________");
            Console.WriteLine($"You attack the {m.Name} for {p.Level} damage!");
            Thread.Sleep(1000);
            m.TakeDamage(p.Level);
            if (!m.IsAlive)
                Console.WriteLine($"Your blow was lethal — the {m.Name} collapses!");
            Console.WriteLine("____________________________________________");
        }

        /// <summary>
        /// The Monster attacks the Player, dealing damage equal to the monster's Level.
        /// </summary>
        /// <exception cref="ArgumentNullException">If player or monster is null.</exception>
        public static void MonsterAttack(Player p, Monster m)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (m == null) throw new ArgumentNullException(nameof(m));

            Console.WriteLine($"The {m.Name} attacks you for {m.Level} damage!");
            Thread.Sleep(1000);
            p.TakeDamage(m.Level);

            if (!p.IsAlive)
                Console.WriteLine("The attack was fatal...");
        }

        /// <summary>
        /// Displays HP and level of both combatants.
        /// </summary>
        /// <exception cref="ArgumentNullException">If player or monster is null.</exception>
        public static void PrintCombatantStats(Player p, Monster m)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (m == null) throw new ArgumentNullException(nameof(m));
            Console.WriteLine("\n--- Combat Status ---");
            Console.WriteLine($"{p.Name} | HP: {p.HP} | Level: {p.Level}");
            Thread.Sleep(500);
            // FIX: was p.HP — now correctly m.HP
            Console.WriteLine($"{m.Name}  | HP: {m.HP} | Level: {m.Level}");
            Console.WriteLine("---------------------");
            Thread.Sleep(1000);
        }
        /*
         *Player should have the option to flee combat
         *If the player's health is below max HP
         *chance depends on Monster level
         */
        public static bool Flee(Player p, Monster m)
        {
            Random random = new Random();

            //base flee chance is 75
            //harder to flee from higher level monsters
            int fleeChance = 75 - (m.Level * 5);

            //keep the chance within a reasonable range
            if (fleeChance < 20)
                fleeChance = 20;

            if (fleeChance > 90)
                fleeChance = 90;

            int roll = random.Next(1, 101);//roll from 1 - 100

            Console.WriteLine($"You try to flee from the {m.Name}...");
            Console.WriteLine($"Escape chance: {fleeChance}%");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Thread.Sleep(500);
            Console.WriteLine($"You rolled: {roll}");
            Thread.Sleep(500);

            if (roll <= fleeChance)
            {
                return true;
            }
            Console.WriteLine("You failed to escape!");
            return false;
                }

        /**
         * End combat between a Player and Monster. If the Player has lost all HP, print a message informing 
         * them of their defeat. If the monster has lost all HP, inform the player of their success. 
         * Print the Player's remaining HP.
         */
          
        /// <summary>
        /// Concludes the battle. If the monster was defeated and had a drop, it is placed in the room.
        /// </summary>
        /// <exception cref="ArgumentNullException">If player or monster is null.</exception>
        public static void EndBattle(Player p, Monster m)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (m == null) throw new ArgumentNullException(nameof(m));

            if (!p.IsAlive)
            {
                Console.WriteLine("You were defeated in battle...");
                return;
            }

            Console.WriteLine($"You have defeated the {m.Name}!");
            Console.WriteLine($"{p.Name}'s remaining HP: {p.HP}");

            if (m.Drop != null)
            {
                try
                {
                    p.CurrentRoom!.AddItem(m.Drop);
                    Console.WriteLine($"The {m.Name} dropped: {m.Drop.Name}!");
                    Console.WriteLine("Room now contains:");
                    Console.WriteLine(p.GetCurrentRoom().GetItems());
                }
                catch (NoCurrentRoomException ex)
                {
                    Console.WriteLine($"(Could not drop loot: {ex.Message})");
                }
            }

            try
            {
                p.GetCurrentRoom().RemoveMonster(m);
            }
            catch (NoCurrentRoomException)
            {
                // Non-critical — ignore if room context unavailable
            }
        }
    }
}