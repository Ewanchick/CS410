using Xunit;
using ZuulRemake.Classes;
using System.Text;

namespace ZuulRemake.Tests
{
    public class SmokeTest
    {
        [Fact]
        public void Player_CanBeCreated()
        {
            var player = new Player("Test",100,1);
            Assert.NotNull(player);
        }
    }
}
