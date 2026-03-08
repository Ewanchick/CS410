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

            Console.WriteLine($"You have entered combat with the {m.Name}!");

            // FIX: was || — battle continued even after a combatant died
            while (p.IsAlive && m.IsAlive)
            {
                PrintCombatantStats(p, m);
                Console.WriteLine($"Will you attack the {m.Name}? (Y/N)");

                string? action = Console.ReadLine()?.Trim().ToLower();

                if (action == "y")
                {
                    Console.WriteLine("You have chosen to attack.");
                    PlayerAttack(p, m);
                }
                else
                {
                    Console.WriteLine("You chose not to attack. The monster seizes the opportunity!");
                }

                // Monster only retaliates if still alive
                if (m.IsAlive)
                    MonsterAttack(p, m);
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

            Console.WriteLine($"You attack the {m.Name} for {p.Level} damage!");
            m.TakeDamage(p.Level);

            if (!m.IsAlive)
                Console.WriteLine($"Your blow was lethal — the {m.Name} collapses!");
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

            Console.WriteLine("--- Combat Status ---");
            Console.WriteLine($"{p.Name} | HP: {p.HP} | Level: {p.Level}");
            // FIX: was p.HP — now correctly m.HP
            Console.WriteLine($"{m.Name}  | HP: {m.HP} | Level: {m.Level}");
            Console.WriteLine("---------------------");
        }

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
                    p.GetCurrentRoom().AddItem(m.Drop);
                    Console.WriteLine($"The {m.Name} dropped: {m.Drop.Name}!");
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