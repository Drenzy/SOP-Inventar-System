namespace SOPTests.Controllers
{
    public class BuildingControllerTests
    {
        private readonly BuildingController _buildingController;

        private readonly Mock<IBuildingRepository> _buildingRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public BuildingControllerTests(ITestOutputHelper testOutputHelper)
        {
            _buildingController = new(_buildingRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        // Test: Verify 200 status code when addresses exist in the repository using GetAllAsync
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenBuildingsExists()
        {
            // Arrange
            List<Building> buildings = new()
            {
                new Building
                {
                    Id = 1,
                    BuildingName = "Test",
                    ZipCode = 2650,
                    Address = new Address
                    {
                        ZipCode = 2650,
                        City = "Hvidovre",
                        Region = "Sjælland", 
                        Road = "Stamholmen 193, 215",
                    }
                },
                new Building
                {
                    Id = 2,
                    BuildingName = "Test",
                    ZipCode = 2750,
                    Address = new Address
                    {
                        ZipCode = 2750,
                        City = "Ballerup",
                        Region = "Sjælland",
                        Road = "Telegrafvej 9",
                    }
                },
            };
            _buildingRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(buildings);

            // Act
            var result = await _buildingController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<BuildingResponse>>(objectResult.Value);

            var data = objectResult.Value as List<BuildingResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        // Test: Verify 500 status code when exception is raised using GetAllAsync
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _buildingRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _buildingController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<BuildingResponse>;

            Assert.Null(data);
        }

        // Test: Verify 200 status code when building is successfully created using CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenBuildingIsSuccessfullyCreated()
        {
            // Arrange
            BuildingRequest buildingRequest = new()
            {
                BuildingName = "Test",
                ZipCode = 2650,
            };

            int buildingId = 1;
            Building building = new()
            {
                Id = buildingId,
                BuildingName = "Test",
                ZipCode = 2650,
                Address = new Address
                {
                    ZipCode = 2650,
                    City = "Hvidovre",
                    Region = "Sjælland",
                    Road = "Stamholmen 193, 215",
                }
            };
            _buildingRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Building>()))
                .ReturnsAsync(building);
            // Act
            var result = await _buildingController.CreateAsync(buildingRequest);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as BuildingResponse;

            Assert.NotNull(data);

            Assert.Equal(buildingId, data.Id);
            Assert.Equal(building.BuildingName, data.BuildingName);
            Assert.Equal(building.ZipCode, data.ZipCode);
        }

        // Test: Verify 500 status code when exception is raised using CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            BuildingRequest buildingRequest = new()
            {
                BuildingName = "Test",
                ZipCode = 2650,
            };

            _buildingRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Building>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _buildingController.CreateAsync(buildingRequest);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when building exists using FindByIdAsync
        [Fact]

        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenBuildingExists()
        {
            // Arrange
            int buildingId = 1;
            BuildingResponse buildingResponse = new()
            {
                Id = buildingId,
                BuildingName = "Test",
                ZipCode = 2650,
                BuildingAddress = new BuildingAddressResponse
                {
                    ZipCode = 2650,
                    City = "Hvidovre",
                    Region = "Sjælland",
                    Road = "Stamholmen 193, 215",
                }
            };

            Building building = new()
            {
                Id = buildingId,
                BuildingName = "Test",
                ZipCode = 2650,
                Address = new Address
                {
                    ZipCode = 2650,
                    City = "Hvidovre",
                    Region = "Sjælland",
                    Road = "Stamholmen 193, 215",
                }
            };

            _buildingRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(building);

            // Act
            var result = await _buildingController.FindByIdAsync(buildingId);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as BuildingResponse;

            Assert.NotNull(data);

            Assert.Equal(buildingId, data.Id);
            Assert.Equal(building.BuildingName, data.BuildingName);
            Assert.Equal(building.ZipCode, data.ZipCode);

            _testOutputHelper.WriteLine(building.ZipCode.ToString());
            _testOutputHelper.WriteLine(building.Address.ZipCode.ToString());
        }

        // Test: Verify 404 status code when building does not exist using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenBuildingDoesNotExist()
        {
            // Arrange
            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _buildingController.FindByIdAsync(buildingId);

            // Assert
            var objectResult = result as NotFoundResult;
            
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _buildingController.FindByIdAsync(buildingId);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when building is updated using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenBuildingIsUpdated()
        {
            // Arrange
            BuildingRequest buildingRequest = new()
            {
                BuildingName = "Test",
                ZipCode = 2650,
            };

            int buildingId = 1;

            Building building = new()
            {
                Id = buildingId,
                BuildingName = "Test",
                ZipCode = 2650,
                Address = new Address
                {
                    ZipCode = 2650,
                    City = "Hvidovre",
                    Region = "Sjælland",
                    Road = "Stamholmen 193, 215",
                }
            };

            _buildingRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Building>()))
                .ReturnsAsync(building);

            // Act
            var result = await _buildingController.UpdateByIdAsync(buildingId, buildingRequest);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as BuildingResponse;

            Assert.NotNull(data);

            Assert.Equal(buildingRequest.BuildingName, data.BuildingName);
            Assert.Equal(buildingRequest.ZipCode, data.ZipCode);
        }

        // Test: Verify 404 status code when building does not exist using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenBuildingDoesNotExist()
        {
            // Arrange

            BuildingRequest buildingRequest = new()
            {
                BuildingName = "Test",
                ZipCode = 2650,
            };

            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Building>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _buildingController.UpdateByIdAsync(buildingId, buildingRequest);

            // Assert
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            BuildingRequest buildingRequest = new()
            {
                BuildingName = "Test",
                ZipCode = 2650,
            };

            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Building>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _buildingController.UpdateByIdAsync(buildingId, buildingRequest);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when building is deleted using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenBuildingIsDeleted()
        {
            // Arrange: Setup mock data for Building 
            int buildingId = 1;

            Building building = new()
            {
                Id = buildingId,
                BuildingName = "Test",
                ZipCode = 2650,
                Address = new Address
                {
                    ZipCode = 2650,
                    City = "Hvidovre",
                    Region = "Sjælland",
                    Road = "Stamholmen 193, 215",
                }
            };

            _buildingRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(building);

            // Act
            var result = await _buildingController.DeleteByIdAsync(buildingId);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as BuildingResponse;

            Assert.NotNull(data);

            Assert.Equal(building.Id, data.Id);
            Assert.Equal(building.BuildingName, data.BuildingName);
            Assert.Equal(building.ZipCode, data.ZipCode);
        }

        // Test: Verify 404 status code when building does not exist using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenBuildingDoesNotExist()
        {
            // Arrange
            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _buildingController.DeleteByIdAsync(buildingId);

            // Assert
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            int buildingId = 1;

            _buildingRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _buildingController.DeleteByIdAsync(buildingId);

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
