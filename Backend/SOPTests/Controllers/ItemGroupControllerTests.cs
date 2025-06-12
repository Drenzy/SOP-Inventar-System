using SOP.Archive.DTOs;
using SOP.Controllers;
using SOP.Entities;

namespace SOPTests.Controllers
{
    public class ItemGroupControllerTests
    {
        private readonly ItemGroupController _itemGroupController;
        private readonly Mock<IItemGroupRepository> _itemGroupRepositoryMock = new();
        public ItemGroupControllerTests()
        {
            _itemGroupController = new(_itemGroupRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenItemGroupsExists()
        {
            // Arrange
            new Archive_ItemType
            {
                Id = 1,
                TypeName = "Computer"
            };

            List<ItemGroup> itemGroup = new()
            {
                new ItemGroup
                {
                    Id = 1,
                    ItemTypeId = 1,
                    ModelName = "MITTZON",
                    Price = 3900.00m,
                    Manufacturer = "IKEA",
                    WarrantyPeriod = "10 år",
                    Quantity = 30
                },
                new ItemGroup
                {
                    Id = 2,
                    ItemTypeId = 1,
                    ModelName = "MILLBERGET",
                    Price = 559.00m,
                    Manufacturer = "IKEA",
                    WarrantyPeriod = "10 år",
                    Quantity = 30
                },
            };

            _itemGroupRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(itemGroup);

            // Act
            var result = await _itemGroupController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ItemGroupResponse>>(objectResult.Value);

            var data = objectResult.Value as List<ItemGroupResponse>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            _itemGroupRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            // Act
            var result = await _itemGroupController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ItemGroupResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenItemGroupIsSuccessfullyCreated()
        {
            // Arrange
            ItemGroupRequest itemGroupRequest = new()
            {
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            int itemGroupId = 1;
            ItemGroup itemGroup = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            _itemGroupRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ItemGroup>()))
                .ReturnsAsync(itemGroup);

            // Act
            var result = await _itemGroupController.CreateAsync(itemGroupRequest);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemGroupResponse;

            Assert.NotNull(data);
            Assert.Equal(itemGroupId, data.Id);
            Assert.Equal(itemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(itemGroup.ModelName, data.ModelName);
            Assert.Equal(itemGroup.Price, data.Price);
            Assert.Equal(itemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, data.WarrantyPeriod);
            Assert.Equal(itemGroup.Quantity, data.Quantity);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arange
            ItemGroupRequest itemGroupRequest = new()
            {
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };
            _itemGroupRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ItemGroup>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemGroupController.CreateAsync(itemGroupRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenItemGroupExists()
        {
            // Arrange
            int itemGroupId = 1;
            ItemGroupResponse itemGroupResponse = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            ItemGroup itemGroup = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };
            _itemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(itemGroup);

            // Act
            var result = await _itemGroupController.FindByIdAsync(itemGroupId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as ItemGroupResponse;
            Assert.NotNull(data);
            Assert.Equal(itemGroupId, data.Id);
            Assert.Equal(itemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(itemGroup.ModelName, data.ModelName);
            Assert.Equal(itemGroup.Price, data.Price);
            Assert.Equal(itemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, data.WarrantyPeriod);
            Assert.Equal(itemGroup.Quantity, data.Quantity);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenItemGroupDoesNotExist()
        {
            // Arrange
            int itemGroupId = 1;

            _itemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _itemGroupController.FindByIdAsync(itemGroupId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int custoemrId = 1;

            _itemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _itemGroupController.FindByIdAsync(custoemrId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenItemGroupIsUpdated()
        {
            // Arrange
            ItemGroupRequest itemGroupRequest = new()
            {
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            int itemGroupId = 1;
            ItemGroup itemGroup = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };
            _itemGroupRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<ItemGroup>()))
                .ReturnsAsync(itemGroup);


            // Act
            var result = await _itemGroupController.UpdateByIdAsync(itemGroupId, itemGroupRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemGroupResponse;
            Assert.NotNull(data);
            Assert.Equal(itemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(itemGroup.ModelName, data.ModelName);
            Assert.Equal(itemGroup.Price, data.Price);
            Assert.Equal(itemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, data.WarrantyPeriod);
            Assert.Equal(itemGroup.Quantity, data.Quantity);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenItemGroupDoesNotExist()
        {
            // Arrange
            ItemGroupRequest itemGroupRequest = new()
            {
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            int itemGroupId = 1;

            _itemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _itemGroupController.UpdateByIdAsync(itemGroupId, itemGroupRequest);

            // Assert
            var objetResult = result as NotFoundResult;
            Assert.NotNull(objetResult);
            Assert.Equal(404, objetResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange 
            ItemGroupRequest itemGroupRequest = new()
            {
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            int ItemGroupId = 1;

            _itemGroupRepositoryMock.Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<ItemGroup>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _itemGroupController.UpdateByIdAsync(ItemGroupId, itemGroupRequest);

            // Assert
            var objetResult = result as ObjectResult;
            Assert.NotNull(objetResult);
            Assert.Equal(500, objetResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenItemGroupIsArchived()
        {
            // Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            Archive_ItemGroup archive_ItemGroup = new()
            {
                Id = 1,
                ItemTypeId = 1,
                DeleteTime = DateTime.Now,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = ""
            };
            _itemGroupRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(archive_ItemGroup);

            // Act
            var result = await _itemGroupController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_ItemGroupResponse;
            Assert.NotNull(data);
            Assert.Equal(archive_ItemGroup.Id, data.Id);
            Assert.Equal(archive_ItemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(archive_ItemGroup.ModelName, data.ModelName);
            Assert.Equal(archive_ItemGroup.Price, data.Price);
            Assert.Equal(archive_ItemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(archive_ItemGroup.WarrantyPeriod, data.WarrantyPeriod);
            Assert.Equal(archive_ItemGroup.Quantity, data.Quantity);
            Assert.Equal(archiveNoteRequest.ArchiveNote, data.ArchiveNote);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenItemGroupDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _itemGroupRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _itemGroupController.ArchiveByIdAsync(itemId, archiveNoteRequest);

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
                
            };

            _itemGroupRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _itemGroupController.ArchiveByIdAsync(itemId, archiveNoteRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}