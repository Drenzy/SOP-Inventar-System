namespace SOPTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly UserRepository _userRepository;
        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserRepositoryTests")
                .Options;

            _context = new(_options);

            _userRepository = new(_context);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfUsers_WhenUserExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Role role1 = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role1);

            Role role2 = new()
            {
                Id = 2,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role2);

            User user1 = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user1);

            User user2 = new()
            {
                Id = 2,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 2,
                TwoFactorAuthentication = true,
            };

            // Add new User
            _context.User.Add(user2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(user1.Id, result[0].Id);
            Assert.Equal(user1.Email, result[0].Email);
            Assert.Equal(user1.Name, result[0].Name);
            Assert.Equal(user1.Password, result[0].Password);
            Assert.Equal(user1.RoleId, result[0].RoleId);
            Assert.Equal(user1.TwoFactorAuthentication, result[0].TwoFactorAuthentication);
            Assert.Equal(user2.Id, result[1].Id);
            Assert.Equal(user2.Email, result[1].Email);
            Assert.Equal(user2.Name, result[1].Name);
            Assert.Equal(user2.Password, result[1].Password);
            Assert.Equal(user2.RoleId, result[1].RoleId);
            Assert.Equal(user2.TwoFactorAuthentication, result[1].TwoFactorAuthentication);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfUsers_WhenNoUserExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Empty(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void CreateAsync_ShouldAddNewUserIdToUser_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int expectedUserId = 1;

            // Add new User
            User User = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true
            };

            // Act
            var result = await _userRepository.CreateAsync(User);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<User>(result);

            Assert.Equal(expectedUserId, result?.Id);
            Assert.Equal(User.Email, result?.Email);
            Assert.Equal(User.Name, result?.Name);
            Assert.Equal(User.Password, result?.Password);
            Assert.Equal(User.RoleId, result?.RoleId);
            Assert.Equal(User.TwoFactorAuthentication, result?.TwoFactorAuthentication);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewUser_WhenUserUserIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Role role = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role);

            await _context.SaveChangesAsync();

            User User = new()
            {
                Id = 1,
                Email = "d",
                Name = "Test",
                Password = "d",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            await _userRepository.CreateAsync(User);

            // Act
            async Task action() => await _userRepository.CreateAsync(User);

            // Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(action);
            Assert.Contains("The instance of entity type 'User' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values", ex.Message);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByUserIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            Role role = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role);

            await _context.SaveChangesAsync();

            User user = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.FindByIdAsync(UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(UserId, result.Id);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Password, result.Password);
            Assert.Equal(user.RoleId, result.RoleId);
            Assert.Equal(user.TwoFactorAuthentication, result.TwoFactorAuthentication);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            // Act
            var result = await _userRepository.FindByIdAsync(UserId);

            // Assert
            Assert.Null(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdateByUserIdAsync_ShouldReturnUpdatedUser_WhenUserExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            Role role = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role);

            User user = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            User updateUser = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            // Act
            var result = await _userRepository.UpdateByIdAsync(UserId, updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(updateUser.Name, result.Name);
            Assert.Equal(updateUser.Id, result.Id);
            Assert.Equal(updateUser.Email, result.Email);
            Assert.Equal(updateUser.Password, result.Password);
            Assert.Equal(updateUser.RoleId, result.RoleId);
            Assert.Equal(updateUser.TwoFactorAuthentication, result.TwoFactorAuthentication);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdateByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            User updateUser = new()
            {
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true
            };

            // Act
            var result = await _userRepository.UpdateByIdAsync(UserId, updateUser);

            // Assert
            Assert.Null(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdatePasswordByIdAsync_ShouldReturnUpdatedUser_WhenUserExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            Role role = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role);

            User user = new()
            {
                Id = 1,
                Email = "test@test",
                Name = "Test",
                Password = "Password",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            User updateUser = new()
            {
                Id = 1,
                Email = "test@test",
                Name = "Test",
                Password = "Passw0rd",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            // Act
            var result = await _userRepository.UpdatePasswordByIdAsync(UserId, updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(updateUser.Name, result.Name);
            Assert.Equal(updateUser.Id, result.Id);
            Assert.Equal(updateUser.Email, result.Email);
            Assert.Equal(updateUser.Password, result.Password);
            Assert.Equal(updateUser.RoleId, result.RoleId);
            Assert.Equal(updateUser.TwoFactorAuthentication, result.TwoFactorAuthentication);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void UpdatePasswordByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            User updateUser = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "Passw0rd",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            // Act
            var result = await _userRepository.UpdatePasswordByIdAsync(UserId, updateUser);

            // Assert
            Assert.Null(result);
        }

        // Should return Archive_User whwn user is archived.
        [Fact]
        public async void ArchiveByUserIdAsync_ShouldReturnArchive_User_WhenUserIsArchived()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            string ArchiveNote = "Test";

            Role role = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role);

            User user = new()
            {
                Id = 1,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            _context.User.Add(user);

            User User = new()
            {
                Id = 2,
                Email = "",
                Name = "Test",
                Password = "",
                RoleId = 1,
                TwoFactorAuthentication = true,
            };

            await _userRepository.CreateAsync(User);

            // Act
            var result = await _userRepository.ArchiveByIdAsync(UserId, ArchiveNote);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Archive_User>(result);
            Assert.Equal(UserId, result?.Id);
            Assert.Equal(User.Email, result.Email);
            Assert.Equal(User.Name, result.Name);
            Assert.Equal(User.Password, result.Password);
            Assert.Equal(User.RoleId, result.RoleId);
            Assert.Equal(User.TwoFactorAuthentication, result.TwoFactorAuthentication);
        }


        // Should return a empty list if database is empty. 
        [Fact]
        public async void ArchiveByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int UserId = 1;

            string ArchiveNote = "Test";

            // Act
            var result = await _userRepository.ArchiveByIdAsync(UserId, ArchiveNote);

            // Assert
            Assert.Null(result);
        }
    }
}