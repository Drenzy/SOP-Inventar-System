namespace SOPTests.Controllers
{
    public class AddressControllerTests
    {  
        private readonly AddressController _addressController;

        private readonly Mock<IAddressRepository> _addressRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public AddressControllerTests(ITestOutputHelper testOutputHelper)
        {
            _addressController = new(_addressRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        // Test: Verify 200 status code when addresses exist in the repository using GetAllAsync
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenAddressesExists()
        {
            // Arrange
            List<Address> addresses = new()
            {
                new Address
                {
                    ZipCode = 2750,
                    City = "1",
                    Region = "2",
                    Road = "3",
                },
                new Address
                {
                    ZipCode = 2780,
                    City = "1",
                    Region = "2",
                    Road = "3",
                },
            };
            _addressRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(addresses);

            // Act
            var result = await _addressController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<AddressResponse>>(objectResult.Value);

            var data = objectResult.Value as List<AddressResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        // Test: Verify 500 status code when exception is raised using GetAllAsync
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _addressRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _addressController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<AddressResponse>;

            Assert.Null(data);
        }

        // Test: Verify 200 status code when building is successfully created using CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenAddressIsSuccessfullyCreated()
        {
            // Arrange
            AddressRequest addressRequest = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            int addressId = 1;
            Address address = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };
            _addressRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Address>()))
                .ReturnsAsync(address);
            // Act
            var result = await _addressController.CreateAsync(addressRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as AddressResponse;

            Assert.NotNull(data);

            Assert.Equal(address.ZipCode, data.ZipCode);
            Assert.Equal(address.City, data.City);
        }

        // Test: Verify 500 status code when exception is raised using CreateAsync
        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            AddressRequest addressRequest = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            _addressRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Address>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act

            // Create from address. 
            var result = await _addressController.CreateAsync(addressRequest);

            // Assert 

            // Explicit type casting to "ObjectResult"
            // Our GetAllAsync is casted to "ObjectResult"
            // ObjectResult contains both StatusCodes and the Response Body. 
            var objectResult = result as ObjectResult;

            // Check if the object is not null. 
            Assert.NotNull(objectResult);

            // Make sure the status code is 200 (ok)
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when address exists in the repository using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenAddressExists()
        {
            // Arrange
            int addressId = 1;
            AddressResponse addressResponse = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };


            // Add new address
            Address address = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            _addressRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(address);

            // Act

            // FindByIdAsync. 
            var result = await _addressController.FindByIdAsync(addressId);

            // Assert 

            // Explicit type casting to "ObjectResult"
            // Our GetAllAsync is casted to "ObjectResult"
            // ObjectResult contains both StatusCodes and the Response Body. 
            var objectResult = result as ObjectResult;

            // Check if the object is not null. 
            Assert.NotNull(objectResult);

            // Make sure the status code is 200 (ok)
            Assert.Equal(200, objectResult.StatusCode);

            // How many values are there. 
            // Explicit type casting to "AddressResponse"
            var data = objectResult.Value as AddressResponse;

            // Check if there are any AddressResponse objects. 
            Assert.NotNull(data);

            // Check that our model is matching the AddressResponse
            Assert.Equal(address.ZipCode, data.ZipCode);
            Assert.Equal(address.City, data.City);

            _testOutputHelper.WriteLine(address.ZipCode.ToString());
            _testOutputHelper.WriteLine(address.City.ToString());
        }

        // Test: Verify 404 status code when address does not exist in the repository using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenAddressDoesNotExist()
        {
            // Arrange
            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _addressController.FindByIdAsync(addressId);

            // Assert 
            var objectResult = result as NotFoundResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _addressController.FindByIdAsync(addressId);

            // Assert 
            var objectResult = result as ObjectResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when address is successfully updated using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenAddressIsUpdated()
        {
            // Arrange 
            AddressRequest addressRequest = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            int addressId = 1;

            // Add new address
            Address address = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            _addressRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Address>()))
                .ReturnsAsync(address);

            // Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            // Assert 
            var objectResult = result as ObjectResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            // How many values are there. 
            // Explicit type casting to "AddressResponse"
            var data = objectResult.Value as AddressResponse;

            // Check if there are any AddressResponse objects. 
            Assert.NotNull(data);

            // Check that our model is matching the AddressResponse
            Assert.Equal(address.ZipCode, data.ZipCode);
            Assert.Equal(address.City, data.City);
        }

        // Test: Verify 404 status code when address does not exist in the repository using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenAddressDoesNotExist()
        {
            // Arrange

            // Add new address
            AddressRequest addressRequest = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Address>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            // Assert 
            var objectResult = result as NotFoundResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using UpdateByIdAsync
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            // Add new address
            AddressRequest addressRequest = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Address>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            // Assert 
            var objectResult = result as ObjectResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when address is successfully deleted using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenAddressIsDeleted()
        {
            // Arrange 
            int addressId = 1;

            // Add new address
            Address address = new()
            {
                ZipCode = 2750,
                City = "1",
                Region = "2",
                Road = "3",
            };

            _addressRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            // Act
            var result = await _addressController.DeleteByIdAsync(addressId);

            // Assert 
            var objectResult = result as ObjectResult;
            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            // How many values are there. 
            // Explicit type casting to "AddressResponse"
            var data = objectResult.Value as AddressResponse;

            // Check if there are any AddressResponse objects. 
            Assert.NotNull(data);

            // Check that our model is matching the AddressResponse
            Assert.Equal(address.ZipCode, data.ZipCode);
            Assert.Equal(address.City, data.City);
        }

        // Test: Verify 404 status code when address does not exist in the repository using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenAddressDoesNotExist()
        {
            // Arrange

            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _addressController.DeleteByIdAsync(addressId);

            // Assert 
            var objectResult = result as NotFoundResult;

            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Test: Verify 500 status code when exception is raised using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            int addressId = 1;

            _addressRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _addressController.DeleteByIdAsync(addressId);

            // Assert 
            var objectResult = result as ObjectResult;

            // Check if there are any AddressResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }
    }
}
