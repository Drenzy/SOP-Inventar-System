using Moq;
using SOP.Archive.Controllers;
using SOP.Archive.Entities;

namespace SOPTests.Archive.Controllers
{
    public class Archive_ItemControllerTests
    {
        private readonly Archive_ItemController _archive_ItemController;
        private readonly Mock<IArchive_ItemRepository> _archive_ItemRepositoryMock = new();
        public Archive_ItemControllerTests()
        {
            _archive_ItemController = new(_archive_ItemRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenArchive_ItemsExists()
        {
            //Arrange
            List<Archive_Item> items = new()
            {
                new Archive_Item
                {
                    Id = 1,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "14123VGE34"
                },
                new Archive_Item
                {
                    Id = 2,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "3456345GB45"
                }
            };

            _archive_ItemRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(items);

            //Act
            var result = await _archive_ItemController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<Archive_ItemResponse>>(objectResult.Value);
            var data = objectResult.Value as List<Archive_ItemResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _archive_ItemRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _archive_ItemController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Archive_ItemResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_ItemExists()
        {
            //Arrange
            int itemId = 1;
            ItemResponse itemResponse = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };
            Archive_Item item = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };
            _archive_ItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            //Act
            var result = await _archive_ItemController.FindByIdAsync(itemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(item.RoomId, data.RoomId);
            Assert.Equal(item.SerialNumber, data.SerialNumber);
            Assert.Equal(item.ItemGroupId, data.ItemGroupId);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_ItemDoesNotExist()
        {
            //Arrange
            int itemId = 1;

            _archive_ItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _archive_ItemController.FindByIdAsync(itemId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int itemId = 1;

            _archive_ItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _archive_ItemController.FindByIdAsync(itemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenItemIsDeleted()
        {
            // Arrange
            int itemId = 1;
            Archive_Item archiveItem = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                DeleteTime = DateTime.UtcNow
            };

            _archive_ItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveItem);

            // Act
            var result = await _archive_ItemController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveItem.RoomId, data.RoomId);
            Assert.Equal(archiveItem.SerialNumber, data.SerialNumber);
            Assert.Equal(archiveItem.ItemGroupId, data.ItemGroupId);
            Assert.Equal(archiveItem.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenItemDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenItemIsRestored()
        {
            // Arrange
            int itemId = 1;

            Item item = new Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            _archive_ItemRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            // Act
            var result = await _archive_ItemController.RestoreByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(item.RoomId, data.RoomId);
            Assert.Equal(item.SerialNumber, data.SerialNumber);
            Assert.Equal(item.ItemGroupId, data.ItemGroupId);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenArchive_ItemDoesNotExist()
        {
            //Arrange
            int itemId = 1;

            _archive_ItemRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _archive_ItemController.RestoreByIdAsync(itemId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int itemId = 1;
            _archive_ItemRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _archive_ItemController.RestoreByIdAsync(itemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
