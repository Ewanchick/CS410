using System;
using System.Collections.Generic;
using System.Text;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    public class CombatTests
    {
        [Fact]
        public void PlayerAttackReducesMonsterHP()
        {
            //Arrange
            var player = new Player("Test");
            var monster = new Monster("ghoul", hp: 50, lvl: 1, drop: null);

            int startingHP = monster.HP;

            //Act
            int damage = player.DealAttack(monster);
            monster.TakeDamage(damage);

            // Assert
            Assert.True(monster.HP < startingHP);
            Assert.Equal(startingHP - player.Level, monster.HP);
        }
        [Fact]
        public void MonsterHPDoesNotGoBelowZero()
        {
            //Arrange
            var monster = new Monster("Dragon", hp: 30, lvl: 10, drop: null);

            //Act
            monster.TakeDamage(1000);

            //Assert
            Assert.Equal(0, monster.HP);
            Assert.False(monster.IsAlive);
        }
        [Fact]
        public void MonsterDropItemOnDeath()
        {
            var room = new Room("test room");
            var key = new Item("key", "opens door", 0);
            var monster = new Monster("Dragon", hp: 10, lvl: 1, drop: key);

            room.SetMonster("dragon", monster);

            //Act
            monster.TakeDamage(999);

            if (!monster.IsAlive)
            {
                room.SetItem(key.Name, key);
                room.RemoveMonster("dragon");
            }

            //Assert
            Item droppedItem = room.GetItem("key");
            Assert.NotNull(droppedItem);
            Assert.Equal("key", droppedItem.Name);
        }
    }
}
