using Moq;
using SOP.Encryption;

namespace SOPTests.Archive.Controllers
{
    public class Archive_UserControllerTests
    {
        private readonly Archive_UserController _archive_UserController;

        private readonly Mock<IArchive_UserRepository> _archive_UserRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IJwtUtils> _JwtUtilsMock = new();

        public Archive_UserControllerTests(ITestOutputHelper testOutputHelper)
        {
            _archive_UserController = new(_archive_UserRepositoryMock.Object, _JwtUtilsMock.Object);
            _testOutputHelper = testOutputHelper;

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenArchive_UsersExists()
        {
            // Arrange
            List<Archive_User> user = new()
            {
                new Archive_User
                {
                    Id = 1,
                    Email = EncryptionHelper.Encrypt("1@1"),
                    Name = "Test",
                    Password = "1",
                    TwoFactorAuthentication = true,
                    RoleId = 1,
                },
                new Archive_User
                {
                    Id = 2,
                    Email = EncryptionHelper.Encrypt("2@2"),
                    Name = "Test",
                    Password = "2",
                    TwoFactorAuthentication = true,
                    RoleId = 1,
                }
            };
            
            // Arrange
            _archive_UserRepositoryMock
            .Setup(a => a.GetAllAsync())
            .ReturnsAsync(user);

            // Act
            var result = await _archive_UserController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<Archive_UserResponse>>(objectResult.Value);

            var data = objectResult.Value as List<Archive_UserResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _archive_UserRepositoryMock
                // 
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_UserController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Archive_UserResponse>;

            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_UserExists()
        {
            // Arrange
            int userId = 1;
            Archive_UserResponse userResponse = new()
            {
                Id = userId,
                Email = "1@1",
                Name = "Test",
                Password = "1",
                TwoFactorAuthentication = true,
                RoleId = 1,
            };

            Archive_User user = new()
            {
                Id = userId,
                Email = EncryptionHelper.Encrypt("1@1"),
                Name = "Test",
                Password = "1",
                TwoFactorAuthentication = true,
                RoleId = 1,
            };

            _archive_UserRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);

            // Act
            var result = await _archive_UserController.FindByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_UserResponse;

            Assert.NotNull(data);

            Assert.Equal(userId, data.Id);
            Assert.Equal(userResponse.Name, data.Name);
            Assert.Equal(userResponse.Email, data.Email);

            _testOutputHelper.WriteLine(user.Name);
            _testOutputHelper.WriteLine(user.Email);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_UserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _archive_UserRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_UserController.FindByIdAsync(userId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;

            _archive_UserRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_UserController.FindByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenUserIsDeleted()
        {
            // Arrange
            int itemId = 1;
            Archive_User archiveUser = new()
            {
                Id = itemId,
                RoleId = 1,
                Name = "Test User",
                Email = EncryptionHelper.Encrypt("testuser@example.com"),
                Password = "password123",
                TwoFactorAuthentication = true,
                TwoFactorSecretKey = "secretkey123",
                DeleteTime = DateTime.UtcNow
            };
            
            _archive_UserRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveUser); 

            // Act
            var result = await _archive_UserController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_UserResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveUser.RoleId, data.RoleId);
            Assert.Equal(archiveUser.Name, data.Name);
            Assert.Equal(EncryptionHelper.Decrypt(archiveUser.Email), data.Email);
            Assert.Equal(archiveUser.Password, data.Password);
            Assert.Equal(archiveUser.TwoFactorAuthentication, data.TwoFactorAuthentication);
            Assert.Equal(archiveUser.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_UserRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_UserController.DeleteByIdAsync(itemId);

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

            _archive_UserRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_UserController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenArchive_UserIsDeleted()
        {
            // Arrange 
            int userId = 1;

            User user = new()
            {
                Id = userId,
                Email = "",
                Name = "Test",
                Password = "",
                TwoFactorAuthentication = true,
                RoleId = 1,
                Role = new()
                {
                    Id = 1,
                    Description = "Test",
                    Name = "Test",
                },
                Loans = new List<Loan>
                {
                    new Loan
                    {
                        Id=userId,
                        ItemId=userId,
                        LoanDate = DateTime.Now,
                        ReturnDate = DateTime.Now,
                        Item = new()
                        {
                            Id = 1,
                            ItemGroupId = userId,
                            RoomId = userId,
                            SerialNumber = "dd",
                            ItemGroup = new()
                            {
                                Id = 1,
                                ItemTypeId = userId,
                                Manufacturer = "d",
                                ModelName = "Test",
                                Price = 2,
                                Quantity = 2,
                                WarrantyPeriod = "2"
                            }
                        }
                    }
                }
            };

            _archive_UserRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            // Act
            var result = await _archive_UserController.RestoreByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as UserResponse;

            Assert.NotNull(data);

            Assert.Equal(user.Id, data.Id);
            Assert.Equal(user.Name, data.Name);
            Assert.Equal(user.Email, data.Email);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenArchive_UserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _archive_UserRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_UserController.RestoreByIdAsync(userId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;

            _archive_UserRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_UserController.RestoreByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
