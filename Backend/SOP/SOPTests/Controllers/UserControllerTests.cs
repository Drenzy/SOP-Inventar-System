using Microsoft.AspNetCore.Mvc;
using SOP.Archive.DTOs;
using SOP.Encryption;
using SOP.Entities;

namespace SOPTests.Controllers
{
    public class UserControllerTests
    { 
        private readonly UserController _userController;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IJwtUtils> _JwtUtilsMock = new();

        public UserControllerTests(ITestOutputHelper testOutputHelper)
        {
            _userController = new(_userRepositoryMock.Object, _JwtUtilsMock.Object);
            _testOutputHelper = testOutputHelper;

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenUsersExists()
        {
            // Arrange
            List<User> user = new()
            {
                new User
                {
                    Id = 1,
                    Email = EncryptionHelper.Encrypt("1@1"),
                    Name = "Test",
                    Password = "1",
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
                            Id=1,
                            ItemId=1,
                            LoanDate = DateTime.Now,
                            ReturnDate = DateTime.Now,
                            Item = new()
                            {
                                Id = 1,
                                ItemGroupId = 1,
                                RoomId = 1,
                                SerialNumber = "dd",
                                ItemGroup = new()
                                {
                                    Id = 1,
                                    ItemTypeId = 1,
                                    Manufacturer = "d",
                                    ModelName = "Test",
                                    Price = 2,
                                    Quantity = 2,
                                    WarrantyPeriod = "2"
                                }
                            }
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    Email = EncryptionHelper.Encrypt("2@2"),
                    Name = "Test",
                    Password = "2",
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
                        Id=1,
                        ItemId=1,
                        LoanDate = DateTime.Now,
                        ReturnDate = DateTime.Now,
                        Item = new()
                        {
                            Id = 1,
                            ItemGroupId = 1,
                            RoomId = 1,
                            SerialNumber = "dd",
                            ItemGroup = new()
                            {
                                Id = 1,
                                ItemTypeId = 1,
                                Manufacturer = "d",
                                ModelName = "Test",
                                Price = 2,
                                Quantity = 2,
                                WarrantyPeriod = "2"
                            }
                        }
                    }
                }
                },
            };

            _userRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(user);

            // Act
            var result = await _userController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<UserResponse>>(objectResult.Value);

            var data = objectResult.Value as List<UserResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _userRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _userController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<UserResponse>;

            Assert.Null(data);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            UserRequest userRequest = new()
            {
                Email = "",
                Name = "Test",
                Password = "",
                TwoFactorAuthentication = true,
            };

