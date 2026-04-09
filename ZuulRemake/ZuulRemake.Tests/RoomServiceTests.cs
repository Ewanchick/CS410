using Moq;
using Xunit;
using ZuulRemake.Models;
using ZuulRemake.Repos;
using ZuulRemake.Services;
using static ZuulRemake.Models.Model;

namespace ZuulRemake.Tests
{
    public class RoomServiceTests
    {
        [Fact]
        public void CreateRoom_ValidInput_CallsAdd()
        {
            var mockRepo = new Mock<IRoomRepo>();
            var service = new RoomService(mockRepo.Object);

            service.CreateRoom("Kitchen", "A dark kitchen", "A dark kitchen with broken cabinets");

            mockRepo.Verify(r => r.Add(It.Is<RoomEntity>(room =>
                room.Name == "Kitchen" &&
                room.NarrativeDescription == "A dark kitchen" &&
                room.LongDescription == "A dark kitchen with broken cabinets"
            )), Times.Once);
        }

        [Fact]
        public void CreateRoom_EmptyName_ThrowsArgumentException()
        {
            var mockRepo = new Mock<IRoomRepo>();
            var service = new RoomService(mockRepo.Object);

            Assert.Throws<ArgumentException>(() =>
                service.CreateRoom("", "desc", "long desc"));

            mockRepo.Verify(r => r.Add(It.IsAny<RoomEntity>()), Times.Never);
        }

        [Fact]
        public void GetRoomById_RoomExists_ReturnsMappedRoom()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(1))
                .Returns(new RoomEntity
                {
                    Id = 1,
                    Name = "Kitchen",
                    NarrativeDescription = "A dark kitchen",
                    LongDescription = "A dark kitchen with broken cabinets"
                });

            var service = new RoomService(mockRepo.Object);

            var result = service.GetRoomById(1);

            Assert.NotNull(result);
            Assert.Equal("Kitchen", result.Name);
        }

        [Fact]
        public void GetRoomById_RoomMissing_ReturnsNull()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(99)).Returns((RoomEntity?)null);

            var service = new RoomService(mockRepo.Object);

            var result = service.GetRoomById(99);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllRooms_ReturnsMappedRooms()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetAll()).Returns(new List<RoomEntity>
            {
                new RoomEntity
                {
                    Id = 1,
                    Name = "Kitchen",
                    NarrativeDescription = "Kitchen narrative",
                    LongDescription = "Kitchen long"
                },
                new RoomEntity
                {
                    Id = 2,
                    Name = "Bedroom",
                    NarrativeDescription = "Bedroom narrative",
                    LongDescription = "Bedroom long"
                }
            });

            var service = new RoomService(mockRepo.Object);

            var result = service.GetAllRooms();

            Assert.Equal(2, result.Count);
            Assert.Equal("Kitchen", result[0].Name);
            Assert.Equal("Bedroom", result[1].Name);
        }

        [Fact]
        public void UpdateRoom_RoomExists_ChangesNarrativeAndCallsUpdate()
        {
            var existingRoom = new RoomEntity
            {
                Id = 1,
                Name = "Kitchen",
                NarrativeDescription = "Old narrative",
                LongDescription = "Long description"
            };

            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(1)).Returns(existingRoom);

            var service = new RoomService(mockRepo.Object);

            service.UpdateRoom(1, "New narrative");

            Assert.Equal("New narrative", existingRoom.NarrativeDescription);
            mockRepo.Verify(r => r.Update(existingRoom), Times.Once);
        }

        [Fact]
        public void UpdateRoom_RoomMissing_ThrowsInvalidOperationException()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(99)).Returns((RoomEntity?)null);

            var service = new RoomService(mockRepo.Object);

            Assert.Throws<InvalidOperationException>(() => service.UpdateRoom(99, "New narrative"));

            mockRepo.Verify(r => r.Update(It.IsAny<RoomEntity>()), Times.Never);
        }

        [Fact]
        public void DeleteRoom_RoomExists_CallsDelete()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(1))
                .Returns(new RoomEntity
                {
                    Id = 1,
                    Name = "Kitchen",
                    NarrativeDescription = "desc",
                    LongDescription = "long desc"
                });

            var service = new RoomService(mockRepo.Object);

            service.DeleteRoom(1);

            mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void DeleteRoom_RoomMissing_ThrowsInvalidOperationException()
        {
            var mockRepo = new Mock<IRoomRepo>();
            mockRepo.Setup(r => r.GetById(99)).Returns((RoomEntity?)null);

            var service = new RoomService(mockRepo.Object);

            Assert.Throws<InvalidOperationException>(() => service.DeleteRoom(99));

            mockRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}