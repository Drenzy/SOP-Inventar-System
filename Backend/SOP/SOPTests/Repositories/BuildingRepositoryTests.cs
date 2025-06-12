namespace SOPTests.Repositories
{
    public class BuildingRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly BuildingRepository _buildingRepository;

        public BuildingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "BuildingRepositoryTests")
                .Options;

            _context = new(_options);
            _buildingRepository = new(_context);
        }

        // Test: Verify GetAllAsync works and returns a list of Buildings when Building exists
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfBuildings_WhenBuildingExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Address address1 = new()
            {
                ZipCode = 1,
                Road = "TestVej1",
                City = "TestBy1",
                Region = "TestRegion1",
            };

            _context.Address.Add(address1);

            Building building1 = new()
            {
                Id = 1,
                BuildingName = "TEST1",
                ZipCode = 1,
            };

            _context.Building.Add(building1);

            Address address2 = new()
            {
                ZipCode = 2,
                Road = "TestVej2",
                City = "TestBy2",
                Region = "TestRegion2",
            };

            _context.Address.Add(address2);

            Building building2 = new()
            {
                Id = 2,
                BuildingName = "TEST2",
                ZipCode = 2,
            };

            _context.Building.Add(building2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _buildingRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Building>>(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(building1.Id, result[0].Id);
            Assert.Equal(building1.BuildingName, result[0].BuildingName);
            Assert.Equal(building1.ZipCode, result[0].ZipCode);

            Assert.Equal(building2.Id, result[1].Id);
            Assert.Equal(building2.BuildingName, result[1].BuildingName);
            Assert.Equal(building2.ZipCode, result[1].ZipCode);

            Assert.Equal(address1.ZipCode, result[0].Address.ZipCode);
            Assert.Equal(address1.Road, result[0].Address.Road);
            Assert.Equal(address1.City, result[0].Address.City);
            Assert.Equal(address1.Region, result[0].Address.Region);

            Assert.Equal(address2.ZipCode, result[1].Address.ZipCode);
            Assert.Equal(address2.Road, result[1].Address.Road);
            Assert.Equal(address2.City, result[1].Address.City);
            Assert.Equal(address2.Region, result[1].Address.Region);
        }

        // Test: Verify GetAllAsync works and returns an empty list of Buildings when no Building exists
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfBuildings_WhenNoBuildingExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _buildingRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Building>>(result);
            Assert.Empty(result);
        }

        // Test: Verify CreateAsync works and returns a Building when Building is added
        [Fact]
        public async void CreateAsync_ShouldAddNewBuildingIdToBuilding_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int expectedBuildingId = 1;

            Building building = new()
            {
                Id = 1,
                BuildingName = "",
                ZipCode = 1,
            };

            // Act
            var result = await _buildingRepository.CreateAsync(building);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Building>(result);
            Assert.Equal(expectedBuildingId, result?.Id);
            Assert.Equal(building.BuildingName, result?.BuildingName);
            Assert.Equal(building.ZipCode, result?.ZipCode);
        }

        // Test: Verify CreateAsync fails to add a Building when BuildingId already exists
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewBuilding_WhenBuildingIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Building building = new()
            {
                Id = 1,
                BuildingName = "",
                ZipCode = 1,
            };

            await _buildingRepository.CreateAsync(building);

            // Act
            async Task action() => await _buildingRepository.CreateAsync(building);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test: Verify FindByIdAsync works and returns a Building when Building exists
        [Fact]
        public async void FindByBuildingIdAsync_ShouldReturnBuilding_WhenBuildingExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            Address address = new()
            {
                ZipCode = 1,
                Road = "TestVej1",
                City = "TestBy1",
                Region = "TestRegion1",
            };

            _context.Address.Add(address);

            Building building = new()
            {
                Id = 1,
                BuildingName = "Test",
                ZipCode = 1,
            };

            _context.Building.Add(building);

            await _context.SaveChangesAsync();

            // Act
            var result = await _buildingRepository.FindByIdAsync(buildingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(buildingId, result.Id);
            Assert.Equal(building.BuildingName, result.BuildingName);
            Assert.Equal(building.ZipCode, result.ZipCode);

            Assert.Equal(address.ZipCode, result.Address.ZipCode);
            Assert.Equal(address.Road, result.Address.Road);
            Assert.Equal(address.City, result.Address.City);
            Assert.Equal(address.Region, result.Address.Region);
        }

        // Test: Verify FindByIdAsync works and returns null when Building does not exist
        [Fact]
        public async void FindByBuildingIdAsync_ShouldReturnNull_WhenBuildingDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            // Act
            var result = await _buildingRepository.FindByIdAsync(buildingId);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify UpdateByIdAsync works and returns an updated Building when Building exists
        [Fact]
        public async void UpdateByBuildingIdAsync_ShouldReturnUpdatedBuilding_WhenBuildingExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            Address address = new()
            {
                ZipCode = 1,
                Road = "TestVej1",
                City = "TestBy1",
                Region = "TestRegion1",
            };

            _context.Address.Add(address);

            Building building = new()
            {
                Id = 1,
                BuildingName = "Test1",
                ZipCode = 1,
            };

            _context.Building.Add(building);

            await _context.SaveChangesAsync();

            Building updateBuilding = new()
            {
                Id = 1,
                BuildingName = "Test2",
                ZipCode = 1,
            };

            // Act
            var result = await _buildingRepository.UpdateByIdAsync(buildingId, updateBuilding);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Building>(result);
            Assert.Equal(updateBuilding.Id, result.Id);
            Assert.Equal(updateBuilding.BuildingName, result.BuildingName);
            Assert.Equal(updateBuilding.ZipCode, result.ZipCode);

            Assert.Equal(address.ZipCode, result.Address.ZipCode);
            Assert.Equal(address.Road, result.Address.Road);
            Assert.Equal(address.City, result.Address.City);
            Assert.Equal(address.Region, result.Address.Region);
        }

        // Test: Verify UpdateByIdAsync works and returns null when Building does not exist
        [Fact]
        public async void UpdateByBuildingIdAsync_ShouldReturnNull_WhenBuildingDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            Building updateBuilding = new()
            {
                BuildingName = "",
                ZipCode = 1,
            };

            // Act
            var result = await _buildingRepository.UpdateByIdAsync(buildingId, updateBuilding);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify DeleteByIdAsync works and returns null when Building is deleted
        [Fact]
        public async void DeleteByBuildingIdAsync_ShouldNotReturnBuilding_WhenBuildingIsDeleted()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            Building building = new()
            {
                Id = 1,
                BuildingName = "Test",
                ZipCode = 1,
            };

            await _buildingRepository.CreateAsync(building);

            // Act
            var result = await _buildingRepository.DeleteByIdAsync(buildingId);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify DeleteByIdAsync works and returns null when Building does not exist
        [Fact]
        public async void DeleteByBuildingIdAsync_ShouldReturnNull_WhenBuildingDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int buildingId = 1;

            // Act
            var result = await _buildingRepository.DeleteByIdAsync(buildingId);

            // Assert
            Assert.Null(result);
        }
    }
}
