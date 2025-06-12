using SOP.Entities;

namespace SOPTests.Archive.Repositories
{
    public class Archive_LoanRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_LoanRepository _archive_LoanRepository;
        public Archive_LoanRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_LoanRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_LoanRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_Loans_WhenArchive_LoanExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_Loan.Add(new Archive_Loan
            {
                Id = 1,
                DeleteTime = new DateTime(2012, 1, 29),
                LoanDate = new DateTime(2021, 6, 10),
                ReturnDate = new DateTime(2021, 12, 24),
                ItemId = 1,
                UserId = 1,
                ArchiveNote = "Test archive note",
            });

            _context.Archive_Loan.Add(new Archive_Loan
            {
                Id = 2,
                DeleteTime = new DateTime(2001, 1, 29),
                LoanDate = new DateTime(2099, 10, 10),
                ReturnDate = new DateTime(2099, 8, 15),
                ItemId = 2,
                UserId = 3,
                ArchiveNote = "Test archive note",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_LoanRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_Loan>>(result);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_Loans_WhenNoArchive_LoanExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_LoanRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_Loan>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnArchive_Loan_WhenArchive_LoanExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_LoanId = 1;

            _context.Archive_Loan.Add(new Archive_Loan
            {
                Id = archive_LoanId,
                DeleteTime = new DateTime(2001, 1, 29),
                LoanDate = new DateTime(2099, 10, 10),
                ReturnDate = new DateTime(2099, 8, 15),
                ItemId = 2,
                UserId = 3,
                ArchiveNote = "Test archive note",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_LoanRepository.FindByIdAsync(archive_LoanId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(archive_LoanId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_LoanDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_LoanId = 1;

            // Act
            var result = await _archive_LoanRepository.FindByIdAsync(archive_LoanId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnLoan_WhenLoanIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loanId = 1;

            Archive_Loan loan = new()
            {
                Id = loanId,
                DeleteTime = new DateTime(2001, 1, 29),
                LoanDate = new DateTime(2099, 10, 10),
                ReturnDate = new DateTime(2099, 8, 15),
                ItemId = 2,
                UserId = 3,
                ArchiveNote = "Test archive note",
            };

            _context.Archive_Loan.Add(loan);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_LoanRepository.DeleteByIdAsync(loanId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Loan>(result);

            Assert.Equal(loanId, result.Id);

            Assert.Equal(loan.DeleteTime, result.DeleteTime);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);
            Assert.Equal(loan.ItemId, result.ItemId);
            Assert.Equal(loan.UserId, result.UserId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenLoanDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loanId = 1;

            // Act
            var result = await _archive_LoanRepository.DeleteByIdAsync(loanId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnItem_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();

            int loanId = 1;

            Archive_Loan loan = new()
            {
                Id = loanId,
                DeleteTime = new DateTime(2001, 1, 29),
                LoanDate = new DateTime(2099, 10, 10),
                ReturnDate = new DateTime(2099, 8, 15),
                ItemId = 2,
                UserId = 3,
                ArchiveNote = "Test archive note",
            };

            _context.Archive_Loan.Add(loan);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_LoanRepository.RestoreByIdAsync(loanId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Loan>(result);

            Assert.Equal(loanId, result.Id);

            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);

            var itemInDatabase = await _context.Loan.FindAsync(loanId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_Loan.FindAsync(loanId);
            Assert.Null(archiveItemInDatabase);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnNull_WhenArchive_LoanDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_LoanId = 1;

            // Act
            var result = await _archive_LoanRepository.RestoreByIdAsync(archive_LoanId);

            // Assert
            Assert.Null(result);
        }
    }
}
