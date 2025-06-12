using SOP.Archive.DTOs;

namespace SOPTests.Controllers
{
    public class RequestControllerTests
    {
        private readonly RequestController _requestController;

        private readonly Mock<IRequestRepository> _requestRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public RequestControllerTests(ITestOutputHelper testOutputHelper)
        {
            _requestController = new(_requestRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenRequestsExists()
        {
            // Arrange
            List<Request> requests = new()
            {
                new Request
                {
                    Id = 1,
                    Date = new DateTime(2002, 12, 29, 23, 59, 59),
                    Message = "Test",
                    RecipientEmail = "Testi@gmail.com",
                    UserId = 1,
                },
                new Request
                {
                    Id = 2,
                    Date = new DateTime(2002, 12, 29, 23, 59, 59),
                    Message = "Test",
                    RecipientEmail = "Testi@gmail.com",
                    UserId = 1,
                },
            };
            // 
            _requestRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(requests);

            // Act
            var result = await _requestController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<RequestResponse>>(objectResult.Value);

            var data = objectResult.Value as List<RequestResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _requestRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _requestController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<RequestResponse>;

            Assert.Null(data);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenRequestIsSuccessfullyCreated()
        {
            // Arrange
            RequestRequest requestRequest = new()
            {
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            int requestId = 1;
            Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };
            _requestRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Request>()))
                .ReturnsAsync(request);
            // Act
            var result = await _requestController.CreateAsync(requestRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(requestId, data.Id);
            Assert.Equal(request.Date, data.Date);
            Assert.Equal(request.Message, data.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            RequestRequest requestRequest = new()
            {
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            _requestRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Request>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act

            var result = await _requestController.CreateAsync(requestRequest);

            // Assert 

            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenRequestExists()
        {
            // Arrange
            int requestId = 1;
            RequestResponse requestResponse = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            _requestRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(request);

            // Act
            var result = await _requestController.FindByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(requestId, data.Id);
            Assert.Equal(request.Date, data.Date);
            Assert.Equal(request.Message, data.Message);

            _testOutputHelper.WriteLine(request.Date.ToString());
            _testOutputHelper.WriteLine(request.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenRequestDoesNotExist()
        {
            // Arrange
            int requestId = 1;

            _requestRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _requestController.FindByIdAsync(requestId);

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

            _requestRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _requestController.FindByIdAsync(requestId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenRequestIsUpdated()
        {
            // Arrange 
            RequestRequest requestRequest = new()
            {
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            int requestId = 1;

            Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            _requestRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Request>()))
                .ReturnsAsync(request);

            // Act
            var result = await _requestController.UpdateByIdAsync(requestId, requestRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(requestRequest.Date, data.Date);
            Assert.Equal(requestRequest.Message, data.Message);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenRequestDoesNotExist()
        {
            // Arrange
            RequestRequest requestRequest = new()
            {
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            int requestId = 1;

            _requestRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Request>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _requestController.UpdateByIdAsync(requestId, requestRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            RequestRequest requestRequest = new()
            {
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
            };

            int requestId = 1;

            _requestRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Request>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _requestController.UpdateByIdAsync(requestId, requestRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenRequestIsDeleted()
        {
            // Arrange 
            int requestId = 1;

            Archive_Request request = new()
            {
                Id = requestId,
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                Message = "Test",
                RecipientEmail = "Testi@gmail.com",
                UserId = 1,
                ArchiveNote = "Archive note",
            };

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _requestRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(request);

            // Act
            var result = await _requestController.ArchiveByIdAsync(requestId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_RequestResponse;

            Assert.NotNull(data);

            Assert.Equal(request.Id, data.Id);
            Assert.Equal(request.Date, data.Date);
            Assert.Equal(request.Message, data.Message);
            Assert.Equal(archiveNoteRequest.ArchiveNote, data.ArchiveNote);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenRequestDoesNotExist()
        {
            // Arrange

            int requestId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _requestRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _requestController.ArchiveByIdAsync(requestId, archiveNoteRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            int requestId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "Archive note",
            };

            _requestRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _requestController.ArchiveByIdAsync(requestId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}