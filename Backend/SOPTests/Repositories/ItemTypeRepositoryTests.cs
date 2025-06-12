namespace SOPTests.Repositories
{
    public class ItemTypeRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ItemTypeRepository _itemTypeRepository;
        public ItemTypeRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ItemTypeRepository")
                .Options;

            _context = new(_optinons);

            _itemTypeRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfItems_WhenItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Bord"
            };

            _context.ItemType.Add(itemType);

            await _context.SaveChangesAsync();

            //Act
            var result = await _itemTypeRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<ItemType>>(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(itemType.Id, result[0].Id);
            Assert.Equal(itemType.TypeName, result[0].TypeName);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfItems_WhenNoItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _itemTypeRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<ItemType>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToItem_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            await _context.SaveChangesAsync();

            ItemType itemType = new()
            {
                TypeName = "e",
            };

            //Act
            var result = await _itemTypeRepository.CreateAsync(itemType);

            //Assert
            Assert.NotNull(result);

            Assert.IsType<ItemType>(result);

            Assert.Equal(expectedId, result?.Id);
            Assert.Equal(itemType.TypeName, result?.TypeName);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewitem_WhenItemIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "test"
            };

            await _itemTypeRepository.CreateAsync(itemType);

            //Act
            async Task action() => await _itemTypeRepository.CreateAsync(itemType);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnItem_WhenItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int itemTypeId = 1;

            ItemType itemType = new()
            {
                Id = itemTypeId,
                TypeName = "Bord"
            };

            _context.ItemType.Add(itemType);

            await _context.SaveChangesAsync();

            //Act
            var result = await _itemTypeRepository.FindByIdAsync(itemTypeId);

            //Assert
            Assert.NotNull(result);

            Assert.IsType<ItemType>(result);

            Assert.Equal(itemTypeId, result.Id);
            Assert.Equal(itemType.TypeName, result.TypeName);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemTypeId = 1;

            //Act
            var result = await _itemTypeRepository.FindByIdAsync(itemTypeId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnArchive_Item_WhenItemIsArchived()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int Archive_ItemTypeId = 1;

            string archiveNote = "Test archive note";

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Bord"
            };

            _context.ItemType.Add(itemType);

            await _context.SaveChangesAsync();

            //Act
            var result = await _itemTypeRepository.ArchiveByIdAsync(Archive_ItemTypeId, archiveNote);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Archive_ItemType>(result);
            Assert.Equal(Archive_ItemTypeId, result.Id);
            Assert.Equal(archiveNote, result.ArchiveNote);
            Assert.Equal(itemType.TypeName, result.TypeName);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnNull_WhenItemTypeIsArchived()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemTypeId = 1;

            string archiveNote = "Test archive note";

            //Act
            var result = await _itemTypeRepository.ArchiveByIdAsync(itemTypeId, archiveNote);

            //Assert
            Assert.Null(result);
        }
    }
}
