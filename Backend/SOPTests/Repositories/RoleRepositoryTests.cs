namespace SOPTests.Repositories
{
    public class RoleRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly RoleRepository _roleRepository;
        public RoleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "RoleRepositoryTests")
                .Options;

            _context = new(_options);

            _roleRepository = new(_context);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfBuildings_WhenRoleExists()
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

            await _context.SaveChangesAsync();

            // Act
            var result = await _roleRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Role>>(result);

            Assert.Equal(2, result.Count);

            Assert.Equal(role1.Id, result[0].Id);
            Assert.Equal(role1.Description, result[0].Description);
            Assert.Equal(role1.Name, result[0].Name);

            Assert.Equal(role2.Id, result[1].Id);
            Assert.Equal(role2.Description, result[1].Description);
            Assert.Equal(role2.Name, result[1].Name);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfBuildings_WhenNoRoleExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _roleRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Role>>(result);
            Assert.Empty(result);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByIdAsync_ShouldReturnRole_WhenRoleExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            // Act
            Role role1 = new()
            {
                Id = 1,
                Description = "Test",
                Name = "Test",
            };

            _context.Role.Add(role1);

            await _context.SaveChangesAsync();

            var result = await _roleRepository.FindByIdAsync(Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Id, result.Id);
            Assert.Equal(role1.Description, result.Description);
            Assert.Equal(role1.Name, result.Name);
        }

        // Should return a empty list if database is empty. 
        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int Id = 1;

            // Act
            var result = await _roleRepository.FindByIdAsync(Id);

            // Assert
            Assert.Null(result);
        }

    }
}
