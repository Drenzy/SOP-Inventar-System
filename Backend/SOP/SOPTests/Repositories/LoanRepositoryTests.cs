namespace SOPTests.Repositories
{
    public class LoanRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly LoanRepository _loanRepository;
        public LoanRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LoanRepositoryTests")
                .Options;

            _context = new(_options);

            _loanRepository = new(_context);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfLoans_WhenLoanExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "222",
            };

            _context.Item.Add(item);

            User user = new()
            {
                Id = 1,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            };

            _context.User.Add(user);

            Loan loan1 = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _context.Loan.Add(loan1);

            Loan loan2 = new()
            {
                Id = 2,
                ItemId = 2,
                UserId = 1,
                LoanDate = new DateTime(2001, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2022, 12, 29, 23, 59, 59),
            };

            _context.Loan.Add(loan2);

            Item item1 = new Item
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "111",
            };

            _context.Item.Add(item1);

            User user1 = new()
            {
                Id = 2,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            };

            _context.User.Add(user1);

            await _context.SaveChangesAsync();

            // Act
            var result = await _loanRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Loan>>(result);

            Assert.Equal(2, result.Count);
            Assert.Equal(loan1.Id, result[0].Id);
            Assert.Equal(loan1.ItemId, result[0].ItemId);
            Assert.Equal(loan1.UserId, result[0].UserId);
            Assert.Equal(loan1.LoanDate, result[0].LoanDate);
            Assert.Equal(loan1.ReturnDate, result[0].ReturnDate);

            Assert.Equal(loan2.Id, result[1].Id);
            Assert.Equal(loan2.ItemId, result[1].ItemId);
            Assert.Equal(loan2.UserId, result[1].UserId);
            Assert.Equal(loan2.LoanDate, result[1].LoanDate);
            Assert.Equal(loan2.ReturnDate, result[1].ReturnDate);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfLoans_WhenNoLoanExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _loanRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Loan>>(result);

            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToLoan_WhenSavingToDatabase()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Loan Loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            // Act
            var result = await _loanRepository.CreateAsync(Loan);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Loan>(result);

            Assert.Equal(expectedId, result?.Id);
            Assert.Equal(Loan.ItemId, result?.ItemId);
            Assert.Equal(Loan.UserId, result?.UserId);
            Assert.Equal(Loan.LoanDate, result?.LoanDate);
            Assert.Equal(Loan.ReturnDate, result?.ReturnDate);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewLoan_WhenLoanIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            Loan Loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            await _loanRepository.CreateAsync(Loan);

            // Act
            async Task action() => await _loanRepository.CreateAsync(Loan);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);

            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnLoan_WhenLoanExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "222",
            };

            _context.Item.Add(item);

            User user = new()
            {
                Id = 1,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            };

            _context.User.Add(user);

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _context.Loan.Add(loan);

            await _context.SaveChangesAsync();

            // Act


            // Act
            var result = await _loanRepository.FindByIdAsync(LoanId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Loan>(result);

            Assert.Equal(LoanId, result.Id);
            Assert.Equal(loan.ItemId, result.ItemId);
            Assert.Equal(loan.UserId, result.UserId);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenLoanDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            // Act
            var result = await _loanRepository.FindByIdAsync(LoanId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedLoan_WhenLoanExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "222",
            };

            _context.Item.Add(item);

            User user = new()
            {
                Id = 1,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            };

            _context.User.Add(user);

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _context.Loan.Add(loan);

            await _context.SaveChangesAsync();

            Loan updateLoan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2099, 12, 1, 1, 1, 1),
                ReturnDate = new DateTime(2099, 12, 1, 1, 1, 1),
            };

            // Act
            var result = await _loanRepository.UpdateByIdAsync(LoanId, updateLoan);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Loan>(result);

            Assert.Equal(updateLoan.LoanDate, result.LoanDate);
            Assert.Equal(updateLoan.ReturnDate, result.ReturnDate);
            Assert.Equal(updateLoan.UserId, result.UserId);
            Assert.Equal(updateLoan.ItemId, result.ItemId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenLoanDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "222",
            });

            _context.User.Add(new User
            {
                Id = 1,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            });

            await _context.SaveChangesAsync();

            Loan updateLoan = new()
            {
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            // Act
            var result = await _loanRepository.UpdateByIdAsync(LoanId, updateLoan);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnArchive_Loan_WhenLoanIsArchived()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            string archiveNote = "Test";

            Item item = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "222",
            };

            _context.Item.Add(item);

            User user = new()
            {
                Id = 1,
                Email = "Test@gmail.com",
                Name = "Test",
                Password = "Test",
            };

            _context.User.Add(user);

            Loan loan = new()
            {
                Id = 1,
                ItemId = 1,
                UserId = 1,
                LoanDate = new DateTime(2002, 12, 29, 23, 59, 59),
                ReturnDate = new DateTime(2002, 12, 29, 23, 59, 59),
            };

            _context.Loan.Add(loan);

            await _context.SaveChangesAsync();

            // Act
            var result = await _loanRepository.ArchiveByIdAsync(LoanId, archiveNote);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Loan>(result);
            Assert.Equal(LoanId, result.Id);
            Assert.Equal(archiveNote, result.ArchiveNote);
            Assert.Equal(loan.ItemId, result.ItemId);
            Assert.Equal(loan.UserId, result.UserId);
            Assert.Equal(loan.LoanDate, result.LoanDate);
            Assert.Equal(loan.ReturnDate, result.ReturnDate);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnNull_WhenLoanDoesNotExist()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int LoanId = 1;

            string archiveNote = "Test";

            // Act
            var result = await _loanRepository.ArchiveByIdAsync(LoanId, archiveNote);

            // Assert
            Assert.Null(result);
        }
    }
}