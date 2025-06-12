using SOP.Entities;

namespace SOPTests.Archive.Repositories
{
    public class Archive_ItemTypeRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_ItemTypeRepository _archive_ItemTypeRepository;
        public Archive_ItemTypeRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_ItemTypeRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_ItemTypeRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_ItemTpes_WhenArchive_ItemTypesExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_ItemType.Add(new Archive_ItemType
            {
                Id = 1,
                DeleteTime = DateTime.Now,
                TypeName = "Type1",
                ArchiveNote = "Test"
            });

            _context.Archive_ItemType.Add(new Archive_ItemType
            {
                Id = 2,
                DeleteTime = DateTime.Now,
                TypeName = "Type2",
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemTypeRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_ItemType>>(result);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_ItemTypes_WhenNoArchive_ItemTypesExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemTypeRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_ItemType>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnArchive_ItemType_WhenArchive_ItemTypeExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_ItemTypeId = 1;

            _context.Archive_ItemType.Add(new Archive_ItemType
            {
                Id = 1,
                DeleteTime = DateTime.Now,
                TypeName = "Type1",
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemTypeRepository.FindByIdAsync(archive_ItemTypeId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(archive_ItemTypeId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_ItemTypeDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_ItemTypeId = 1;

            // Act
            var result = await _archive_ItemTypeRepository.FindByIdAsync(archive_ItemTypeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnItemType_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemTypeId = 1;

            Archive_ItemType itemType = new()
            {
                Id = itemTypeId,
                DeleteTime = DateTime.Now,
                TypeName = "Bord",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_ItemType.Add(itemType);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemTypeRepository.DeleteByIdAsync(itemTypeId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_ItemType>(result);

            Assert.Equal(itemTypeId, result.Id);

            Assert.Equal(itemType.DeleteTime, result.DeleteTime);
            Assert.Equal(itemType.TypeName, result.TypeName);
        }


        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenItemTypeDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemTypeId = 1;

            // Act
            var result = await _archive_ItemTypeRepository.DeleteByIdAsync(itemTypeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnItemType_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();

            int itemTypeId = 1;

            Archive_ItemType itemType = new()
            {
                Id = 1,
                DeleteTime = DateTime.Now,
                TypeName = "Type1",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_ItemType.Add(itemType);
            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemTypeRepository.RestoreByIdAsync(itemTypeId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<ItemType>(result);

            Assert.Equal(itemTypeId, result.Id);

            Assert.Equal(itemType.TypeName, result.TypeName);

            var itemInDatabase = await _context.ItemType.FindAsync(itemTypeId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_ItemType.FindAsync(itemTypeId);
            Assert.Null(archiveItemInDatabase);
        }


        [Fact]
        public async void RestoreByIdAsync_ShouldReturnNull_WhenArchive_ItemTypesDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemTypeId = 1;

            // Act
            var result = await _archive_ItemTypeRepository.RestoreByIdAsync(itemTypeId);

            // Assert
            Assert.Null(result);
        }
    }
}
