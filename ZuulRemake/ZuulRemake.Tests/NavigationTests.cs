using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZuulRemake.Classes;
namespace ZuulRemake.Tests
{
    public class NavigationTests
    {
        private NavigationManager _nav;
        private Player p;
        private Room roomA;
        private Room roomB;
        private Room roomC;

        public NavigationTests()
        {
            _nav = new NavigationManager();
            p = new Player("Player");
            roomA = new Room("Room A", "", "");
            roomB = new Room("Room B", "", "");
            roomC = new Room("Room C", "", "");

            roomA.AddExit("north", roomB, false);
            roomB.AddExit("south", roomA, false);
            roomB.AddExit("north", roomC, true);
            roomC.AddExit("south", roomB, false);
        }

        [Fact]
        public void PlayerCanExitRoomTest()
        {
            // Arrange
            p.GoNewRoom(roomA);

            // Act
            _nav.MovePlayer(p, "north");

            // Assert
            Assert.Equal(roomB, p.CurrentRoom);
        }

        [Fact]
        public void PlayerCanGoBack()
        {
            // Arrange
            p.GoNewRoom(roomA);

            // Act
            _nav.MovePlayer(p, "north");
            p.GoBack();

            // Assert
            Assert.Equal(roomA, p.CurrentRoom);
        }

        [Fact]
        public void PlayerCannotEnterLockedRoom()
        {
            // Arrange
            p.GoNewRoom(roomB);

            // Act
            string attempt = _nav.MovePlayer(p, "north");

            // Assert
            Assert.Equal(roomB, p.CurrentRoom);
            Assert.Equal("The door is locked! A key will unlock it." + "Come back and try again when you have found a key. ", attempt);        
        }
        /*
         *   [Fact]
                public void PlayerCanUnlockExit()
                {
                    // Arrange
                    p.GoNewRoom(roomB);
                    Item key = new Item("key", "", 0, null);
                    p.AddItem(key);

                    // Act
                    // player use key -> call unlock() on this exit
                    _nav.MovePlayer(p, "north");

                    // Assert
                    Assert.Equal(roomC, p.CurrentRoom);
                    Assert.DoesNotContain(key, p.GetItems());
                }
         */

    }
}

    }
}