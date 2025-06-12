namespace SOPTests.Repositories
{
    public class ComputerRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ComputerRepository _computerRepository;
        public ComputerRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ComputerRepositoryTests")
                .Options;

            _context = new(_optinons);

            _computerRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfComputers_WhenComputerExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "356346BR44"
            });

            // Add new entity to the database
            _context.Computer.Add(new Computer
            {
                Id = 1,
            });

            // Add new entity to the database
            _context.Computer.Add(new Computer
            {
                Id = 2,
            });

            // Save the database
            await _context.SaveChangesAsync();

            //Act
            var result = await _computerRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Computer>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfComputers_WhenNoComputerExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _computerRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Computer>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToComputer_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });


            await _context.SaveChangesAsync();

            // Add new entity 
            Computer computer = new()
            {
                Id = 1,
            };

            //Act
            var result = await _computerRepository.CreateAsync(computer);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Computer>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewcomputer_WhenComputerIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            await _context.SaveChangesAsync();

            Computer computer = new()
            {
                Id = 1,
            };

            await _computerRepository.CreateAsync(computer);

            //Act
            async Task action() => await _computerRepository.CreateAsync(computer);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnComputer_WhenComputerExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int computerId = 1;

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            // Add new entity to the database
            _context.Computer.Add(new()
            {
                Id = computerId
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _computerRepository.FindByIdAsync(computerId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(computerId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenComputerDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerId = 1;

            //Act
            var result = await _computerRepository.FindByIdAsync(computerId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnComputer_WhenComputerIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerId = 1;

            // Add new entity to the database
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });
            
            await _context.SaveChangesAsync();

            Computer computer = new()
            {
                Id = 1,
            };

            await _computerRepository.CreateAsync(computer);

            //Act
            var result = await _computerRepository.DeleteByIdAsync(computerId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Computer>(result);
            Assert.Equal(computerId, result.Id);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenComputerDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computerId = 1;

            //Act
            var result = await _computerRepository.DeleteByIdAsync(computerId);

            //Assert
            Assert.Null(result);
        }
    }
}
