using Xunit;
using ZuulRemake.Classes;

namespace ZuulRemake.Tests
{
    public class ItemTests
    {
        [Fact]
        public void ItemInitialization_WorksCorrectly()
        {
            //arrange
            var sword = new Item("Sword", "A sharp blade", 2, 5);

            //Act & Assert
            Assert.Equal("Sword", sword.Name);
            Assert.Equal("A sharp blade", sword.Description);
            Assert.Equal(2, sword.Weight);
            Assert.Equal(5, sword.StatIncrease);
        }
        [Fact]
        public void ItemToString_ReturnsCorectString()
        {
            //Arrange
            var sword = new Item("Sword", "A sharp blade", 2, 5);

            //Act
            var str = sword.ToString();

            //assert
            Assert.Contains("Sword", str);
            Assert.Contains("A sharp blade", str);
            Assert.Contains("Buff: 5", str);
        }
        [Fact]
        public void ItemToString_ReturnsString_ForPotion()
        {
            // Arrange
            var potion = new Item("Potion", "Heals you", 1, 10);

            // Act
            var str = potion.ToString();

            // Assert
            Assert.Contains("Potion", str);
            Assert.Contains("Heals you", str);
            Assert.Contains("Weight: 1", str);
            Assert.Contains("Health Increase: 10", str);
        }

        [Fact]
        public void PlayerCanPickUpAndDropItem()
        {
            // Arrange
            var room = new Room("Test Room");
            var player = new Player("Hero");
            player.CurrentRoom = room;

            var sword = new Item("Sword", "heavy sword, might be used to kill the dragon", 1,10);
            room.SetItem(sword.Name, sword);

            // Act - pick up item
            string takeResult = player.TakeItem("Sword");

            // Assert pickup
            Assert.Contains("took", takeResult.ToLower());
            Assert.Contains("sword", player.GetInventoryString().ToLower());
            Assert.DoesNotContain("sword", room.GetRoomItems().ToLower());

            // Act - drop item
            string dropResult = player.DropItem("Sword");

            // Assert drop
            Assert.Contains("dropped", dropResult.ToLower());
            Assert.DoesNotContain("sword", player.GetInventoryString().ToLower());
            Assert.Contains("sword", room.GetRoomItems().ToLower());
        }
    }
}
