namespace SOPTests.Controllers
{
    public class ComputerPartControllerTests
    {
        private readonly ComputerPartController _computerPartController;
        private readonly Mock<IComputerPartRepository> _computerPartRepositoryMock = new();
        public ComputerPartControllerTests()
        {
            _computerPartController = new(_computerPartRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenComputerPartsExists()
        {
            //Arrange
            List<ComputerPart> computerParts = new()
            {
                new ComputerPart
                {
                    Id = 1,
                    PartGroupId = 1,
                    SerialNumber = "11345134513",
                    ModelNumber = "14123VGE34"
                },
                new ComputerPart
                {
                    Id = 2,
                    PartGroupId = 1,
                    SerialNumber = "546873957",
                    ModelNumber = "3456345GB45"
                }
            };

            _computerPartRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(computerParts);

            //Act
            var result = await _computerPartController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ComputerPartResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ComputerPartResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _computerPartRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerPartController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ComputerPartResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhencomputerPartIsSuccessfullyCreated()
        {
            //Arrange
            ComputerPartRequest computerPartRequest = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            int computerPartId = 1;
            ComputerPart computerPart = new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };
            _computerPartRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ComputerPart>()))
                .ReturnsAsync(computerPart);

            //Act
            var result = await _computerPartController.CreateAsync(computerPartRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computerPartId, data.Id);
            Assert.Equal(computerPart.PartGroupId, data.PartGroupId);
            Assert.Equal(computerPart.SerialNumber, data.SerialNumber);
            Assert.Equal(computerPart.ModelNumber, data.ModelNumber);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ComputerPartRequest computerPartRequest = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };


            _computerPartRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ComputerPart>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerPartController.CreateAsync(computerPartRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenComputerPartExists()
        {
            //Arrange
            int computerPartId = 1;
            ComputerPartResponse computerPartResponse = new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };
            ComputerPart computerPart = new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };
            _computerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computerPart);

            //Act
            var result = await _computerPartController.FindByIdAsync(computerPartId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computerPartId, data.Id);
            Assert.Equal(computerPart.PartGroupId, data.PartGroupId);
            Assert.Equal(computerPart.SerialNumber, data.SerialNumber);
            Assert.Equal(computerPart.ModelNumber, data.ModelNumber);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenComputerPartDoesNotExist()
        {
            //Arrange
            int computerPartId = 1;

            _computerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _computerPartController.FindByIdAsync(computerPartId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int computerPartId = 1;

            _computerPartRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerPartController.FindByIdAsync(computerPartId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenComputerPartIsUpdated()
        {
            //Arrange
            ComputerPartRequest computerPartRequest = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            int computerPartId = 1;
            ComputerPart computerPart = new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };
            _computerPartRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<ComputerPart>()))
                .ReturnsAsync(computerPart);

            //Act
            var result = await _computerPartController.UpdateByIdAsync(computerPartId, computerPartRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computerPart.PartGroupId, data.PartGroupId);
            Assert.Equal(computerPart.SerialNumber, data.SerialNumber);
            Assert.Equal(computerPart.ModelNumber, data.ModelNumber);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenComputerPartDoesNotExist()
        {
            //Arrange
            ComputerPartRequest computerPartRequest = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            int computerPartId = 1;
            _computerPartRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<ComputerPart>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _computerPartController.UpdateByIdAsync(computerPartId, computerPartRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ComputerPartRequest computerPartRequest = new()
            {
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };

            int computerPartId = 1;
            _computerPartRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<ComputerPart>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerPartController.UpdateByIdAsync(computerPartId, computerPartRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenComputerPartIsDeleted()
        {
            //Arrange
            int computerPartId = 1;

            ComputerPart computerPart = new()
            {
                Id = computerPartId,
                PartGroupId = 1,
                SerialNumber = "11345134513",
                ModelNumber = "14123VGE34"
            };
            _computerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(computerPart);

            //Act
            var result = await _computerPartController.DeleteByIdAsync(computerPartId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ComputerPartResponse;
            Assert.NotNull(data);
            Assert.Equal(computerPartId, data.Id);
            Assert.Equal(computerPart.PartGroupId, data.PartGroupId);
            Assert.Equal(computerPart.SerialNumber, data.SerialNumber);
            Assert.Equal(computerPart.ModelNumber, data.ModelNumber);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenComputerPartDoesNotExist()
        {
            //Arrange
            int computerPartId = 1;

            _computerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _computerPartController.DeleteByIdAsync(computerPartId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int computerPartId = 1;
            _computerPartRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _computerPartController.DeleteByIdAsync(computerPartId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