            int userId = 1;
            User user = new()
            {
                Id = userId,
                Email = "",
                Name = "Test",
                Password = "",
                TwoFactorAuthentication = true,
            };
            _userRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.CreateAsync(userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as UserResponse;
            Assert.NotNull(data);
            Assert.Equal(userId, data.Id);
            Assert.Equal(user.Name, data.Name);
            Assert.Equal(user.Email, data.Email);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            UserRequest userRequest = new()
            {
                Email = "",
                Name = "Test",
                Password = "",
                TwoFactorAuthentication = true,
            };

            _userRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _userController.CreateAsync(userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        // Find by id async should return status code 200 when user exists
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            UserResponse userResponse = new()
            {
                Id = userId,
                Email = "1@1",
                Name = "Test",
                Password = "1",
                TwoFactorAuthentication = true,
                RoleId = 1,
                UserRole = new()
                {
                    Id = 1,
                    Description = "Test",
                    Name = "Test",
                },
                UserLoans = new List<UserLoanResponse>
                {
                    new UserLoanResponse
                    {
                        Id=userId,
                        ItemId=userId,
                        LoanDate = DateTime.Now,
                        ReturnDate = DateTime.Now,
                        UserLoanItem = new()
                        {
                            Id = 1, 
                            ItemGroupId = userId,
                            RoomId = userId,
                            SerialNumber = "dd",
                            UserLoanItemItemGroup = new()
                            {
                                Id = 1,
                                ItemTypeId = userId,
                                Manufacturer = "d",
                                ModelName = "Test",
                                Price = 2,
                                Quantity = 2,
                                WarrantyPeriod = "2",
                            }
                        }
                    }
                }
            };


            User user = new()
            {
                Id = userId,
                Email = EncryptionHelper.Encrypt("1@1"),
                Name = "Test",
                Password = "1@1",
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

            _userRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);

            // Act
            var result = await _userController.FindByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as UserResponse;

            Assert.NotNull(data);
            Assert.Equal(userId, data.Id);
            Assert.Equal(userResponse.Name, data.Name);
            Assert.Equal(userResponse.Email, data.Email);
        }

        // Find by id async should return status code 404 when user does not exist.
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userController.FindByIdAsync(userId);

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
            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _userController.FindByIdAsync(userId);

            // Assert 
            var objectResult = result as ObjectResult;
            // Check if there are any UserResponse objects. 
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        // Update by id async should return status code 200 when user is updated.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenUserIsUpdated()
        {
            // Arrange 
            UserRequest userRequest = new()
            {
                Email = "1@1",
                Name = "Test",
                Password = "1",
                TwoFactorAuthentication = true,
                RoleId = 1,
            };

            int userId = 1;

            User user = new()
            {
                Id = userId,
                Email = EncryptionHelper.Encrypt("1@1"),
                Name = "Test",
                Password = "1",
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

            _userRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.UpdateByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as UserResponse;

            Assert.NotNull(data);
            Assert.Equal(userRequest.Name, data.Name);
            Assert.Equal(userRequest.Email, data.Email);
        }

        // Update by id async should return status code 404 when user does not exist.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            UserRequest userRequest = new()
            {
                Email = "1@1",
                Name = "Test",
                Password = "Passw0rd",
                TwoFactorAuthentication = true,
            };

            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userController.UpdateByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Update by id async should return status code 500 when exception is raised
        [Fact]
        public async Task UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            UserRequest userRequest = new()
            {
                Email = "test@example.com",
                Name = "Test",
                Password = "Passw0rd!0789",
                TwoFactorAuthentication = true,
            };

            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User { Id = userId, Email = "test@example.com", Name = "Test User" });

            _userRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _userController.UpdateByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void UpdatePasswordByIdAsyncAsync_ShouldReturnStatusCode200_WhenUserIsSuccessfullyUpdated()
        {
            // Arrange
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            int userId = 1;

            UserRequest userRequest = new()
            {
                Password = "Passw0rd!123",
            };

            User user = new()
            {
                Id = userId,
                Email = EncryptionHelper.Encrypt("1@1"),
                Name = "Test",
                Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, salt, true, BCrypt.Net.HashType.SHA512),
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


            _userRepositoryMock
                .Setup(x => x.UpdatePasswordByIdAsync(userId, It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.UpdatePasswordByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as UserResponse;

            Assert.NotNull(data);
            Assert.Equal(user.Id, data.Id);
        }


        [Fact]
        public async void UpdatePasswordByIdAsyncAsync_shouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            int userId = 1;

            UserRequest userRequest = new()
            {
                Password = "Passw0rd!123",
            };

            _userRepositoryMock
                .Setup(x => x.UpdatePasswordByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userController.UpdatePasswordByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }
    

        [Fact]
        public async void UpdatePasswordByIdAsyncAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            UserRequest userRequest = new()
            {
                Email = "test@example.com",
                Name = "Test",
                Password = "Passw0rd!0789",
                TwoFactorAuthentication = true,
            };

            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User { Id = userId, Email = "test@example.com", Name = "Test User" });

            _userRepositoryMock
                .Setup(x => x.UpdatePasswordByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _userController.UpdatePasswordByIdAsync(userId, userRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }




        // Delete by id async should return status code 200 when user is deleted.
        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenUserIsDeleted()
        {
            // Arrange 
            int userId = 1;

            Archive_User user = new()
            {
                Id = userId,
                Email = "",
                Name = "Test",
                Password = "",
                TwoFactorAuthentication = true,
                RoleId = 1,
                DeleteTime = DateTime.Now,
                ArchiveNote = "This is an archive note",
            };

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _userRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.ArchiveByIdAsync(userId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_UserResponse;

            Assert.NotNull(data);
            Assert.Equal(user.Id, data.Id);
            Assert.Equal(user.Name, data.Name);
            Assert.Equal(user.Email, data.Email);
            Assert.Equal(user.Password, data.Password);
            Assert.Equal(user.TwoFactorAuthentication, data.TwoFactorAuthentication);
            Assert.Equal(user.RoleId, data.RoleId);
            Assert.Equal(user.DeleteTime, data.DeleteTime);
            Assert.Equal(archiveNoteRequest.ArchiveNote, data.ArchiveNote);
        }

        // Delete by id async should return status code 404 when user does not exist.
        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _userRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userController.ArchiveByIdAsync(userId, archiveNoteRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        // Delete by id async should return status code 500 when exception is raised
        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange

            int userId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _userRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _userController.ArchiveByIdAsync(userId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void SignInAsync_ShouldReturnStatusCode200_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            
            SignInRequest signInRequest = new()
            {
                Password = "Test",
                Email = "Test",
            };

            int userId = 1;
            User user = new()
            {
                Id = userId,
                Name = "Test",
                Password = BCrypt.Net.BCrypt.HashPassword("Test", salt, true, BCrypt.Net.HashType.SHA512),
                Email = "Test",
                RoleId = 1,
                Role = new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Administrator"
                }
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.SignInAsync(signInRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as SignInResponse;
            Assert.NotNull(data);
            Assert.Equal(user.Role, data.Role);
        }

        [Fact]
        public async void SignInAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arange
            SignInRequest signInRequest = new()
            {
                Password = "Test",
                Email = "Test",
            };
            _userRepositoryMock
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _userController.SignInAsync(signInRequest);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

    }
}
