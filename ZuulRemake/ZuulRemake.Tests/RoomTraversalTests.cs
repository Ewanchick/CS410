using System;
using System.Globalization;
using Xunit;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    public class RoomTraversalTests
    {
        private readonly Game game;
        private readonly Player player;
        private readonly Room entryway;
        private readonly Room dininghall;
        private readonly Room exitRoom;

        public RoomTraversalTests()
        {
            // Arrange once for all tests in this class
            game = new Game();
            player = GetPrivateField<Player>(game, "player");

            entryway = GetPrivateField<Room>(game, "entryway");
            dininghall = GetPrivateField<Room>(game, "dininghall");
            exitRoom = GetPrivateField<Room>(game, "exit");
        }

        [Fact]
        public void GoNewRoom_ValidDirection_MovesPlayer()
        {
            // Act: attempt to move north from the starting room
            var moved = TryMoveByDirection("north");

            // Assert
            Assert.True(moved);
            Assert.Equal(dininghall, player.GetCurrentRoom());
            Assert.Contains("dining hall", player.GetCurrentRoom().ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GoNewRoom_InvalidDirection_DoesNotMove()
        {
            // Act: attempt to move where there is no exit
            var moved = TryMoveByDirection("upward");

            // Assert: did not move and still in entryway
            Assert.False(moved);
            Assert.Equal(entryway, player.GetCurrentRoom());
        }

        [Fact]
        public void GoBack_ReturnsToPreviousRoom()
        {
            // Act: move north then go back
            var moved = TryMoveByDirection("north");
            Assert.True(moved);

            var previous = player.GoBack();

            // Assert
            Assert.Equal(entryway, player.GetCurrentRoom());
            Assert.Contains("entryway", previous.GetShortDescription(), StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GoBack_NoPreviousRoom_Throws()
        {
            // Arrange: new player in current game has no previous rooms (constructor sets starting room)
            // Make sure stack is empty by creating fresh player via reflection
            var freshPlayer = GetPrivateField<Player>(new Game(), "player");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => freshPlayer.GoBack());
        }

        [Fact]
        public void GoNewRoom_LockedExit_CannotTraverse()
        {
            // Arrange: lock south exit on entryway to simulate locked door
            entryway.SetExit("south", exitRoom, true);

            // Act
            var moved = TryMoveByDirection("south");

            // Assert
            Assert.False(moved);
            Assert.Equal(entryway, player.GetCurrentRoom());
        }

        // Helper: Try to move player by a direction string using Room.TryGetExit
        private bool TryMoveByDirection(string direction)
        {
            if (player == null) return false;

            var current = player.GetCurrentRoom();
            if (current.TryGetExit(direction, out var next) && next != null)
            {
                player.GoNewRoom(next);
                return true;
            }
            return false;
        }

        // Helper to access private fields (preserved from original tests)
        private static T GetPrivateField<T>(object obj, string fieldName)
        {
            return (T)obj.GetType()
                          .GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                          .GetValue(obj);
        }
    }
}

