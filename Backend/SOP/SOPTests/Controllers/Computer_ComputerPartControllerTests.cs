namespace SOPTests.Controllers
{
    public class Computer_ComputerPartControllerTests
    {
        // Moq is a framework used for simulating a database and objects. 
        // Make _computer_ComputerPartController variable. 
        private readonly Computer_ComputerPartController _computer_ComputerPartController;

        // Simulate IComputer_ComputerPartRepository using "Moq"
        private readonly Mock<IComputer_ComputerPartRepository> _computer_ComputerPartRepositoryMock = new();

        // Constructor to initialize controller
        public Computer_ComputerPartControllerTests()
        {
            _computer_ComputerPartController = new(_computer_ComputerPartRepositoryMock.Object);
        }

        // Test: Verify 200 status code when computer parts exist in the repository using GetAllAsync
        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenComputer_ComputerPartsExists()
        {
            // Arrange: Setup mock data for Computer_ComputerPart
            List<Computer_ComputerPart> computer_ComputerParts = new()
            {
                new Computer_ComputerPart
                {
                    Id = 1,
                    ComputerId = 1,
                    ComputerPartId = 1,
                },
                new Computer_ComputerPart
                {
                    Id = 2,
                    ComputerId = 2,
                    ComputerPartId = 2,
                }
            };

            // Mocking the GetAllAsync method to return mock Computer_ComputerParts
            _computer_ComputerPartRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(computer_ComputerParts);

            // Act: Call the controller's GetAllAsync method
            var result = await _computer_ComputerPartController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<Computer_ComputerPartResponse>>(objectResult.Value);
            var data = objectResult.Value as List<Computer_ComputerPartResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        // Test: Verify 500 status code when exception is raised using GetAllAsync
        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _computer_ComputerPartRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _computer_ComputerPartController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Computer_ComputerPartResponse>;
            Assert.Null(data);
        }

        // Test: Verify 200 status code when computer part is successfully created using CreateAsync
        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_Whencomputer_ComputerPartIsSuccessfullyCreated()
        {
            // Arrange: Setup mock data for Computer_ComputerPartRequest
            Computer_ComputerPartRequest computer_ComputerPartRequest = new()
            {
                ComputerId = 1,
                ComputerPartId = 1,
            };

            int computer_ComputerPartId = 1;
            Computer_ComputerPart computer_ComputerPart = new()
            {
                Id = computer_ComputerPartId,
                ComputerId = 1,
                ComputerPartId = 1,
            };

            // Mocking the CreateAsync method to return mock Computer_ComputerPart
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Computer_ComputerPart>()))
                .ReturnsAsync(computer_ComputerPart);

            // Act
            var result = await _computer_ComputerPartController.CreateAsync(computer_ComputerPartRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Computer_ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computer_ComputerPartId, data.Id);
            Assert.Equal(computer_ComputerPart.ComputerId, data.ComputerId);
            Assert.Equal(computer_ComputerPart.ComputerPartId, data.ComputerPartId);
        }

        // Test: Verify 500 status code when exception is raised using CreateAsync
        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange: Setup mock data for Computer_ComputerPartRequest
            Computer_ComputerPartRequest computer_ComputerPartRequest = new()
            {
                ComputerId = 1,
                ComputerPartId = 1,
            };

            // Mocking the CreateAsync method to throw an exception
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Computer_ComputerPart>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _computer_ComputerPartController.CreateAsync(computer_ComputerPartRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when computer part exists in the repository using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenComputer_ComputerPartExists()
        {
            // Arrange: Setup mock data for Computer_ComputerPart
            int computer_ComputerPartId = 1;
            Computer_ComputerPartResponse computer_ComputerPartResponse = new()
            {
                Id = computer_ComputerPartId,
                ComputerId = 1,
                ComputerPartId = 1,
            };
            Computer_ComputerPart computer_ComputerPart = new()
            {
                Id = computer_ComputerPartId,
                ComputerId = 1,
                ComputerPartId = 1,
            };

            // Mocking the FindByIdAsync method to return mock Computer_ComputerPart
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computer_ComputerPart);

            // Act
            var result = await _computer_ComputerPartController.FindByIdAsync(computer_ComputerPartId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Computer_ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computer_ComputerPartId, data.Id);
            Assert.Equal(computer_ComputerPart.ComputerId, data.ComputerId);
            Assert.Equal(computer_ComputerPart.ComputerPartId, data.ComputerPartId);
        }

        // Test: Verify 404 status code when computer part does not exist in the repository using FindByIdAsync
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenComputer_ComputerPartDoesNotExist()
        {
            // Arrange
            int computer_ComputerPartId = 1;

            // Mocking the FindByIdAsync method to return null
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _computer_ComputerPartController.FindByIdAsync(computer_ComputerPartId);

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
            int computer_ComputerPartId = 1;

            // Mocking the FindByIdAsync method to throw an exception
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _computer_ComputerPartController.FindByIdAsync(computer_ComputerPartId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test: Verify 200 status code when computer part is successfully deleted using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenComputer_ComputerPartIsDeleted()
        {
            // Arrange: Setup mock data for Computer_ComputerPart
            int computer_ComputerPartId = 1;
            Computer_ComputerPart computer_ComputerPart = new()
            {
                Id = computer_ComputerPartId,
                ComputerId = 1,
                ComputerPartId = 1,
            };

            // Mocking the DeleteByIdAsync method to return mock Computer_ComputerPart
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computer_ComputerPart);

            // Act
            var result = await _computer_ComputerPartController.DeleteByIdAsync(computer_ComputerPartId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Computer_ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computer_ComputerPartId, data.Id);
            Assert.Equal(computer_ComputerPart.ComputerId, data.ComputerId);
            Assert.Equal(computer_ComputerPart.ComputerPartId, data.ComputerPartId);
        }

        // Test: Verify 404 status code when computer part does not exist in the repository using DeleteByIdAsync
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenComputer_ComputerPartDoesNotExist()
        {
            // Arrang
            int computer_ComputerPartId = 1;

            // Mocking the DeleteByIdAsync method to return null
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _computer_ComputerPartController.DeleteByIdAsync(computer_ComputerPartId);

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
            int computer_ComputerPartId = 1;

            // Mocking the DeleteByIdAsync method to throw an exception
            _computer_ComputerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _computer_ComputerPartController.DeleteByIdAsync(computer_ComputerPartId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}