using System;
using System.Collections.Generic;
using System.Text;
using ZuulRemake.Classes;
using Xunit;
using Bogus;

namespace ZuulRemake.Tests
{

    public class CombatManagerTests
    {
        Faker<Player> playerFaker = new Faker<Player>()
                .RuleFor(p => p.Name, f => f.Name.FirstName())
                .RuleFor(p => p.HP, f => f.Random.Number(50, 100))
                .RuleFor(p => p.Level, f => f.Random.Number(50, 100));

        Faker<Monster> monsterFaker = new Faker<Monster>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.HP, f => f.Random.Number(50, 100))
            .RuleFor(p => p.Level, f => f.Random.Number(50, 100));

        /**
         * Ensure that when a player attacks a monster, the monster's HP
         * is reduced by subtracting the player's level from the monster's HP
         */
        [Fact]
        public void PlayerAttackReducesMonsterHP()
        {
            //Arrange
            var players = playerFaker.Generate(1);
            var monsters = monsterFaker.Generate(1);
            var p = players.First();
            var m = monsters.First();

            int startingHP = m.HP;

            //Act
            CombatManager.PlayerAttack(p, m);

            // Assert
            Assert.True(m.HP < startingHP);
            Assert.Equal(startingHP - p.Level, m.HP);
        }

        /**
         * Ensure that when a monster attacks a player, the player's HP
         * is reduced by subtracting the monster's level from the player's HP
         */
        [Fact]
        public void MonsterAttackReducesPlayerHP()
        {
            //Arrange
            var players = playerFaker.Generate(1);
            var monsters = monsterFaker.Generate(1);
            var p = players.FirstOrDefault();
            var m = monsters.FirstOrDefault();
            int startingHP = p.HP;

            //Act
            CombatManager.MonsterAttack(p, m);

            // Assert
            Assert.True(p.HP < startingHP);
            Assert.Equal(startingHP - m.Level, p.HP);
        }

        /**
         * Ensure that when a player or monster takes more damage than 
         * the value of their current HP, their HP correctly resets to 0.
         */
        [Fact]
        public void EntityHPDoesNotGoBelowZero()
        {
            //Arrange
            var players = playerFaker.Generate(1);
            var e = players.FirstOrDefault();

            //Act
            e.TakeDamage(100);

            //Assert
            Assert.Equal(0, e.HP);
            Assert.False(e.IsAlive);
        }
        [Fact]
        public void MonsterDropsItemOnDeath()
        {
            // Arrange
            var room = new Room("TestRoom", "This Room is a test", "This room is a test");
            var monster = new Monster("Goblin", hp: 10, level: 1, drop: new Item("Gold", "Shiny", 1, 0));
            room.AddMonster(monster);

            // Act - simulate killing monster
            monster.TakeDamage(20); // More than HP so it dies
            if (!monster.IsAlive && monster.Drop != null)
            {
                room.AddItem(monster.Drop);
            }

            // Assert
            Assert.False(monster.IsAlive);
            Assert.Contains("Gold", room.GetItems());
        }
    }
}