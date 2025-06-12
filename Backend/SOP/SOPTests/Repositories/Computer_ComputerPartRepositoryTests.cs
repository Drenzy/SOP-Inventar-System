namespace SOPTests.Repositories
{
    public class Computer_ComputerPartRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Computer_ComputerPartRepository _computer_ComputerPartRepository;
        public Computer_ComputerPartRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Computer_ComputerPartRepositoryTests")
                .Options;

            _context = new(_optinons);

            _computer_ComputerPartRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfComputer_ComputerParts_WhenComputer_ComputerPartExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            // Add new PartGroup
            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30

            });

            // Add new ComputerPart
            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 1,
                PartGroupId = 1,
                SerialNumber = "153145VH34",
                ModelNumber = "234523GR34",

            });

            // Add new Item
            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            // Add new Computer
            _context.Computer.Add(new Computer
            {
                Id = 1,
            });

            // Add new ComputerPart
            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 2,
                PartGroupId = 1,
                SerialNumber = "153145VH34",
                ModelNumber = "234523GR34",

            });

            _context.Item.Add(new Item
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });


            _context.Computer.Add(new Computer
            {
                Id = 2,
            });


            _context.Computer_ComputerPart.Add(new Computer_ComputerPart
            {
                Id = 1,
                ComputerId = 1,
                ComputerPartId = 1,
            });

            _context.Computer_ComputerPart.Add(new Computer_ComputerPart
            {
                Id = 2,
                ComputerId = 2,
                ComputerPartId = 2,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _computer_ComputerPartRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Computer_ComputerPart>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfComputer_ComputerParts_WhenNoComputer_ComputerPartExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _computer_ComputerPartRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Computer_ComputerPart>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToComputer_ComputerPart_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30

            });

            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 1,
                PartGroupId = 1,
                SerialNumber = "153145VH34",
                ModelNumber = "234523GR34",

            });

            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.Computer.Add(new Computer
            {
                Id = 1,
            });

            await _context.SaveChangesAsync();

            Computer_ComputerPart computer_ComputerPart = new()
            {
                Id = 1,
                ComputerId = 1,
                ComputerPartId = 1,
            };

            //Act
            var result = await _computer_ComputerPartRepository.CreateAsync(computer_ComputerPart);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Computer_ComputerPart>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewcomputer_ComputerPart_WhenComputer_ComputerPartIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Computer_ComputerPart computer_ComputerPart = new()
            {
                ComputerId = 1,
                ComputerPartId = 1,
            };

            await _computer_ComputerPartRepository.CreateAsync(computer_ComputerPart);

            //Act
            async Task action() => await _computer_ComputerPartRepository.CreateAsync(computer_ComputerPart);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnComputer_ComputerPart_WhenComputer_ComputerPartExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int computer_ComputerPartId = 1;

            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30

            });

            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 1,
                PartGroupId = 1,
                SerialNumber = "153145VH34",
                ModelNumber = "234523GR34",

            });

            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.Computer.Add(new Computer
            {
                Id = 1,
            });

            _context.Computer_ComputerPart.Add(new()
            {
                Id = computer_ComputerPartId,
                ComputerId = 1,
                ComputerPartId = 1,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _computer_ComputerPartRepository.FindByIdAsync(computer_ComputerPartId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(computer_ComputerPartId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenComputer_ComputerPartDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computer_ComputerPartId = 1;

            //Act
            var result = await _computer_ComputerPartRepository.FindByIdAsync(computer_ComputerPartId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnComputer_ComputerPart_WhenComputer_ComputerPartIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computer_ComputerPartId = 1;

            _context.PartGroup.Add(new PartGroup
            {
                Id = 1,
                PartTypeId = 1,
                PartName = "Corsair Vengeance RGB DDR5-6400",
                Price = 999.00m,
                Manufacturer = "Corsair",
                WarrantyPeriod = "3 år",
                ReleaseDate = new DateTime(2024, 10, 30),
                Quantity = 30

            });

            _context.ComputerPart.Add(new ComputerPart
            {
                Id = 1,
                PartGroupId = 1,
                SerialNumber = "153145VH34",
                ModelNumber = "234523GR34",

            });

            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.Computer.Add(new Computer
            {
                Id = 1,
            });

            await _context.SaveChangesAsync();

            Computer_ComputerPart computer_ComputerPart = new()
            {
                ComputerId = 1,
                ComputerPartId = 1,
            };

            await _computer_ComputerPartRepository.CreateAsync(computer_ComputerPart);

            //Act
            var result = await _computer_ComputerPartRepository.DeleteByIdAsync(computer_ComputerPartId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Computer_ComputerPart>(result);
            Assert.Equal(computer_ComputerPartId, result.Id);
            Assert.Equal(computer_ComputerPart.ComputerId, result.ComputerId);
            Assert.Equal(computer_ComputerPart.ComputerPartId, result.ComputerPartId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenComputer_ComputerPartDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int computer_ComputerPartId = 1;

            //Act
            var result = await _computer_ComputerPartRepository.DeleteByIdAsync(computer_ComputerPartId);

            //Assert
            Assert.Null(result);
        }
    }
}
