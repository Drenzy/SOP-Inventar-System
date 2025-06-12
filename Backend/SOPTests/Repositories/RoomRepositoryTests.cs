using SOP.Entities;

namespace SOPTests.Repositories
{
    public class RoomRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly RoomRepository _roomRepository;
        public RoomRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "RoomRepositoryTests")
                .Options;

            _context = new(_options);

            _roomRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfRooms_WhenRoomExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Building building = new()
            {
                ZipCode = 1,
                BuildingName = "Test1",
            };

            _context.Building.Add(building);

            Address address = new()
            {
                ZipCode = 1,
                City = "TestCity",
                Region = "TestRegion",
                Road = "TestRoad",
            };

            _context.Address.Add(address);

            Room room1 = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            _context.Room.Add(room1);

            Room room2 = new()
            {
                Id = 2,
                BuildingId = 1,
                RoomNumber = 1,
            };

            _context.Room.Add(room2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _roomRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Room>>(result);

            Assert.Equal(2, result.Count);

            Assert.Equal(room1.Id, result[0].Id);
            Assert.Equal(room1.BuildingId, result[0].BuildingId);
            Assert.Equal(room1.RoomNumber, result[0].RoomNumber);

            Assert.Equal(room2.Id, result[1].Id);
            Assert.Equal(room2.BuildingId, result[1].BuildingId);
            Assert.Equal(room2.RoomNumber, result[1].RoomNumber);

        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfRooms_WhenNoRoomExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _roomRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Room>>(result);
            Assert.Empty(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void CreateAsync_ShouldAddNewRoomIdToRoom_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int expectedRoomId = 1;

            // Add new Room
            Room Room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            // Act
            var result = await _roomRepository.CreateAsync(Room);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
            Assert.Equal(expectedRoomId, result?.RoomNumber);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewRoom_WhenIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Room Room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            await _roomRepository.CreateAsync(Room);

            // Act
            async Task action() => await _roomRepository.CreateAsync(Room);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByRoomIdAsync_ShouldReturnRoom_WhenRoomExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            Building building = new()
            {
                ZipCode = 1,
                BuildingName = "Test1",
            };

            _context.Building.Add(building);

            Address address = new()
            {
                ZipCode = 1,
                City = "TestCity",
                Region = "TestRegion",
                Road = "TestRoad",
            };

            _context.Address.Add(address);

            Room room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            _context.Room.Add(room);

            await _context.SaveChangesAsync();

            // Act
            var result = await _roomRepository.FindByIdAsync(Id);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Room>(result);
            Assert.Equal(Id, result.Id);
            Assert.Equal(room.BuildingId, result.BuildingId);
            Assert.Equal(room.RoomNumber, result.RoomNumber);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByRoomIdAsync_ShouldReturnNull_WhenRoomDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            // Act
            var result = await _roomRepository.FindByIdAsync(Id);

            // Assert
            Assert.Null(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdateByRoomIdAsync_ShouldReturnUpdatedRoom_WhenRoomExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            Building building = new()
            {
                ZipCode = 1,
                BuildingName = "Test1",
            };

            _context.Building.Add(building);

            Address address = new()
            {
                ZipCode = 1,
                City = "TestCity",
                Region = "TestRegion",
                Road = "TestRoad",
            };

            _context.Address.Add(address);

            Room room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            _context.Room.Add(room);

            await _context.SaveChangesAsync();

            Room updateRoom = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            // Act
            var result = await _roomRepository.UpdateByIdAsync(Id, updateRoom);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
            Assert.Equal(Id, result.Id);
            Assert.Equal(updateRoom.BuildingId, result.BuildingId);
            Assert.Equal(updateRoom.RoomNumber, result.RoomNumber);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdateByRoomIdAsync_ShouldReturnNull_WhenRoomDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            Room updateRoom = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            // Act
            var result = await _roomRepository.UpdateByIdAsync(Id, updateRoom);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByRoomIdAsync_ShouldReturnRoom_WhenRoomIsDeleted()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            Building building = new()
            {
                ZipCode = 1,
                BuildingName = "Test1",
            };

            _context.Building.Add(building);

            Address address = new()
            {
                ZipCode = 1,
                City = "TestCity",
                Region = "TestRegion",
                Road = "TestRoad",
            };

            _context.Address.Add(address);

            Room Room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };

            await _roomRepository.CreateAsync(Room);

            // Act
            var result = await _roomRepository.DeleteByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Room>(result);
            Assert.Equal(Id, result?.Id);
            Assert.Equal(Room.BuildingId, result.BuildingId);
            Assert.Equal(Room.RoomNumber, result.RoomNumber);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void DeleteByRoomIdAsync_ShouldReturnNull_WhenRoomDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int Id = 1;

            // Act
            var result = await _roomRepository.DeleteByIdAsync(Id);

            // Assert
            Assert.Null(result);
        }
    }
}
