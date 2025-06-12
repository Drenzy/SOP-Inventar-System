using SOP.Entities;

namespace SOPTests.Archive.Repositories
{
    public class Archive_UserRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_UserRepository _archive_UserRepository;
        public Archive_UserRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_UserRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_UserRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_Users_WhenArchive_UserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_User.Add(new Archive_User
            {
                Id = 1,
                DeleteTime = new DateTime(2021, 1, 1),
                Email = "pkg.thomsen@gmail.com",
                Name = "Peter Thomsen",
                Password = "password",
                RoleId = 1,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
                ArchiveNote = "Test Archive Note"
            });

            _context.Archive_User.Add(new Archive_User
            {
                Id = 2,
                DeleteTime = new DateTime(2021, 1, 1),
                Email = "AAAA@gmail.com",
                Name = "Arthur Augustus Adamson Andrews",
                Password = "password",
                RoleId = 5,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_UserRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_User>>(result);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_Users_WhenNoArchive_UserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_UserRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_User>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnArchive_User_WhenArchive_UserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_UserId = 1;

            _context.Archive_User.Add(new Archive_User
            {
                Id = archive_UserId,
                DeleteTime = new DateTime(2021, 1, 1),
                Email = "AAAA@gmail.com",
                Name = "Arthur Augustus Adamson Andrews",
                Password = "password",
                RoleId = 5,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_UserRepository.FindByIdAsync(archive_UserId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(archive_UserId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_UserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_UserId = 1;

            // Act
            var result = await _archive_UserRepository.FindByIdAsync(archive_UserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnUser_WhenUserIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;

            Archive_User user = new()
            {
                Id = userId,
                DeleteTime = new DateTime(2021, 1, 1),
                Email = "AAAA@gmail.com",
                Name = "Arthur Augustus Adamson Andrews",
                Password = "password",
                RoleId = 5,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_User.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_UserRepository.DeleteByIdAsync(userId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_User>(result);

            Assert.Equal(userId, result.Id);

            Assert.Equal(user.DeleteTime, result.DeleteTime);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Password, result.Password);
            Assert.Equal(user.RoleId, result.RoleId);
            Assert.Equal(user.TwoFactorAuthentication, result.TwoFactorAuthentication);
            Assert.Equal(user.TwoFactorSecretKey, result.TwoFactorSecretKey);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int userId = 1;

            // Act
            var result = await _archive_UserRepository.DeleteByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnItem_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();

            int userId = 1;

            _context.Role.Add(new Role
            {
                Id = 1,
                Name = "Admin",
                Description = "Administrator"
            });

            await _context.SaveChangesAsync();

            Archive_User user = new()
            {
                Id = userId,
                Email = "AAAA@gmail.com",
                Name = "Arthur Augustus Adamson Andrews",
                Password = "password",
                RoleId = 1,
                TwoFactorAuthentication = false,
                TwoFactorSecretKey = null,
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_User.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_UserRepository.RestoreByIdAsync(userId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<User>(result);

            Assert.Equal(userId, result.Id);

            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Password, result.Password);
            Assert.Equal(user.RoleId, result.RoleId);
            Assert.Equal(user.TwoFactorAuthentication, result.TwoFactorAuthentication);
            Assert.Equal(user.TwoFactorSecretKey, result.TwoFactorSecretKey);

            var itemInDatabase = await _context.User.FindAsync(userId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_User.FindAsync(userId);
            Assert.Null(archiveItemInDatabase);
        }


        [Fact]
        public async void RestoreByIdAsync_ShouldReturnNull_WhenArchive_UserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_UserId = 1;

            // Act
            var result = await _archive_UserRepository.RestoreByIdAsync(archive_UserId);

            // Assert
            Assert.Null(result);
        }
    }
}
