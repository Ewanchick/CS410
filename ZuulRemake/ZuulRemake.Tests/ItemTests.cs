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
    }
}
