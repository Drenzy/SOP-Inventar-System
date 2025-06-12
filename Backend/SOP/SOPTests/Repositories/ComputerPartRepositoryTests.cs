namespace SOPTests.Repositories
{
    public class ComputerPartRepositoryTests
    {
        // Database context options for in-memory database
        private readonly DbContextOptions<DatabaseContext> _optinons;

        // Database context
        private readonly DatabaseContext _context;

        // ComputerPart repository
        private readonly ComputerPartRepository _computerPartRepository;

        // Constructor to initialize context and repository
        public ComputerPartRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ComputerPartRepositoryTests")
                .Options;

            _context = new(_optinons);

            _computerPartRepository = new(_context);
        }

        // Test: Verify GetAllAsync works and returns a list of ComputerParts when ComputerPart exists
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfComputerParts_WhenComputerPartExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Add PartGroup to the context
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30,
                PartTypeId = 1,
                PartType = new PartType
                {
                    Id = 1,
                    PartTypeName = "RAM"
                }
            });

            // Add ComputerParts to the context
            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 1,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34",
            });

            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 2,
                PartGroupId = 1,
                SerialNumber = "546873957",
                ModelNumber = "3456345GB45",
            });

            // Save changes to the context
            await _context.SaveChangesAsync();

            // Act
            var result = await _computerPartRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ComputerPart>>(result);
            Assert.Equal(2, result.Count);
        }

        // Test: Verify GetAllAsync works and returns an empty list of ComputerParts when no ComputerPart exists
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfComputerParts_WhenNoComputerPartExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _computerPartRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ComputerPart>>(result);
            Assert.Empty(result);
        }

        // Test: Verify CreateAsync works and returns a ComputerPart when ComputerPart is added
        [Fact]
        public async void CreateAsync_ShouldAddNewIdToComputerPart_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            // Add PartGroup to the context
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30,
                PartType = new PartType
                {
                    Id = expectedId,
                    PartTypeName = "RAM"
                }
            });

            await _context.SaveChangesAsync();

            // Create new ComputerPart
            ComputerPart computerPart = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            // Act
            var result = await _computerPartRepository.CreateAsync(computerPart);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ComputerPart>(result);
            Assert.Equal(expectedId, result?.Id);
        }

        // Test: Verify CreateAsync fails to add a ComputerPart when ComputerPartId already exists
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewcomputerPart_WhenComputerPartIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Create new ComputerPart
            ComputerPart computerPart = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            await _computerPartRepository.CreateAsync(computerPart);

            // Act
            async Task action() => await _computerPartRepository.CreateAsync(computerPart);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test: Verify FindByIdAsync works and returns a ComputerPart when ComputerPart exists
        [Fact]
        public async void FindByIdAsync_ShouldReturnComputerPart_WhenComputerPartExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int computerPartId = 1;

            // Add PartGroup to the context
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30,
                PartType = new PartType
                {
                    Id = 1,
                    PartTypeName = "RAM"
                }
            });

            // Add ComputerPart to the context
            _context.ComputerPart.Add(new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            });

            // Save changes to the context
            await _context.SaveChangesAsync();

            // Act
            var result = await _computerPartRepository.FindByIdAsync(computerPartId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(computerPartId, result.Id);
        }

        // Test: Verify FindByIdAsync works and returns null when ComputerPart does not exist
        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenComputerPartDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerPartId = 1;

            // Act
            var result = await _computerPartRepository.FindByIdAsync(computerPartId);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify UpdateByIdAsync works and returns an updated ComputerPart when ComputerPart exists
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedComputerPart_WhenComputerPartExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int computerPartId = 1;

            // Add PartGroups to the context
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30,
                PartType = new PartType
                {
                    Id = 1,
                    PartTypeName = "RAM"
                }
            });

            _context.PartGroup.Add(new PartGroup
            {
                Id = 2,
                PartTypeId = 1,
                PartName = "ASUS GeForce RTX 4060 DUAL EVO OC",
                Price = 2368.00m,
                Manufacturer = "ASUS",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 10
            });

            // Add ComputerPart to the context
            _context.ComputerPart.Add(new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            });

            // Save changes to the context
            await _context.SaveChangesAsync();

            // Create updated ComputerPart
            ComputerPart updatecomputerPart = new()
            {
                PartGroupId = 2,
                SerialNumber = "65776765465",
                ModelNumber = "5678TZ457"
            };

            // Act
            var result = await _computerPartRepository.UpdateByIdAsync(computerPartId, updatecomputerPart);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ComputerPart>(result);
            Assert.Equal(updatecomputerPart.PartGroupId, result.PartGroupId);
            Assert.Equal(updatecomputerPart.SerialNumber, result.SerialNumber);
            Assert.Equal(updatecomputerPart.ModelNumber, result.ModelNumber);
        }

        // Test: Verify UpdateByIdAsync works and returns null when ComputerPart does not exist
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenComputerPartDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerPartId = 1;

            // Create updated ComputerPart
            ComputerPart updatecomputerPart = new()
            {
                PartGroupId = 2,
                SerialNumber = "65776765465",
                ModelNumber = "5678TZ457"
            };

            // Act
            var result = await _computerPartRepository.UpdateByIdAsync(computerPartId, updatecomputerPart);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify DeleteByIdAsync works and returns a ComputerPart when ComputerPart is deleted
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnComputerPart_WhenComputerPartIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerPartId = 1;

            // Add PartGroup to the context
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30,
                PartType = new PartType
                {
                    Id = 1,
                    PartTypeName = "RAM"
                }
            });

            await _context.SaveChangesAsync();

            // Create new ComputerPart
            ComputerPart computerPart = new()
            {
                PartGroupId = 1,
                SerialNumber = "65776765465",
                ModelNumber = "5678TZ457"
            };

            await _computerPartRepository.CreateAsync(computerPart);

            // Act
            var result = await _computerPartRepository.DeleteByIdAsync(computerPartId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ComputerPart>(result);
            Assert.Equal(computerPartId, result.Id);
            Assert.Equal(computerPart.PartGroupId, result.PartGroupId);
            Assert.Equal(computerPart.SerialNumber, result.SerialNumber);
            Assert.Equal(computerPart.ModelNumber, result.ModelNumber);
        }

        // Test: Verify DeleteByIdAsync works and returns null when ComputerPart does not exist
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenComputerPartDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerPartId = 1;

            // Act
            var result = await _computerPartRepository.DeleteByIdAsync(computerPartId);

            // Assert
            Assert.Null(result);
        }
    }
}

