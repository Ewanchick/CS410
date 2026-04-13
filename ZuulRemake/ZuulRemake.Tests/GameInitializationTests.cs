using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    public class GameInitializationTests
    {
        [Fact]
        public void NewGame_PlayerStartsInEntryway()
        {
            //Arrange
            var game = new Game(new Player("Test"));

            //Act
            var dininghall = game.GetType()
                                  .GetField("dininghall", System.Reflection.BindingFlags.NonPublic
                                                        | System.Reflection.BindingFlags.Instance)
                                  .GetValue(game) as Room;
            var ballroom = game.GetType()
                               .GetField("ballroom", System.Reflection.BindingFlags.NonPublic
                                                     | System.Reflection.BindingFlags.Instance)
                               .GetValue(game) as Room;
            var dungeon = game.GetType()
                              .GetField("dungeon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                              .GetValue(game) as Room;

            var bathroom = game.GetType()
                               .GetField("bathroom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                               .GetValue(game) as Room;

            //Act & Assert
            Assert.Contains("lantern", dininghall.GetItems().ToLower());
            Assert.Contains("armour", ballroom.GetItems().ToLower());
            Assert.Contains("dragon", dungeon.GetMonsters().ToLower());
            Assert.Contains("ghoul", bathroom.GetMonsters().ToLower());
        }
        [Fact]
        public void NewGame_PlayerStatsAreDefault()
        {
            var game = new Game(new Player("Test"));

            Assert.Equal(100, game.Player.HP);
            Assert.Equal(2, game.Player.MaxWeight);
            Assert.Equal(0, game.Player.CarryWeight);
            Assert.Equal(50, game.Player.Level);
        }
    }
}