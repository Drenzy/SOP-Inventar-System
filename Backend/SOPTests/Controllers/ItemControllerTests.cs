using SOP.Archive.DTOs;

namespace SOPTests.Controllers
{
    public class ItemControllerTests
    {
        private readonly ItemController _itemController;
        private readonly Mock<IItemRepository> _itemRepositoryMock = new();
        public ItemControllerTests()
        {
            _itemController = new(_itemRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenItemsExists()
        {
            //Arrange
            List<Item> items = new()
            {
                new Item
                {
                    Id = 1,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "14123VGE34"
                },
                new Item
                {
                    Id = 2,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "3456345GB45"
                }
            };

            _itemRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(items);

            //Act
            var result = await _itemController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ItemResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ItemResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _itemRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _itemController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ItemResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenItemIsSuccessfullyCreated()
        {
            //Arrange
            ItemRequest itemRequest = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            int itemId = 1;
            Item item = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };
            _itemRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Item>()))
                .ReturnsAsync(item);

            //Act
            var result = await _itemController.CreateAsync(itemRequest);

            //Assert
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
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ItemRequest itemRequest = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };


            _itemRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Item>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _itemController.CreateAsync(itemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenItemExists()
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
            Item item = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };
            _itemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);

            //Act
            var result = await _itemController.FindByIdAsync(itemId);

            //Assert
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
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenItemDoesNotExist()
        {
            //Arrange
            int itemId = 1;

            _itemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _itemController.FindByIdAsync(itemId);

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

            _itemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _itemController.FindByIdAsync(itemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenItemIsUpdated()
        {
            //Arrange
            ItemRequest itemRequest = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            int itemId = 1;
            Item item = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };
            _itemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Item>()))
                .ReturnsAsync(item);

            //Act
            var result = await _itemController.UpdateByIdAsync(itemId, itemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemResponse;
            Assert.NotNull(data);
            Assert.Equal(item.RoomId, data.RoomId);
            Assert.Equal(item.SerialNumber, data.SerialNumber);
            Assert.Equal(item.ItemGroupId, data.ItemGroupId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenItemDoesNotExist()
        {
            //Arrange
            ItemRequest itemRequest = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            int itemId = 1;
            _itemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Item>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _itemController.UpdateByIdAsync(itemId, itemRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ItemRequest itemRequest = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            int itemId = 1;
            _itemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Item>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _itemController.UpdateByIdAsync(itemId, itemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenItemIsArchived()
        {
            //Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            Archive_Item item = new()
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                DeleteTime = DateTime.Now,
                ArchiveNote = archiveNoteRequest.ArchiveNote
            };

            _itemRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(item);

            //Act
            var result = await _itemController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_Item;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(item.RoomId, data.RoomId);
            Assert.Equal(item.SerialNumber, data.SerialNumber);
            Assert.Equal(item.ItemGroupId, data.ItemGroupId);
            Assert.Equal(archiveNoteRequest.ArchiveNote, data.ArchiveNote);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenItemDoesNotExist()
        {
            //Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _itemRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _itemController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _itemRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _itemController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
