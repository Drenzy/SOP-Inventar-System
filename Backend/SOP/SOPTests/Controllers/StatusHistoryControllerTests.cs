namespace SOPTests.Controllers
{
    public class StatusHistoryControllerTests
    {
        private readonly StatusHistoryController _statusHistoryController;
        private readonly Mock<IStatusHistoryRepository> _statusHistoryRepositoryMock = new();
        public StatusHistoryControllerTests()
        {
            _statusHistoryController = new(_statusHistoryRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenStatusHistorysExists()
        {
            // Arrange
            List<StatusHistory> statusHistory = new()
            {
                new StatusHistory
                {
                    Id = 1,
                    ItemId = 1,
                    StatusId = 1,
                    StatusUpdateDate = new DateTime(2024, 10, 28),
                    Note = "Ny",
                    Item = new Item
                    {
                        Id = 1,
                        RoomId = 1,
                        ItemGroupId = 1,
                        SerialNumber = "1345re2345"
                    },
                    Status = new Status
                    {
                        Id = 1,
                        Name = "Virke"
                    }
                },
                new StatusHistory
                {
                    Id = 2,
                    ItemId = 1,
                    StatusId = 1,
                    StatusUpdateDate = new DateTime(2024, 10, 28),
                    Note = "Ny",
                    Item = new Item
                    {
                        Id = 1,
                        RoomId = 1,
                        ItemGroupId = 1,
                        SerialNumber = "1345re2345"
                    },
                    Status = new Status
                    {
                        Id = 1,
                        Name = "Virke"
                    }
                },
            };

            _statusHistoryRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(statusHistory);

            // Act
            var result = await _statusHistoryController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<StatusHistoryResponse>>(objectResult.Value);

            var data = objectResult.Value as List<StatusHistoryResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _statusHistoryRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            // Act
            var result = await _statusHistoryController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<StatusHistoryResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenStatusHistoryIsSuccessfullyCreated()
        {
            // Arrange
            StatusHistoryRequest statusHistoryRequest = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            int statusHistoryId = 1;
            StatusHistory statusHistory = new()
            {
                Id = statusHistoryId,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny",
                Item = new Item
                {
                    Id = 1,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "1345re2345"
                },
                Status = new Status
                {
                    Id = 1,
                    Name = "Virke"
                }

            };

            _statusHistoryRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<StatusHistory>()))
                .ReturnsAsync(statusHistory);

            // Act
            var result = await _statusHistoryController.CreateAsync(statusHistoryRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as StatusHistoryResponse;
            Assert.NotNull(data);
            Assert.Equal(statusHistoryId, data.Id);
            Assert.Equal(statusHistory.ItemId, data.ItemId);
            Assert.Equal(statusHistory.StatusId, data.StatusId);
            Assert.Equal(statusHistory.StatusUpdateDate, data.StatusUpdateDate);
            Assert.Equal(statusHistory.Note, data.Note);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arange
            StatusHistoryRequest statusHistoryRequest = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };
            _statusHistoryRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<StatusHistory>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _statusHistoryController.CreateAsync(statusHistoryRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenStatusHistoryExists()
        {
            // Arrange
            int statusHistoryId = 1;
            StatusHistoryResponse statusHistoryResponse = new()
            {
                Id = statusHistoryId,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            StatusHistory statusHistory = new()
            {
                Id = statusHistoryId,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny",
                Item = new Item
                {
                    Id = 1,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "1345re2345"
                },
                Status = new Status
                {
                    Id = 1,
                    Name = "Virke"
                }
            };
            _statusHistoryRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(statusHistory);

            // Act
            var result = await _statusHistoryController.FindByIdAsync(statusHistoryId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as StatusHistoryResponse;
            Assert.NotNull(data);
            Assert.Equal(statusHistoryId, data.Id);
            Assert.Equal(statusHistory.ItemId, data.ItemId);
            Assert.Equal(statusHistory.StatusId, data.StatusId);
            Assert.Equal(statusHistory.StatusUpdateDate, data.StatusUpdateDate);
            Assert.Equal(statusHistory.Note, data.Note);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenStatusHistoryDoesNotExist()
        {
            // Arrange
            int statusHistoryId = 1;

            _statusHistoryRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _statusHistoryController.FindByIdAsync(statusHistoryId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int custoemrId = 1;

            _statusHistoryRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _statusHistoryController.FindByIdAsync(custoemrId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenStatusHistoryIsUpdated()
        {
            // Arrange
            StatusHistoryRequest statusHistoryRequest = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            int statusHistoryId = 1;
            StatusHistory statusHistory = new()
            {
                Id = statusHistoryId,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny",
                Item = new Item
                {
                    Id = 1,
                    RoomId = 1,
                    ItemGroupId = 1,
                    SerialNumber = "1345re2345"
                },
                Status = new Status
                {
                    Id = 1,
                    Name = "Virke"
                }
            };
            _statusHistoryRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<StatusHistory>()))
                .ReturnsAsync(statusHistory);


            // Act
            var result = await _statusHistoryController.UpdateByIdAsync(statusHistoryId, statusHistoryRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as StatusHistoryResponse;
            Assert.NotNull(data);
            Assert.Equal(statusHistory.ItemId, data.ItemId);
            Assert.Equal(statusHistory.StatusId, data.StatusId);
            Assert.Equal(statusHistory.StatusUpdateDate, data.StatusUpdateDate);
            Assert.Equal(statusHistory.Note, data.Note);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenStatusHistoryDoesNotExist()
        {
            // Arrange
            StatusHistoryRequest statusHistoryRequest = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            int statusHistoryId = 1;

            _statusHistoryRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _statusHistoryController.UpdateByIdAsync(statusHistoryId, statusHistoryRequest);

            // Assert
            var objetResult = result as NotFoundResult;
            Assert.NotNull(objetResult);
            Assert.Equal(404, objetResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange 
            StatusHistoryRequest statusHistoryRequest = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            int StatusHistoryId = 1;

            _statusHistoryRepositoryMock.Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<StatusHistory>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _statusHistoryController.UpdateByIdAsync(StatusHistoryId, statusHistoryRequest);

            // Assert
            var objetResult = result as ObjectResult;
            Assert.NotNull(objetResult);
            Assert.Equal(500, objetResult.StatusCode);
        }


    }
}
