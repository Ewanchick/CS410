using System;
using System.Collections.Generic;
using System.Text;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    public class RoomTraversalTests
    {
        [Fact]
        public void GoNewRoom_ValidDirection_MovesPlayer()
        {
            // Arrange
            var game = new Game();
            var player = GetPrivateField<Player>(game, "player");

            var entryway = GetPrivateField<Room>(game, "entryway");
            var dininghall = GetPrivateField<Room>(game, "dininghall");

            // Act
            var result = player.GoNewRoom("north");

            // Assert
            Assert.Equal(dininghall, player.GetCurrentRoom());
            Assert.Contains("dining hall", result.ToLower());
        }

        [Fact]
        public void GoNewRoom_InvalidDirection_ReturnsError()
        {
            // Arrange
            var game = new Game();
            var player = GetPrivateField<Player>(game, "player");

            // Act
            var result = player.GoNewRoom("upward"); // no such exit

            // Assert
            Assert.Equal("there is no door (or it is locked).", result);
        }

        [Fact]
        public void GoBack_ReturnsToPreviousRoom()
        {
            // Arrange
            var game = new Game();
            var player = GetPrivateField<Player>(game, "player");

            var entryway = GetPrivateField<Room>(game, "entryway");
            var dininghall = GetPrivateField<Room>(game, "dininghall");

            player.GoNewRoom("north"); // move to dining hall
            var result = player.GoBack(); // go back to entryway

            // Assert
            Assert.Equal(entryway, player.GetCurrentRoom());
            Assert.Contains("entryway", result.ToLower());
        }

        [Fact]
        public void GoBack_NoPreviousRoom_ReturnsWarning()
        {
            // Arrange
            var game = new Game();
            var player = GetPrivateField<Player>(game, "player");

            // Act
            var result = player.GoBack();

            // Assert
            Assert.Contains("there is no turning back", result.ToLower());
        }

        [Fact]
        public void GoNewRoom_LockedExit_CannotTraverse()
        {
            // Arrange
            var game = new Game();
            var player = GetPrivateField<Player>(game, "player");

            var exit = GetPrivateField<Room>(game, "exit");
            var entryway = GetPrivateField<Room>(game, "entryway");

            // Lock the south exit
            entryway.SetExit("south", exit, true);

            // Act
            var result = player.GoNewRoom("south");

            // Assert
            Assert.Equal("there is no door (or it is locked).", result);
        }

        // Helper to access private fields
        private T GetPrivateField<T>(object obj, string fieldName)
        {
            return (T)obj.GetType()
                          .GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                          .GetValue(obj);
        }
    }
}

