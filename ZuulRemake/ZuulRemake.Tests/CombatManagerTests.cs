using System;
using System.Collections.Generic;
using System.Text;
using ZuulRemake.Classes;
using Xunit;

namespace ZuulRemake.Tests
{

    public class CombatManagerTests
    {
        /**
         * Ensure that when a player attacks a monster, the monster's HP
         * is reduced by subtracting the player's level from the monster's HP
         */
        [Fact]
        public void PlayerAttackReducesMonsterHP()
        {
            //Arrange
            var p = new Player("Test", hp: 100, level: 10);
            var m = new Monster("Ghoul", hp: 100, level: 10);
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
            var p = new Player("Test", hp: 100, level: 10);
            var m = new Monster("Ghoul", hp: 100, level: 10);
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
            var e = new Player("Test", 10, 1);

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

            var player = new Player("Hero");
            player.GoNewRoom(room);

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