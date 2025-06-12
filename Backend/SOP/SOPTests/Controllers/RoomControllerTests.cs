namespace SOPTests.Controllers
{
    public class RoomControllerTests
    {
        private readonly RoomController _roomController;

        private readonly Mock<IRoomRepository> _roomRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public RoomControllerTests(ITestOutputHelper testOutputHelper)
        {
            _roomController = new(_roomRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenRoomsExists()
        {
            // Arrange
            List<Room> rooms = new()
            {
                new Room
                {
                    Id = 1,
                    BuildingId = 1,
                    RoomNumber = 1,
                },
                new Room
                {
                    Id = 1,
                    BuildingId = 1,
                    RoomNumber = 1,
                },
            };

            _roomRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(rooms);

            // Act
            var result = await _roomController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<RoomResponse>>(objectResult.Value);

            var data = objectResult.Value as List<RoomResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _roomRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roomController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var data = objectResult.Value as List<RoomResponse>; 
            Assert.Null(data);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenRoomIsSuccessfullyCreated()
        {
            // Arrange
            RoomRequest roomRequest = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            int roomId = 1;
            Room room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
            };
            _roomRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Room>()))
                .ReturnsAsync(room);

            // Act
            var result = await _roomController.CreateAsync(roomRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RoomResponse;

            Assert.NotNull(data);
            Assert.Equal(roomId, data.Id);
            Assert.Equal(room.RoomNumber, data.RoomNumber);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            RoomRequest roomRequest = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            _roomRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Room>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roomController.CreateAsync(roomRequest);

            // Assert 
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Find by id async should return status code 200 when room exists
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenRoomExists()
        {
            // Arrange
            int roomId = 1;
            RoomResponse roomResponse = new()
            {
                Id = roomId,
                BuildingId = 1,
                Building = new BuildingRoomResponse()
                {
                    BuildingName = "Test",
                    Id = roomId,
                    ZipCode = 2650
                },
                RoomNumber = 1,
            };

            Room room = new()
            {
                Id = roomId,
                BuildingId = 1,
                RoomNumber = 1,
                Building = new Building
                {
                    Id = 1,
                    BuildingName = "",
                    ZipCode = 2750,
                },

                Items = new List<Item>
                {
                    new Item
                    {
                        Id=1,
                        RoomId = 1,
                        SerialNumber = "33",
                        ItemGroupId = 1,
                    }
                },
            };

            _roomRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(room);

            // Act
            var result = await _roomController.FindByIdAsync(roomId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RoomResponse;

            Assert.NotNull(data);
            Assert.Equal(roomId, data.Id);
            Assert.Equal(room.RoomNumber, data.RoomNumber);

            _testOutputHelper.WriteLine(room.RoomNumber.ToString());
            _testOutputHelper.WriteLine(room.Items[0].SerialNumber);
        }

        // Find by id async should return status code 404 when room does not exist.
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenRoomDoesNotExist()
        {
            // Arrange
            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _roomController.FindByIdAsync(roomId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Find by id async should return status code 500 when exception is raised
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roomController.FindByIdAsync(roomId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        // Update by id async should return status code 200 when room is updated.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenRoomIsUpdated()
        {
            // Arrange 
            RoomRequest roomRequest = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            int roomId = 1;

            Room room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
                Building = new Building
                {
                    Id = 1,
                    BuildingName = "",
                    ZipCode = 2750,
                    Address = new Address
                    {
                        ZipCode = 2750,
                        City = "Ballerup",
                        Region = "Sjælland",
                        Road = "Telegrafvej 9",
                    },
                },
                Items = new List<Item>
                {
                    new Item
                    {
                        Id=1,
                        RoomId = 1,
                        SerialNumber = "33",
                        ItemGroupId = 1,
                    }
                },
            };

            _roomRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Room>()))
                .ReturnsAsync(room);

            // Act
            var result = await _roomController.UpdateByIdAsync(roomId, roomRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RoomResponse;

            Assert.NotNull(data);
            Assert.Equal(roomRequest.RoomNumber, data.RoomNumber);
        }


        // Update by id async should return status code 404 when room does not exist.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenRoomDoesNotExist()
        {
            // Arrange
            RoomRequest roomRequest = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Room>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _roomController.UpdateByIdAsync(roomId, roomRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Update by id async should return status code 500 when exception is raised
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            RoomRequest roomRequest = new()
            {
                BuildingId = 1,
                RoomNumber = 1,
            };

            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Room>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roomController.UpdateByIdAsync(roomId, roomRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        // Delete by id async should return status code 200 when room is deleted.
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenRoomIsDeleted()
        {
            // Arrange 
            int roomId = 1;

            Room room = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1,
                Building = new Building
                {
                    Id = 1,
                    BuildingName = "",
                    ZipCode = 2750,
                    Address = new Address
                    {
                        ZipCode = 2750,
                        City = "Ballerup",
                        Region = "Sjælland",
                        Road = "Telegrafvej 9",
                    },
                },
                Items = new List<Item>
                {
                    new Item
                    {
                        Id=1,
                        RoomId = 1,
                        SerialNumber = "33",
                        ItemGroupId = 1,
                    }
                },
            };

            _roomRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(room);

            // Act
            var result = await _roomController.DeleteByIdAsync(roomId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RoomResponse;

            Assert.NotNull(data);
            Assert.Equal(room.Id, data.Id);
            Assert.Equal(room.RoomNumber, data.RoomNumber);
        }


        // Delete by id async should return status code 404 when room does not exist.
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenRoomDoesNotExist()
        {
            // Arrange
            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _roomController.DeleteByIdAsync(roomId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Delete by id async should return status code 500 when exception is raised
        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int roomId = 1;

            _roomRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roomController.DeleteByIdAsync(roomId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
