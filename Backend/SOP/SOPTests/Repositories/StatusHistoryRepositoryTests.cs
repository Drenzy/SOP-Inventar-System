namespace SOPTests.Repositories
{
    public class StatusHistoryRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly StatusHistoryRepository _statusHistoryRepository;
        public StatusHistoryRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "StatusHistoryRepositoryTests")
                .Options;

            _context = new(_optinons);

            _statusHistoryRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfStatusHistorys_WhenStatusHistoryExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "1345re2345"
            };

            _context.Item.Add(item);

            Status status = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status);

            StatusHistory statusHistory1 = new()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            _context.StatusHistory.Add(statusHistory1);

            StatusHistory statusHistory2 = new()
            {
                Id = 2,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            _context.StatusHistory.Add(statusHistory2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusHistoryRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StatusHistory>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(statusHistory1.Id, result[0].Id);
            Assert.Equal(statusHistory1.ItemId, result[0].ItemId);
            Assert.Equal(statusHistory1.StatusId, result[0].StatusId);
            Assert.Equal(statusHistory1.StatusUpdateDate, result[0].StatusUpdateDate);
            Assert.Equal(statusHistory1.Note, result[0].Note);

            Assert.Equal(statusHistory2.Id, result[1].Id);
            Assert.Equal(statusHistory2.ItemId, result[1].ItemId);
            Assert.Equal(statusHistory2.StatusId, result[1].StatusId);
            Assert.Equal(statusHistory2.StatusUpdateDate, result[1].StatusUpdateDate);
            Assert.Equal(statusHistory2.Note, result[1].Note);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfStatusHistorys_WhenNoStatusHistoryExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusHistoryRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StatusHistory>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToStatusHistory_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "1345re2345"
            };

            _context.Item.Add(item);

            Status status = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status);

            await _context.SaveChangesAsync();

            StatusHistory statusHistory = new()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            // Act
            var result = await _statusHistoryRepository.CreateAsync(statusHistory);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<StatusHistory>(result);
            Assert.Equal(expectedId, result?.Id);
            Assert.Equal(statusHistory.ItemId, result?.ItemId);
            Assert.Equal(statusHistory.StatusId, result?.StatusId);
            Assert.Equal(statusHistory.StatusUpdateDate, result?.StatusUpdateDate);
            Assert.Equal(statusHistory.Note, result?.Note);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewstatusHistory_WhenStatusHistoryIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            StatusHistory statusHistory = new()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            await _statusHistoryRepository.CreateAsync(statusHistory);

            // Act
            async Task action() => await _statusHistoryRepository.CreateAsync(statusHistory);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusHistory_WhenStatusHistoryExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int statusHistoryId = 1;

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "1345re2345"
            };

            _context.Item.Add(item);

            Status status = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status);

            StatusHistory statusHistory = new()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            _context.StatusHistory.Add(statusHistory);

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusHistoryRepository.FindByIdAsync(statusHistoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(statusHistoryId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenStatusHistoryDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusHistoryId = 1;

            // Act
            var result = await _statusHistoryRepository.FindByIdAsync(statusHistoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedStatusHistory_WhenStatusHistoryExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int statusHistoryId = 1;

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "1345re2345"
            };

            _context.Item.Add(item);

            Status status = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status);

            StatusHistory statusHistory = new()
            {
                Id = 1,
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Ny"
            };

            _context.StatusHistory.Add(statusHistory);

            await _context.SaveChangesAsync();

            StatusHistory updatestatusHistory = new()
            {
                ItemId = 1,
                StatusId = 1,
                StatusUpdateDate = DateTime.Now,
                Note = "Gamle"
            };

            // Act
            var result = await _statusHistoryRepository.UpdateByIdAsync(statusHistoryId, updatestatusHistory);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<StatusHistory>(result);
            Assert.Equal(updatestatusHistory.ItemId, result.ItemId);
            Assert.Equal(updatestatusHistory.StatusId, result.StatusId);
            Assert.Equal(updatestatusHistory.StatusUpdateDate, result.StatusUpdateDate);
            Assert.Equal(updatestatusHistory.Note, result.Note);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenStatusHistoryDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusHistoryId = 1;

            StatusHistory updatestatusHistory = new()
            {
                ItemId = 2,
                StatusId = 3,
                StatusUpdateDate = new DateTime(2024, 10, 28),
                Note = "Gamle"
            };

            // Act
            var result = await _statusHistoryRepository.UpdateByIdAsync(statusHistoryId, updatestatusHistory);

            // Assert
            Assert.Null(result);
        }
    }
}
