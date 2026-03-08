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
            while (p.IsAlive && m.IsAlive)
            {
                PrintPlayerAndMonsterStats(p, m);

                Console.WriteLine($"Woulf you like to Attack or Flee the {m.Name}? (A/F)");
                string? action = Console.ReadLine()?.ToLower();

                if (action == "a")
                {
                    PlayerAttack(p, m);
                    if (m.IsAlive)
                    {
                        MonsterAttack(p, m);
                    }
                }
                else if (action == "f")
                {
                    bool escaped = Flee(p, m);
                    if (escaped)
                    {
                        Console.WriteLine($"You escaped from the {m.Name}!");
                        return;
                    }
                    else
                    {
                        MonsterAttack(p, m);
                    }
                    
                }
                else
                {
                    Console.WriteLine("You hesitate...");
                    MonsterAttack(p, m);
                }
            }
            EndBattle(p, m);
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
            Console.WriteLine($"The {m.Name}'s HP: {m.HP}");
            Console.WriteLine($"Level: {m.Level} \n");
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
            //harder to flee from higher level monsrters
            int fleeChance = 75 - (m.Level * 5);

            //keep the chance within a reasonable range
            if (fleeChance < 20)
                fleeChance = 20;

            if (fleeChance > 90)
                fleeChance = 90;

            int roll = random.Next(1, 101);//roll from 1 - 100

            Console.WriteLine($"You try to flee from the {m.Name}...");
            Console.WriteLine($"Escape chance: {fleeChance}%");
            Console.WriteLine($"You Rolled: {roll}");

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
