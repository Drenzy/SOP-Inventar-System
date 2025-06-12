using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOPTests.Repositories
{
    public class StatusRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly StatusRepository _statusRepository;
        public StatusRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "StatusRepositoryTests")
                .Options;

            _context = new(_optinons);

            _statusRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfStatuss_WhenStatusExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Status status1 = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status1);

            Status status2 = new()
            {
                Id = 2,
                Name = "Virker"
            };

            _context.Status.Add(status2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Status>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfStatuss_WhenNoStatusExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Status>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToStatus_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Status status = new()
            {
                Name = "Virke"
            };

            // Act
            var result = await _statusRepository.CreateAsync(status);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Status>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewstatus_WhenStatusIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Status status = new()
            {
                Name = "Virke"
            };

            await _statusRepository.CreateAsync(status);

            // Act
            async Task action() => await _statusRepository.CreateAsync(status);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatus_WhenStatusExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusId = 1;

            Status status = new()
            {
                Id = 1,
                Name = "Virker"
            };

            _context.Status.Add(status);

            await _context.SaveChangesAsync();

            // Act
            var result = await _statusRepository.FindByIdAsync(statusId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(statusId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenStatusDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusId = 1;

            // Act
            var result = await _statusRepository.FindByIdAsync(statusId);

            // Assert
            Assert.Null(result);
        }

    }
}
