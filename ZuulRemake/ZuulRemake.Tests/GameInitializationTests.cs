using System;
using System.Collections.Generic;
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
            var game = new Game();

            //Act
            var dininghall = game.GetType()
                                  .GetField("dininghall", System.Reflection.BindingFlags.NonPublic
                                                        | System.Reflection.BindingFlags.Instance)
                                  .GetValue(game) as Room;
            var ballroom = game.GetType()
                               .GetField("ballroom", System.Reflection.BindingFlags.NonPublic 
                                                     | System.Reflection.BindingFlags.Instance)
                               .GetValue (game) as Room;
            var dungeon = game.GetType()
                              .GetField("dungeon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                              .GetValue(game) as Room;

            var bathroom = game.GetType()
                               .GetField("bathroom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                               .GetValue(game) as Room;

            //Act & Assert
            Assert.Contains("lantern", dininghall.GetRoomItems().ToLower());
            Assert.Contains("armour", ballroom.GetRoomItems().ToLower());
            Assert.Contains("dragon", dungeon.GetRoomMonsters().ToLower());
            Assert.Contains("ghoul", bathroom.GetRoomMonsters().ToLower());
        }
        [Fact]
        public void NewGame_PlayerStatsAreDefault()
        {
            var game = new Game();
            var player = new Game().GetType()
                                   .GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                   .GetValue(game) as Player;

            // Assert default stats
            Assert.Equal(100, player.HP);
            Assert.Equal(2, player.MaxWeight);
            Assert.Equal(0, player.CarryWeight);
            Assert.Equal(10, player.Level);
            Assert.Empty(player.GetInventoryString().Contains("backpack is empty") ? "" : player.GetInventoryString());
        }
    }
}
