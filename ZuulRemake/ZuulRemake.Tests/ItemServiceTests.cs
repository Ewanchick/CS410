using Moq;
using Xunit;
using ZuulRemake.Models;
using ZuulRemake.Repos;
using ZuulRemake.Services;
using static ZuulRemake.Models.Model;

public class ItemServiceTests
{
    [Fact]
    public void CreateItem_ValidInput_CallsAdd()
    {
        var mockRepo = new Mock<IItemRepo>();
        var service = new ItemService(mockRepo.Object);

        service.CreateItem("Key", "Opens a locked door", 1);

        mockRepo.Verify(r => r.Add(It.Is<ItemEntity>(i =>
            i.Name == "Key" &&
            i.Description == "Opens a locked door" &&
            i.Weight == 1)), Times.Once);
    }

    [Fact]
    public void GetItemById_ItemExists_ReturnsItem()
    {
        var mockRepo = new Mock<IItemRepo>();
        mockRepo.Setup(r => r.GetById(1))
            .Returns(new ItemEntity
            {
                Id = 1,
                Name = "Potion",
                Description = "Restores health",
                Weight = 1,
                StatIncrease = 10
            });

        var service = new ItemService(mockRepo.Object);

        var result = service.GetItemById(1);

        Assert.NotNull(result);
        Assert.Equal("Potion", result.Name);
    }

    [Fact]
    public void UpdateItem_ItemExists_ChangesFieldsAndCallsUpdate()
    {
        var existing = new ItemEntity
        {
            Id = 1,
            Name = "Potion",
            Description = "Old description",
            Weight = 1
        };

        var mockRepo = new Mock<IItemRepo>();
        mockRepo.Setup(r => r.GetById(1)).Returns(existing);

        var service = new ItemService(mockRepo.Object);

        service.UpdateItem(1, "New description", 3);

        Assert.Equal("New description", existing.Description);
        Assert.Equal(3, existing.Weight);

        mockRepo.Verify(r => r.Update(existing), Times.Once);
    }

    [Fact]
    public void DeleteItem_ItemExists_CallsDelete()
    {
        var mockRepo = new Mock<IItemRepo>();
        mockRepo.Setup(r => r.GetById(1))
            .Returns(new ItemEntity
            {
                Id = 1,
                Name = "Key",
                Description = "Opens a door",
                Weight = 1
            });

        var service = new ItemService(mockRepo.Object);

        service.DeleteItem(1);

        mockRepo.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public void DeleteItem_ItemMissing_Throws()
    {
        var mockRepo = new Mock<IItemRepo>();
        mockRepo.Setup(r => r.GetById(99)).Returns((ItemEntity?)null);

        var service = new ItemService(mockRepo.Object);

        Assert.Throws<InvalidOperationException>(() => service.DeleteItem(99));

        mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
    }
}