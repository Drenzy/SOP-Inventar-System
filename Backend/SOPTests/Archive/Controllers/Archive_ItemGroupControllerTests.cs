using Moq;
using SOP.Controllers;

namespace SOPTests.Archive.Controllers
{
    public class Archive_ItemGroupControllerTests
    {
        private readonly Archive_ItemGroupController _archive_ItemGroupController;
        private readonly Mock<IArchive_ItemGroupRepository> _archive_ItemGroupRepositoryMock = new();
        public Archive_ItemGroupControllerTests()
        {
            _archive_ItemGroupController = new(_archive_ItemGroupRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenArchive_ItemGroupsExists()
        {
            // Arrange
            new Archive_ItemType
            {
                Id = 1,
                TypeName = "Computer"
            };

            List<Archive_ItemGroup> itemGroup = new()
            {
                new Archive_ItemGroup
                {
                    Id = 1,
                    ItemTypeId = 1,
                    ModelName = "MITTZON",
                    Price = 3900.00m,
                    Manufacturer = "IKEA",
                    WarrantyPeriod = "10 år",
                    Quantity = 30
                },
                new Archive_ItemGroup
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
            _archive_ItemGroupRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(itemGroup);

            // Act
            var result = await _archive_ItemGroupController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<Archive_ItemGroupResponse>>(objectResult.Value);


            var data = objectResult.Value as List<Archive_ItemGroupResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _archive_ItemGroupRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            // Act
            var result = await _archive_ItemGroupController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ItemGroupResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_ItemGroupExists()
        {
            // Arrange
            int itemGroupId = 1;
            Archive_ItemGroupResponse itemGroupResponse = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            Archive_ItemGroup itemGroup = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };
            _archive_ItemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(itemGroup);

            // Act
            var result = await _archive_ItemGroupController.FindByIdAsync(itemGroupId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as Archive_ItemGroupResponse;
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
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_ItemGroupDoesNotExist()
        {
            // Arrange
            int itemGroupId = 1;

            _archive_ItemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemGroupController.FindByIdAsync(itemGroupId);

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

            _archive_ItemGroupRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _archive_ItemGroupController.FindByIdAsync(custoemrId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenItemGroupIsDeleted()
        {
            // Arrange
            int itemId = 1;

            var archiveItemGroup = new Archive_ItemGroup
            {
                Id = itemId,
                ModelName = "Test Model",
                ItemTypeId = 1,
                Quantity = 10,
                Price = 100.00m,
                Manufacturer = "Test Manufacturer",
                WarrantyPeriod = "2 years",
                DeleteTime = DateTime.UtcNow 
            };

            
            _archive_ItemGroupRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveItemGroup); 

            // Act
            var result = await _archive_ItemGroupController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode); 

            var data = objectResult.Value as Archive_ItemGroupResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveItemGroup.ModelName, data.ModelName);
            Assert.Equal(archiveItemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(archiveItemGroup.Quantity, data.Quantity);
            Assert.Equal(archiveItemGroup.Price, data.Price);
            Assert.Equal(archiveItemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(archiveItemGroup.WarrantyPeriod, data.WarrantyPeriod);
            Assert.Equal(archiveItemGroup.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenItemGroupDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemGroupRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemGroupController.DeleteByIdAsync(itemId);

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

            _archive_ItemGroupRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemGroupController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenItemGroupIsRestored()
        {
            // Arrange
            int itemId = 1;

            ItemGroup itemGroup = new ItemGroup
            {
                Id = itemId,
                ModelName = "Restored Model",
                ItemTypeId = 1,
                Quantity = 5,
                Price = 150.00m,
                Manufacturer = "Restored Manufacturer",
                WarrantyPeriod = "1 year",
            };

            _archive_ItemGroupRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(itemGroup);

            // Act
            var result = await _archive_ItemGroupController.RestoreByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ItemGroupResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(itemGroup.ModelName, data.ModelName);
            Assert.Equal(itemGroup.ItemTypeId, data.ItemTypeId);
            Assert.Equal(itemGroup.Quantity, data.Quantity);
            Assert.Equal(itemGroup.Price, data.Price);
            Assert.Equal(itemGroup.Manufacturer, data.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, data.WarrantyPeriod);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenItemGroupDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemGroupRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_ItemGroupController.RestoreByIdAsync(itemId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int itemId = 1;

            _archive_ItemGroupRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_ItemGroupController.RestoreByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
