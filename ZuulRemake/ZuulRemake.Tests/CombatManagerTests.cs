using System;
using System.Collections.Generic;
using System.Text;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    [TestClass]
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
            var combat = new CombatManager();
            var p = new Player("Test", hp: 100, level: 10);
            var m = new Monster("Ghoul", hp: 100, level: 10);
            int startingHP = m.HP;

            //Act
            combat.PlayerAttack(p, m);

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
            var combat = new CombatManager();
            var p = new Player("Test", hp: 100, level: 10);
            var m = new Monster("Ghoul", hp: 100, level: 10);
            int startingHP = p.HP;

            //Act
            combat.MonsterAttack(p, m);

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
            var e = new Entity("Test", hp: 10);

            //Act
            e.TakeDamage(100);

            //Assert
            Assert.Equal(0, e.HP);
            Assert.False(e.IsAlive);
        }
    }
}
