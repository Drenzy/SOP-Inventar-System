using SOP.Entities;

namespace SOPTests.Controllers
{
    public class RoleControllerTests
    {
        // Moq is a framework used for simulating a database and objects. 
        // Make _roleController variable. 
        private readonly RoleController _roleController;

        // Simulate IRoleRepository using "Moq"
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public RoleControllerTests(ITestOutputHelper testOutputHelper)
        {
            // _roleController contains our IRoleRepository objects and methods. 
            // _ = prefix for private
            _roleController = new(_roleRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        // Should return a code 200 when roles exists
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenRolesExists()
        {
            // Arrange
            List<Role> roles = new()
            {
                new Role
                {
                    Id = 1,
                    Description = "description",
                    Name = "name",
                },
                new Role
                {
                    Id = 2,
                    Description = "description",
                    Name = "name",
                },
            };

            _roleRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(roles);

            // Act
            var result = await _roleController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<RoleResponse>>(objectResult.Value);

            var data = objectResult.Value as List<RoleResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        // Should return a code 500 when exception is raised
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _roleRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roleController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
            var data = objectResult.Value as List<RoleResponse>;
            Assert.Null(data);
        }


        // Find by id async should return status code 200 when role exists
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenRoleExists()
        {
            // Arrange
            int roleId = 1;
            RoleResponse roleResponse = new()
            {
                Id = roleId,
                Description = "description",
                Name = "name",
            };

            Role role = new()
            {
                Id = roleId,
                Description = "description",
                Name = "name",
            };

            _roleRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(role);

            // Act
            var result = await _roleController.FindByIdAsync(roleId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as RoleResponse;

            Assert.NotNull(data);
            Assert.Equal(roleId, data.Id);
            Assert.Equal(role.Description, data.Description);
            Assert.Equal(role.Name, data.Name);
        }

        // Find by id async should return status code 404 when role does not exist.
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenRoleDoesNotExist()
        {
            // Arrange
            int roleId = 1;

            _roleRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _roleController.FindByIdAsync(roleId);

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
            int roleId = 1;

            _roleRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _roleController.FindByIdAsync(roleId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

    }
}
