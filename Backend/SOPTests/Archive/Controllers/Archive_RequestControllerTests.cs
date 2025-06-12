using SOP.Entities;

namespace SOPTests.Archive.Controllers
{
    public class Archive_RequestControllerTests
    {
        private readonly Archive_RequestController _archive_RequestController;

        private readonly Mock<IArchive_RequestRepository> _archive_RequestRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public Archive_RequestControllerTests(ITestOutputHelper testOutputHelper)
        {
            _archive_RequestController = new(_archive_RequestRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenArchive_RequestsExists()
        {
            // Arrange
            List<Archive_Request> requests = new()
            {
                new Archive_Request
                {
                    Id = 1,
                    Date = new DateTime(2002, 12, 29, 23, 59, 59),
                    Message = "Test",
                    RecipientEmail = "Testi@gmail.com",
                    UserId = 1,
                },
                new Archive_Request
                {
                    Id = 2,
                    Date = new DateTime(2002, 12, 29, 23, 59, 59),
                    Message = "Test",
                    RecipientEmail = "Testi@gmail.com",
                    UserId = 1,
                },
            };
            // 
            _archive_RequestRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(requests);

            // Act
            var result = await _archive_RequestController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<Archive_RequestResponse>>(objectResult.Value);

            var data = objectResult.Value as List<Archive_RequestResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _archive_RequestRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_RequestController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Archive_RequestResponse>;

            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_RequestExists()
        {
            // Arrange
            int requestId = 1;
            Archive_RequestResponse requestResponse = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };


            // Add new request
            Archive_Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            _archive_RequestRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(request);

            // Act
            var result = await _archive_RequestController.FindByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(requestId, data.Id);
            Assert.Equal(request.Date, data.Date);
            Assert.Equal(request.Message, data.Message);

            _testOutputHelper.WriteLine(request.Date.ToString());
            _testOutputHelper.WriteLine(request.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_RequestDoesNotExist()
        {
            // Arrange
            int requestId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_RequestController.FindByIdAsync(requestId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int requestId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_RequestController.FindByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenRequestIsDeleted()
        {
            // Arrange
            int itemId = 1;
            Archive_Request archiveRequest = new()
            {
                Id = itemId,
                UserId = 1,
                RecipientEmail = "recipient@example.com",
                Item = "Test Item",
                Message = "Test Message",
                Date = DateTime.UtcNow.AddDays(-1),
                Status = "Pending",
                DeleteTime = DateTime.UtcNow
            };

            _archive_RequestRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveRequest);

            // Act
            var result = await _archive_RequestController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_RequestResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveRequest.UserId, data.UserId);
            Assert.Equal(archiveRequest.RecipientEmail, data.RecipientEmail);
            Assert.Equal(archiveRequest.Item, data.Item);
            Assert.Equal(archiveRequest.Message, data.Message);
            Assert.Equal(archiveRequest.Date, data.Date);
            Assert.Equal(archiveRequest.Status, data.Status);
            Assert.Equal(archiveRequest.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenRequestDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_RequestController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int itemId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_RequestController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenArchive_Archive_RequestIsDeleted()
        {
            // Arrange 
            int requestId = 1;

            Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            _archive_RequestRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(request);

            // Act
            var result = await _archive_RequestController.RestoreByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(request.Id, data.Id);
            Assert.Equal(request.Date, data.Date);
            Assert.Equal(request.Message, data.Message);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenArchive_RequestDoesNotExist()
        {
            // Arrange

            int requestId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_RequestController.RestoreByIdAsync(requestId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            int requestId = 1;

            _archive_RequestRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_RequestController.RestoreByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}