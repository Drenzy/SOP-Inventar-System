using SOP.Entities;

namespace SOPTests.Archive.Repositories
{
    public class Archive_RequestRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_RequestRepository _archive_RequestRepository;
        public Archive_RequestRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_RequestRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_RequestRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_Requests_WhenArchive_RequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_Request.Add(new Archive_Request
            {
                Id = 1,
                DeleteTime = new DateTime(2025, 1, 1),
                Date = new DateTime(2088, 1, 1),
                Message = "Test Message",
                RecipientEmail = "eee@gmail.com",
                UserId = 1,
                Status = "Test Status",
                Item = "Test archive_Request",
                ArchiveNote = "Test Archive Note"
            });

            _context.Archive_Request.Add(new Archive_Request
            {
                Id = 2,
                DeleteTime = new DateTime(2025, 1, 1),
                Date = new DateTime(2088, 1, 1),
                Message = "Test Message",
                RecipientEmail = "eee@gmail.com",
                UserId = 1,
                Status = "Test Status",
                Item = "Test archive_Request",
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_RequestRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_Request>>(result);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_Requests_WhenNoArchive_RequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_RequestRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_Request>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnArchive_Request_WhenArchive_RequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_RequestId = 1;

            _context.Archive_Request.Add(new Archive_Request
            {
                Id = archive_RequestId,
                DeleteTime = new DateTime(2025, 1, 1),
                Date = new DateTime(2088, 1, 1),
                Message = "Test Message",
                RecipientEmail = "eee@gmail.com",
                UserId = 1,
                Status = "Test Status",
                Item = "Test archive_Request",
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_RequestRepository.FindByIdAsync(archive_RequestId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(archive_RequestId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_RequestDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_RequestId = 1;

            // Act
            var result = await _archive_RequestRepository.FindByIdAsync(archive_RequestId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnRequest_WhenRequestIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int requestId = 1;

            Archive_Request request = new()
            {
                Id = 1,
                DeleteTime = new DateTime(2025, 1, 1),
                Date = new DateTime(2088, 1, 1),
                Message = "Test Message",
                RecipientEmail = "eee@gmail.com",
                UserId = 1,
                Status = "Test Status",
                Item = "Test request",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_Request.Add(request);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_RequestRepository.DeleteByIdAsync(requestId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Request>(result);

            Assert.Equal(requestId, result.Id);

            Assert.Equal(request.DeleteTime, result.DeleteTime);
            Assert.Equal(request.Date, result.Date);
            Assert.Equal(request.Message, result.Message);
            Assert.Equal(request.RecipientEmail, result.RecipientEmail);
            Assert.Equal(request.UserId, result.UserId);
            Assert.Equal(request.Status, result.Status);
            Assert.Equal(request.Item, result.Item);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int requestId = 1;

            // Act
            var result = await _archive_RequestRepository.DeleteByIdAsync(requestId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnItem_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();

            int requestId = 1;

            _context.User.Add(new User
            {
                Id = 1,
                Email = "AAAA@gmail.com",
                Name = "Arthur Augustus Adamson Andrews",
                Password = "password",
                RoleId = 5,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
            });

            await _context.SaveChangesAsync();

            Archive_Request request = new()
            {
                Id = requestId,
                DeleteTime = new DateTime(2025, 1, 1),
                Date = new DateTime(2088, 1, 1),
                Message = "Test Message",
                RecipientEmail = "eee@gmail.com",
                UserId = 1,
                Status = "Test Status",
                Item = "Test request",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_Request.Add(request);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_RequestRepository.RestoreByIdAsync(requestId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Request>(result);

            Assert.Equal(requestId, result.Id);

            Assert.Equal(request.Date, result.Date);
            Assert.Equal(request.Message, result.Message);
            Assert.Equal(request.RecipientEmail, result.RecipientEmail);

            var itemInDatabase = await _context.Request.FindAsync(requestId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_Request.FindAsync(requestId);
            Assert.Null(archiveItemInDatabase);
        }


        [Fact]
        public async void RestoreByIdAsync_ShouldReturnNull_WhenArchive_RequestDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_RequestId = 1;

            // Act
            var result = await _archive_RequestRepository.RestoreByIdAsync(archive_RequestId);

            // Assert
            Assert.Null(result);
        }
    }
}
