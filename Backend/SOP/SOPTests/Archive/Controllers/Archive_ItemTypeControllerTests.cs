using Moq;
using SOP.Archive.Controllers;
using SOP.Archive.Repository;

namespace SOPTests.Archive.Controllers
{
    public class Archive_ItemTypeControllerTests
    {
        private readonly Archive_ItemTypeController _archive_ItemTypeController;
        private readonly Mock<IArchive_ItemTypeRepository> _archive_ItemTypeRepositoryMock = new();
        public Archive_ItemTypeControllerTests()
        {
            _archive_ItemTypeController = new(_archive_ItemTypeRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenArchive_ItemTypesExists()
        {
            // Arrange
            List<Archive_ItemType> itemTypes = new()
            {
                new Archive_ItemType
                {
                    Id = 1,
                    TypeName = "Foo",
                },
                new Archive_ItemType
                {
                    Id = 2,
                    TypeName = "Foo",
                }
            };

            _archive_ItemTypeRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(itemTypes);

            // Act
            var result = await _archive_ItemTypeController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<Archive_ItemTypeResponse>>(objectResult.Value);
            var data = objectResult.Value as List<Archive_ItemTypeResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _archive_ItemTypeRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemTypeController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Archive_ItemTypeResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_ItemTypeExists()
        {
            // Arrange
            int itemId = 1;
            Archive_ItemTypeResponse itemResponse = new()
            {
                Id = 1,
                TypeName = "Foo",
            };
            Archive_ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Foo",
            };
            _archive_ItemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(itemType);

            // Act
            var result = await _archive_ItemTypeController.FindByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemTypeResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(itemType.TypeName, data.TypeName);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_ItemTypeDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemTypeController.FindByIdAsync(itemId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemTypeController.FindByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenItemTypeIsDeleted()
        {
            // Arrange
            int itemId = 1;
            Archive_ItemType archiveItemType = new()
            {
                Id = itemId,
                TypeName = "Test Type",
                DeleteTime = DateTime.UtcNow
            };

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveItemType);

            // Act
            var result = await _archive_ItemTypeController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemTypeResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveItemType.TypeName, data.TypeName);
            Assert.Equal(archiveItemType.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenItemTypeDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemTypeController.DeleteByIdAsync(itemId);

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

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemTypeController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenItemTypeIsRestored()
        {
            // Arrange
            int itemId = 1;

            ItemType itemType = new ItemType
            {
                Id = itemId,
                TypeName = "Test Type",
            };

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(itemType);

            // Act
            var result = await _archive_ItemTypeController.RestoreByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemTypeResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(itemType.TypeName, data.TypeName);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenItemTypeDoesNotExist()
        {
            //Arrange
            int itemId = 1;

            _archive_ItemTypeRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _archive_ItemTypeController.RestoreByIdAsync(itemId);

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
            _archive_ItemTypeRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _archive_ItemTypeController.RestoreByIdAsync(itemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

    }
}
