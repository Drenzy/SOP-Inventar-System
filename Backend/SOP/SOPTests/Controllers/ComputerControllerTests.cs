namespace SOPTests.Controllers
{
    public class ComputerControllerTests
    {
        private readonly ComputerController _computerController;
        private readonly Mock<IComputerRepository> _computerRepositoryMock = new();
        public ComputerControllerTests()
        {
            _computerController = new(_computerRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenComputersExists()
        {
            //Arrange
            List<Computer> computers = new()
            {
                new Computer
                {
                    Id = 1,
                },
                new Computer
                {
                    Id = 2
                }
            };

            _computerRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(computers);

            //Act
            var result = await _computerController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ComputerResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ComputerResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _computerRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ComputerResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhencomputerIsSuccessfullyCreated()
        {
            //Arrange
            ComputerRequest computerRequest = new()
            {
                Id = 1
            };

            int computerId = 1;
            Computer computer = new()
            {
                Id = computerId
            };
            _computerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Computer>()))
                .ReturnsAsync(computer);

            //Act
            var result = await _computerController.CreateAsync(computerRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerResponse;
            Assert.NotNull(data);
            Assert.Equal(computerId, data.Id);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ComputerRequest computerRequest = new()
            {
                Id = 1
            };


            _computerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Computer>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerController.CreateAsync(computerRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenComputerExists()
        {
            //Arrange
            int computerId = 1;
            ComputerResponse computerResponse = new()
            {
                Id = computerId,
            };
            Computer computer = new()
            {
                Id = computerId
            };
            _computerRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computer);

            //Act
            var result = await _computerController.FindByIdAsync(computerId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerResponse;
            Assert.NotNull(data);
            Assert.Equal(computerId, data.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenComputerDoesNotExist()
        {
            //Arrange
            int computerId = 1;

            _computerRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _computerController.FindByIdAsync(computerId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int computerId = 1;

            _computerRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerController.FindByIdAsync(computerId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenComputerIsDeleted()
        {
            //Arrange
            int computerId = 1;

            Computer computer = new()
            {
                Id = computerId
            };
            _computerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computer);

            //Act
            var result = await _computerController.DeleteByIdAsync(computerId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerResponse;
            Assert.NotNull(data);
            Assert.Equal(computerId, data.Id);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenComputerDoesNotExist()
        {
            //Arrange
            int computerId = 1;

            _computerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _computerController.DeleteByIdAsync(computerId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int computerId = 1;
            _computerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerController.DeleteByIdAsync(computerId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
