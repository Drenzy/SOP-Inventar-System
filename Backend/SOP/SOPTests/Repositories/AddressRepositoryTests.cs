using SOP.Entities;

namespace SOPTests.Repositories
{
    public class AddressRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;

        private readonly DatabaseContext _context;

        private readonly AddressRepository _addressRepository;

        public AddressRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AddressRepositoryTests")
                .Options;

            _context = new(_options);

            _addressRepository = new(_context);
        }

        // Test: Verify GetAllAsync works and returns a list of Address when Address exists
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfAddresss_WhenAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Address address1 = new()
            {
                ZipCode = 1,
                City = "Greve",
                Region = "Sjæland",
                Road = "Landvej",
            };

            _context.Address.Add(address1);

            Address address2 = new()
            {
                ZipCode = 2,
                City = "Greve",
                Region = "Sjæland",
                Road = "Landvej",
            };

            _context.Address.Add(address2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _addressRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Address>>(result);

            Assert.Equal(2, result.Count);

            Assert.Equal(address1.ZipCode, result[0].ZipCode);
            Assert.Equal(address1.City, result[0].City);
            Assert.Equal(address1.Region, result[0].Region);
            Assert.Equal(address1.Road, result[0].Road);

            Assert.Equal(address2.ZipCode, result[1].ZipCode);
            Assert.Equal(address2.City, result[1].City);
            Assert.Equal(address2.Region, result[1].Region);
            Assert.Equal(address2.Road, result[1].Road);
        }

        // Test: Verify GetAllAsync works and returns an empty list of Address when no Address exists
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfAddresss_WhenNoAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _addressRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Address>>(result);

            Assert.Empty(result);
        }

        // Test: Verify CreateAsync works and returns an Address when Address is added
        [Fact]
        public async void CreateAsync_ShouldAddNewAddressIdToAddress_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int expectedAddressId = 2670;

            Address address = new()
            {
                ZipCode = 2670,
                City = "Greve",
                Region = "Sjæland",
                Road = "Landvej",
            };

            // Act
            var result = await _addressRepository.CreateAsync(address);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Address>(result);

            Assert.Equal(expectedAddressId, result?.ZipCode);
            Assert.Equal(address.ZipCode, result.ZipCode);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.Region, result.Region);
            Assert.Equal(address.Road, result.Road);
        }

        // Test: Verify CreateAsync fails to add an Address when AddressId already exists
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewAddress_WhenAddressAddressIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Address address = new()
            {
                ZipCode = 1,
                City = "",
                Region = "",
                Road = "",
            };

            await _addressRepository.CreateAsync(address);

            // Act
            async Task action() => await _addressRepository.CreateAsync(address);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);

            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test: Verify FindByIdAsync works and returns an Address when Address exists
        [Fact]
        public async void FindByAddressIdAsync_ShouldReturnAddress_WhenAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            Address address = new()
            {
                ZipCode = 1,
                City = "Greve",
                Region = "Sjæland",
                Road = "Landvej",
            };

            _context.Address.Add(address);

            await _context.SaveChangesAsync();

            // Act
            var result = await _addressRepository.FindByIdAsync(addressId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(addressId, result.ZipCode);
            Assert.Equal(address.ZipCode, result.ZipCode);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.Region, result.Region);
            Assert.Equal(address.Road, result.Road);
        }

        // Test: Verify FindByIdAsync works and returns null when Address does not exist
        [Fact]
        public async void FindByAddressIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            // Act
            var result = await _addressRepository.FindByIdAsync(addressId);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify UpdateByIdAsync works and returns an Address when Address exists
        [Fact]
        public async void UpdateByAddressIdAsync_ShouldReturnUpdatedAddress_WhenAddressExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            _context.Address.Add(new()
            {
                ZipCode = addressId,
                City = "Test",
                Region = "Test",
                Road = "Test",
            });

            await _context.SaveChangesAsync();

            Address updateAddress = new()
            {
                City = "NewTest",
                Region = "NewTest",
                Road = "NewTest",
            };

            // Act
            var result = await _addressRepository.UpdateByIdAsync(addressId, updateAddress);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Address>(result);

            Assert.Equal(addressId, result.ZipCode);

            Assert.Equal(updateAddress.City, result.City);
            Assert.Equal(updateAddress.Region, result.Region);
            Assert.Equal(updateAddress.Road, result.Road);
        }

        // Test: Verify UpdateByIdAsync works and returns null when Address does not exist
        [Fact]
        public async void UpdateByAddressIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            Address updateAddress = new()
            {
                City = "",
                Region = "",
                Road = "",
            };

            // Act
            var result = await _addressRepository.UpdateByIdAsync(addressId, updateAddress);

            // Assert
            Assert.Null(result);
        }

        // Test: Verify DeleteByIdAsync works and returns an Address when Address exists
        [Fact]
        public async void DeleteByAddressIdAsync_ShouldReturnAddress_WhenAddressIsDeleted()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            Address address = new()
            {
                ZipCode = 1,
                City = "af",
                Region = "asdf",
                Road = "asdf",
            };

            await _addressRepository.CreateAsync(address);

            // Act
            var result = await _addressRepository.DeleteByIdAsync(addressId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Address>(result);

            Assert.Equal(addressId, result?.ZipCode);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.Region, result.Region);
            Assert.Equal(address.Road, result.Road);
        }

        // Test: Verify DeleteByIdAsync works and returns null when Address does not exist
        [Fact]
        public async void DeleteByAddressIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            // Act
            var result = await _addressRepository.DeleteByIdAsync(addressId);

            // Assert
            Assert.Null(result);
        }
    }
}
