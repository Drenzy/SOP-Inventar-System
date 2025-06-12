namespace SOPTests.Controllers
{
    public class StatusControllerTests
    {
        private readonly StatusController _statusController;
        private readonly Mock<IStatusRepository> _statusRepositoryMock = new();
        public StatusControllerTests()
        {
            _statusController = new(_statusRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenStatussExists()
        {
            // Arrange
            List<Status> statuss = new()
            {
                new Status
                {
                    Id = 1,
                    Name = "Virker"
                },
                new Status
                {
                    Id = 2,
                    Name = "Gik stykker"
                }
            };

            _statusRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(statuss);

            // Act
            var result = await _statusController.GetAllASync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<StatusResponse>>(objectResult.Value);
            var data = objectResult.Value as List<StatusResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _statusRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _statusController.GetAllASync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<StatusResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenstatusIsSuccessfullyCreated()
        {
            // Arrange
            StatusRequest statusRequest = new()
            {
                Name = "Virke"
            };

            int statusId = 1;
            Status status = new()
            {
                Id = statusId,
                Name = "Virke"
            };
            _statusRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Status>()))
                .ReturnsAsync(status);

            // Act
            var result = await _statusController.CreateAsync(statusRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as StatusResponse;
            Assert.NotNull(data);
            Assert.Equal(statusId, data.Id);
            Assert.Equal(status.Name, data.Name);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            StatusRequest statusRequest = new()
            {
                Name = "Virke"
            };


            _statusRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Status>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _statusController.CreateAsync(statusRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenStatusExists()
        {
            // Arrange
            int statusId = 1;
            StatusResponse statusResponse = new()
            {
                Id = statusId,
                Name = "Virke"
            };
            Status status = new()
            {
                Id = statusId,
                Name = "Virke"
            };
            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(status);

            // Act
            var result = await _statusController.FindByIdAsync(statusId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as StatusResponse;
            Assert.NotNull(data);
            Assert.Equal(statusId, data.Id);
            Assert.Equal(status.Name, data.Name);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenStatusDoesNotExist()
        {
            // Arrange
            int statusId = 1;

            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _statusController.FindByIdAsync(statusId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int statusId = 1;

            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _statusController.FindByIdAsync(statusId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
