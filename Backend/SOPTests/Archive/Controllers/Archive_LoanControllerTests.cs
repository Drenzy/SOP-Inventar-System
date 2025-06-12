namespace SOPTests.Archive.Controllers
{
    public class Archive_LoanControllerTests
    {
        private readonly Archive_LoanController _archive_LoanController;

        private readonly Mock<IArchive_LoanRepository> _archive_LoanRepositoryMock = new();

        private readonly ITestOutputHelper _testOutputHelper;

        public Archive_LoanControllerTests(ITestOutputHelper testOutputHelper)
        {
            _archive_LoanController = new(_archive_LoanRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenArchive_LoansExists()
        {
            // Arrange
            List<Archive_Loan> loans = new()
            {
                new Archive_Loan
                {
                    Id = 1,
                    ItemId = 1,
                    UserId = 1,
                    LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                },
                new Archive_Loan
                {
                    Id = 2,
                    ItemId = 1,
                    UserId = 1,
                    LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                },
            };

            _archive_LoanRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(loans);

            // Act

            var result = await _archive_LoanController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);

            Assert.IsType<List<Archive_LoanResponse>>(objectResult.Value);

            var data = objectResult.Value as List<Archive_LoanResponse>;

            Assert.NotNull(data);

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _archive_LoanRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_LoanController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<Archive_LoanResponse>;

            Assert.Null(data);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenArchive_LoanExists()
        {
            // Arrange
            int loanId = 1;
            Archive_LoanResponse loanResponse = new()
            {
                Id = loanId,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            Archive_Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _archive_LoanRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(loan);

            // Act
            var result = await _archive_LoanController.FindByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_LoanResponse;

            Assert.NotNull(data);

            Assert.Equal(loanId, data.Id);
            Assert.Equal(loan.LoanDate, data.LoanDate);
            Assert.Equal(loan.ReturnDate, data.ReturnDate);

            _testOutputHelper.WriteLine(loan.LoanDate.ToString());
            _testOutputHelper.WriteLine(loan.ReturnDate.ToString());
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenArchive_LoanDoesNotExist()
        {
            // Arrange
            int loanId = 1;

            _archive_LoanRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_LoanController.FindByIdAsync(loanId);

            // Assert 
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int loanId = 1;

            _archive_LoanRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_LoanController.FindByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenLoanIsDeleted()
        {
            // Arrange
            int itemId = 1;
            Archive_Loan archiveLoan = new()
            {
                Id = itemId,
                UserId = 1,
                ItemId = 2,
                LoanDate = DateTime.UtcNow.AddMonths(-1),
                ReturnDate = DateTime.UtcNow.AddMonths(1),
                DeleteTime = DateTime.UtcNow
            };

            _archive_LoanRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(archiveLoan);

            // Act
            var result = await _archive_LoanController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_LoanResponse;
            Assert.NotNull(data);
            Assert.Equal(itemId, data.Id);
            Assert.Equal(archiveLoan.UserId, data.UserId);
            Assert.Equal(archiveLoan.ItemId, data.ItemId);
            Assert.Equal(archiveLoan.LoanDate, data.LoanDate);
            Assert.Equal(archiveLoan.ReturnDate, data.ReturnDate);
            Assert.Equal(archiveLoan.DeleteTime, data.DeleteTime);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenLoanDoesNotExist()
        {
            // Arrange
            int itemId = 1;

            _archive_LoanRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_LoanController.DeleteByIdAsync(itemId);

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

            _archive_LoanRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = await _archive_LoanController.DeleteByIdAsync(itemId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode200_WhenLoanIsDeleted()
        {
            // Arrange 
            int loanId = 1;

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _archive_LoanRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(loan);

            // Act
            var result = await _archive_LoanController.RestoreByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoanResponse;

            Assert.NotNull(data);

            Assert.Equal(loan.Id, data.Id);
            Assert.Equal(loan.LoanDate, data.LoanDate);
            Assert.Equal(loan.ReturnDate, data.ReturnDate);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode404_WhenArchive_LoanDoesNotExist()
        {
            // Arrange

            int loanId = 1;

            _archive_LoanRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _archive_LoanController.RestoreByIdAsync(loanId);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int loanId = 1;

            _archive_LoanRepositoryMock
                .Setup(x => x.RestoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _archive_LoanController.RestoreByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
