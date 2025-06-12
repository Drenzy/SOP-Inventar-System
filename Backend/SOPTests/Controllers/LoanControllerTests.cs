using SOP.Archive.DTOs;
using SOP.Entities;

namespace SOPTests.Controllers
{
    public class LoanControllerTests
    { 
        private readonly LoanController _loanController;
        private readonly Mock<ILoanRepository> _loanRepositoryMock = new();
        private readonly ITestOutputHelper _testOutputHelper;

        public LoanControllerTests(ITestOutputHelper testOutputHelper)
        {
            _loanController = new(_loanRepositoryMock.Object);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenLoansExists()
        {
            // Arrange
            List<Loan> loans = new()
            {
                new Loan
                {
                    Id = 1,
                    ItemId = 1,
                    UserId = 1,
                    LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    Item = new Item { 
                        Id = 1,
                        ItemGroupId = 1,
                        RoomId = 1,
                        SerialNumber = "222",
                    }
                },
                new Loan
                {
                    Id = 2,
                    ItemId = 1,
                    UserId = 1,
                    LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                    Item = new Item
                    {
                        Id = 1,
                        ItemGroupId = 1,
                        RoomId = 1,
                        SerialNumber = "222",
                    },
                },
            };

            _loanRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(loans);

            // Act
            var result = await _loanController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<LoanResponse>>(objectResult.Value);

            var data = objectResult.Value as List<LoanResponse>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _loanRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _loanController.GetAllAsync();

            // Assert
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<LoanResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode200_WhenLoanIsSuccessfullyCreated()
        {
            // Arrange
            LoanRequest loanRequest = new()
            {
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            int loanId = 1;
            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23,59,59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                Item = new Item
                {
                    Id = 1,
                    ItemGroupId = 1,
                    RoomId = 1,
                    SerialNumber = "222",
                },
            };

            _loanRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Loan>()))
                .ReturnsAsync(loan);

            // Act
            var result = await _loanController.CreateAsync(loanRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoanResponse;

            Assert.NotNull(data);
            Assert.Equal(loanId, data.Id);
            Assert.Equal(loan.LoanDate, data.LoanDate);
            Assert.Equal(loan.ReturnDate, data.ReturnDate);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            LoanRequest loanRequest = new()
            {
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            _loanRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Loan>()))
            .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _loanController.CreateAsync(loanRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Find by id async should return status code 200 when loan exists
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenLoanExists()
        {
            // Arrange
            int loanId = 1;
            LoanResponse loanResponse = new()
            {
                Id = loanId,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                Item = new Item
                {
                    Id = 1,
                    ItemGroupId = 1,
                    RoomId = 1,
                    SerialNumber = "222",
                },
            };

            _loanRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(loan);

            // Act
            var result = await _loanController.FindByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoanResponse;

            Assert.NotNull(data);
            Assert.Equal(loanId, data.Id);
            Assert.Equal(loan.LoanDate, data.LoanDate);
            Assert.Equal(loan.ReturnDate, data.ReturnDate);
        }

        // Find by id async should return status code 404 when loan does not exist.
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenLoanDoesNotExist()
        {
            // Arrange
            int loanId = 1;

            _loanRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loanController.FindByIdAsync(loanId);

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
            int loanId = 1;

            _loanRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _loanController.FindByIdAsync(loanId);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Update by id async should return status code 200 when loan is updated.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenLoanIsUpdated()
        {
            // Arrange 
            LoanRequest loanRequest = new()
            {
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            int loanId = 1;

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                Item = new Item
                {
                    Id = 1,
                    ItemGroupId = 1,
                    RoomId = 1,
                    SerialNumber = "222",
                },
            };

            _loanRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Loan>()))
                .ReturnsAsync(loan);

            // Act
            var result = await _loanController.UpdateByIdAsync(loanId, loanRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoanResponse;

            Assert.NotNull(data);
            Assert.Equal(loanRequest.LoanDate, data.LoanDate);
            Assert.Equal(loanRequest.ReturnDate, data.ReturnDate);
        }

        // Update by id async should return status code 404 when loan does not exist.
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenLoanDoesNotExist()
        {
            // Arrange
            LoanRequest loanRequest = new()
            {
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            int loanId = 1;

            _loanRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Loan>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loanController.UpdateByIdAsync(loanId, loanRequest);

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
            LoanRequest loanRequest = new()
            {
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2003, 1, 29, 23, 59, 59),
                ItemId = 1,
                UserId = 1,
            };

            int loanId = 1;

            _loanRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Loan>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _loanController.UpdateByIdAsync(loanId, loanRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        //Delete by id async should return status code 200 when loan is deleted.
        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode200_WhenLoanIsDeleted()
        {
            // Arrange 
            int loanId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            Archive_Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
                DeleteTime = new DateTime(2002, 12, 29, 23, 59, 59),
                ArchiveNote = "This is an archive note",
            };

            _loanRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(loan);

            // Act
            var result = await _loanController.ArchiveByIdAsync(loanId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as Archive_LoanResponse;

            Assert.NotNull(data);
            Assert.Equal(loan.Id, data.Id);
            Assert.Equal(loan.LoanDate, data.LoanDate);
            Assert.Equal(loan.ReturnDate, data.ReturnDate);
        }

        //Delete by id async should return status code 404 when loan does not exist.

       [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode404_WhenLoanDoesNotExist()
        {
            // Arrange

            int loanId = 1;
            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _loanRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _loanController.ArchiveByIdAsync(loanId, archiveNoteRequest);

            // Assert 
            var objectResult = result as NotFoundResult;

            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        //Delete by id async should return status code 500 when exception is raised
       [Fact]
        public async void ArchiveByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int loanId = 1;

            ArchiveNoteRequest archiveNoteRequest = new()
            {
                ArchiveNote = "This is an archive note"
            };

            _loanRepositoryMock
                .Setup(x => x.ArchiveByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            // Act
            var result = await _loanController.ArchiveByIdAsync(loanId, archiveNoteRequest);

            // Assert 
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
