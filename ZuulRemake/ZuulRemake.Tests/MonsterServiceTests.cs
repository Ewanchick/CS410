using Moq;
using Xunit;
using ZuulRemake.Repos;
using ZuulRemake.Services;
using static ZuulRemake.Models.Model;

public class MonsterServiceTests
{
    [Fact]
    public void CreateMonster_ValidInput_CallsAdd()
    {
        var mockRepo = new Mock<IMonsterRepo>();
        var service = new MonsterService(mockRepo.Object);

        service.CreateMonster("Goblin", 10, 1);

        mockRepo.Verify(r => r.Add(It.Is<MonsterEntity>(m =>
            m.Name == "Goblin" &&
            m.HP == 10 &&
            m.Level == 1)), Times.Once);
    }

    [Fact]
    public void GetMonsterById_MonsterExists_ReturnsMonster()
    {
        var mockRepo = new Mock<IMonsterRepo>();
        mockRepo.Setup(r => r.GetById(1))
            .Returns(new MonsterEntity
            {
                Id = 1,
                Name = "Goblin",
                HP = 10,
                Level = 1
            });

        var service = new MonsterService(mockRepo.Object);

        var result = service.GetMonsterById(1);

        Assert.NotNull(result);
        Assert.Equal("Goblin", result.Name);
    }

    [Fact]
    public void UpdateMonster_MonsterExists_UpdatesFieldsAndCallsUpdate()
    {
        var existing = new MonsterEntity
        {
            Id = 1,
            Name = "Goblin",
            HP = 10,
            Level = 1
        };

        var mockRepo = new Mock<IMonsterRepo>();
        mockRepo.Setup(r => r.GetById(1)).Returns(existing);

        var service = new MonsterService(mockRepo.Object);

        service.UpdateMonster(1, 20, 2);

        Assert.Equal(20, existing.HP);
        Assert.Equal(2, existing.Level);
        mockRepo.Verify(r => r.Update(existing), Times.Once);
    }

    [Fact]
    public void DeleteMonster_MonsterExists_CallsDelete()
    {
        var mockRepo = new Mock<IMonsterRepo>();
        mockRepo.Setup(r => r.GetById(1))
            .Returns(new MonsterEntity
            {
                Id = 1,
                Name = "Goblin",
                HP = 10,
                Level = 1
            });

        var service = new MonsterService(mockRepo.Object);

        service.DeleteMonster(1);

        mockRepo.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public void DeleteMonster_MonsterDoesNotExist_Throws()
    {
        var mockRepo = new Mock<IMonsterRepo>();
        mockRepo.Setup(r => r.GetById(99)).Returns((MonsterEntity?)null);

        var service = new MonsterService(mockRepo.Object);

        Assert.Throws<InvalidOperationException>(() => service.DeleteMonster(99));

        mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
    }
}