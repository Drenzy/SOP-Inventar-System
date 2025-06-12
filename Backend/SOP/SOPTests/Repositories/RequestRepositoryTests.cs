namespace SOPTests.Repositories
{
    public class RequestRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly RequestRepository _requestRepository;
        public RequestRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "RequestRepositoryTests")
                .Options;

            _context = new(_options);

            _requestRepository = new(_context);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfRequests_WhenRequestExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            User user1 = new()
            {
                Id = 1,
                RoleId = 1,
                Email = "",
                Name = "Test",
                Password = "Test",
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user1);

            User user2 = new()
            {
                Id = 2,
                RoleId = 1,
                Email = "Test@Test.com",
                Name = "Test",
                Password = "Test",
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user2);

            Request request1 = new()
            {
                Id = 1,
                UserId = 1,
                Message = "Test1",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "",
                Item = "Test1",
                Status = "OK1",
            };

            _context.Request.Add(request1);

            Request request2 = new()
            {
                Id = 2,
                UserId = 2,
                Message = "Test2",
                Date = new DateTime(2099, 12, 1, 1, 1, 1),
                RecipientEmail = "",
                Item = "Test2",
                Status = "OK2",
            };

            _context.Request.Add(request2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _requestRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Request>>(result);

            Assert.Equal(2, result.Count);
            Assert.Equal(request1.Id, result[0].Id);
            Assert.Equal(request1.UserId, result[0].UserId);
            Assert.Equal(request1.Message, result[0].Message);
            Assert.Equal(request1.Date, result[0].Date);
            Assert.Equal(request1.RecipientEmail, result[0].RecipientEmail);
            Assert.Equal(request1.Item, result[0].Item);
            Assert.Equal(request1.Status, result[0].Status);
            Assert.Equal(request2.Id, result[1].Id);
            Assert.Equal(request2.UserId, result[1].UserId);
            Assert.Equal(request2.Message, result[1].Message);
            Assert.Equal(request2.Date, result[1].Date);
            Assert.Equal(request2.RecipientEmail, result[1].RecipientEmail);
            Assert.Equal(request2.Item, result[1].Item);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfRequests_WhenNoRequestExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _requestRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Request>>(result);

            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToRequest_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Request Request = new()
            {
                Id = 1,
                UserId = 1,
                Message = "",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "",
                Item = "Test",
                Status = "OK",
            };

            // Act
            var result = await _requestRepository.CreateAsync(Request);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Request>(result);

            Assert.Equal(expectedId, result?.Id);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewRequest_WhenRequestIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Request Request = new()
            {
                Id = 1,
                UserId = 1,
                Message = "",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "",
                Item = "Test",
                Status = "OK",
            };

            await _requestRepository.CreateAsync(Request);

            // Act
            async Task action() => await _requestRepository.CreateAsync(Request);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);

            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnRequest_WhenRequestExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            // Act
            User user1 = new()
            {
                Id = 1,
                RoleId = 1,
                Email = "",
                Name = "Test",
                Password = "Test",
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user1);

            Request request1 = new()
            {
                Id = 1,
                UserId = 1,
                Message = "Test1",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "",
                Item = "Test1",
                Status = "OK1",
            };

            _context.Request.Add(request1);

            await _context.SaveChangesAsync();

            // Act
            var result = await _requestRepository.FindByIdAsync(Id);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Request>(result);

            Assert.Equal(Id, result.Id);
            Assert.Equal(request1.UserId, result.UserId);
            Assert.Equal(request1.Message, result.Message);
            Assert.Equal(request1.Date, result.Date);
            Assert.Equal(request1.RecipientEmail, result.RecipientEmail);
            Assert.Equal(request1.Item, result.Item);
            Assert.Equal(request1.Status, result.Status);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            // Act
            var result = await _requestRepository.FindByIdAsync(Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedRequest_WhenRequestExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            User user1 = new()
            {
                Id = 1,
                RoleId = 1,
                Email = "Test@Test.com",
                Name = "Test",
                Password = "Test",
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user1);

            Request request1 = new()
            {
                Id = 1,
                UserId = 1,
                Message = "Test1",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "Test@Test",
                Item = "Test1",
                Status = "OK1",
            };

            _context.Request.Add(request1);

            await _context.SaveChangesAsync();

            Request updateRequest = new()
            {
                Id = 1,
                UserId = 1,
                Message = "Test1@test.com",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "Test@request.com",
                Item = "Test",
                Status = "OK",
            };

            // Act
            var result = await _requestRepository.UpdateByIdAsync(Id, updateRequest);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Request>(result);

            Assert.Equal(updateRequest.Message, result.Message);
            Assert.Equal(updateRequest.Id, result.Id);
            Assert.Equal(updateRequest.UserId, result.UserId);
            Assert.Equal(updateRequest.Date, result.Date);
            Assert.Equal(updateRequest.RecipientEmail, result.RecipientEmail);
            Assert.Equal(updateRequest.Item, result.Item);
            Assert.Equal(updateRequest.Status, result.Status);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            Request updateRequest = new()
            {
                UserId = 1,
                Message = "",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "",
            };

            // Act
            //
            var result = await _requestRepository.UpdateByIdAsync(Id, updateRequest);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnArchive_Request_WhenRequestIsArchived()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            string archiveNote = "Test";

            User user1 = new()
            {
                Id = 1,
                RoleId = 1,
                Email = "Test@Test.com",
                Name = "Test",
                Password = "Test",
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user1);

            Request Request = new()
            {
                UserId = 1,
                Message = "Test",
                Date = new DateTime(2002, 12, 29, 23, 59, 59),
                RecipientEmail = "Test",
                Item = "Test",
                Status = "OK",
            };

            await _requestRepository.CreateAsync(Request);

            // Act
            var result = await _requestRepository.ArchiveByIdAsync(Id, archiveNote);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Request>(result);

            Assert.Equal(Id, result?.Id);
            Assert.Equal(Request.UserId, result.UserId);
            Assert.Equal(Request.Message, result.Message);
            Assert.Equal(Request.Date, result.Date);
            Assert.Equal(Request.RecipientEmail, result.RecipientEmail);
            Assert.Equal(Request.Item, result.Item);
            Assert.Equal(Request.Status, result.Status);
            Assert.Equal(archiveNote, result.ArchiveNote);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            string archiveNote = "Test";

            // Act
            var result = await _requestRepository.ArchiveByIdAsync(Id, archiveNote);

            // Assert
            Assert.Null(result);
        }
    }
}
