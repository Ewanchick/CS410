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
            var p = new Player("Test");
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
            var p = new Player("Test");
            var m = new Monster("Ghoul", hp: 100, level: 10);
            int startingHP = p.HP;

            //Act
            CombatManager.MonsterAttack(p, m);

            // Assert
            Assert.True(p.HP < startingHP);
            Assert.Equal(startingHP - m.Level, p.HP);
        }
    }
}
