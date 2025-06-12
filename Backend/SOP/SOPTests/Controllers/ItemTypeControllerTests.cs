using SOP.Archive.DTOs;
using SOP.Controllers;

namespace SOPTests.Controllers
{
    public class ItemTypeControllerTests
    {
        private readonly ItemTypeController _itemTypeController;
        private readonly Mock<IItemTypeRepository> _itemTypeRepositoryMock = new();
        public ItemTypeControllerTests()
        {
            _itemTypeController = new(_itemTypeRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenItemsExists()
        {
            // Arrange
            List<ItemType> itemTypes = new()
            {
                new ItemType
                {
                    Id = 1,
                    TypeName = "Foo",
                },
                new ItemType
                {
                    Id = 2,
                    TypeName = "Foo",
                }
            };

            _itemTypeRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(itemTypes);

            // Act
            var result = await _itemTypeController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ItemTypeResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ItemTypeResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _itemTypeRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemTypeController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ItemTypeResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenitemIsSuccessfullyCreated()
        {
            // Arrange
            ItemTypeRequest itemTypeRequest = new()
            {
                TypeName = "Foo",
            };

            int itemId = 1;
            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Foo",
            };
            _itemTypeRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ItemType>()))
                .ReturnsAsync(itemType);

            // Act
            var result = await _itemTypeController.CreateAsync(itemTypeRequest);

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
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            ItemTypeRequest itemTypeRequest = new()
            {
                TypeName = "Foo",
            };


            _itemTypeRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ItemType>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemTypeController.CreateAsync(itemTypeRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenItemExists()
        {
            // Arrange
            int itemId = 1;
            ItemTypeResponse itemResponse = new()
            {
                Id = 1,
                TypeName = "Foo",
            };

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Foo",
            };
            _itemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(itemType);

            // Act
            var result = await _itemTypeController.FindByIdAsync(itemId);

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
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenItemDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _itemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _itemTypeController.FindByIdAsync(itemId);

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

            _itemTypeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemTypeController.FindByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenItemTypeIsArchived()
        {
            // Arrange
            int itemTypeId = 1;

            Archive_ItemType archive_ItemType = new()
            {
                Id = 1,
                DeleteTime = DateTime.Now,
                TypeName = "Foo",
                ArchiveNote = "Archive note",
            };

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _itemTypeRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(archive_ItemType);

            // Act
            var result = await _itemTypeController.ArchiveByIdAsync(itemTypeId, archiveNoteRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemTypeResponse;
            Assert.NotNull(data);
            Assert.Equal(archive_ItemType.Id, data.Id);
            Assert.Equal(archive_ItemType.DeleteTime, data.DeleteTime);
            Assert.Equal(archive_ItemType.TypeName, data.TypeName);
            Assert.Equal(archiveNoteRequest.ArchiveNote, data.ArchiveNote);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenItemTypeDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _itemTypeRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _itemTypeController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _itemTypeRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemTypeController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
