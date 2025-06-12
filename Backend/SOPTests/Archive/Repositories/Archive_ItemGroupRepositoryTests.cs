using SOP.Entities;

namespace SOPTests.Archive.Repositories
{
    public class Archive_ItemGroupRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_ItemGroupRepository _archive_ItemGroupRepository;
        public Archive_ItemGroupRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_ItemGroupRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_ItemGroupRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_ItemGroups_WhenItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_ItemGroup.Add(new Archive_ItemGroup
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = "Test archive note",
            });

            _context.Archive_ItemGroup.Add(new Archive_ItemGroup
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = "Test archive note",
            });

            await _context.SaveChangesAsync();

            var count = await _context.Archive_ItemGroup.CountAsync();
            Assert.Equal(2, count); 

            // Act
            var result = await _archive_ItemGroupRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_ItemGroup>>(result);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_ItemGroups_WhenNoItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _archive_ItemGroupRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_ItemGroup>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnItem_WhenArchive_ItemGroupExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_ItemGroupId = 1;

            _context.Archive_ItemGroup.Add(new Archive_ItemGroup
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = "Test archive note",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemGroupRepository.FindByIdAsync(archive_ItemGroupId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(archive_ItemGroupId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_ItemGroupDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int archive_ItemGroupId = 1;

            //Act
            var result = await _archive_ItemGroupRepository.FindByIdAsync(archive_ItemGroupId);

            //Assert
            Assert.Null(result);
        }
       

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnItem_WhenArchive_ItemGroupIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int archive_ItemGroupId = 1;

            Archive_ItemGroup archive_ItemGroup = new Archive_ItemGroup
            {
                Id = archive_ItemGroupId,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = "Test archive note",
            };

            _context.Archive_ItemGroup.Add(archive_ItemGroup);

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemGroupRepository.DeleteByIdAsync(archive_ItemGroupId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_ItemGroup>(result);

            Assert.Equal(archive_ItemGroupId, result.Id);

            Assert.Equal(archive_ItemGroup.ItemTypeId, result.ItemTypeId);
            Assert.Equal(archive_ItemGroup.ModelName, result.ModelName);
            Assert.Equal(archive_ItemGroup.Price, result.Price);
            Assert.Equal(archive_ItemGroup.Manufacturer, result.Manufacturer);
            Assert.Equal(archive_ItemGroup.WarrantyPeriod, result.WarrantyPeriod);
            Assert.Equal(archive_ItemGroup.Quantity, result.Quantity);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenArchive_ItemGroupDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemGroupId = 1;

            // Act
            var result = await _archive_ItemGroupRepository.DeleteByIdAsync(itemGroupId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RestoreByIdAsync_ShouldRestoreItemGroup_WhenItemGroupExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync(); 
            await _context.SaveChangesAsync();
            int itemGroupId = 1;

            _context.ItemType.Add(new ItemType
            {
                Id = 1,
                TypeName = "Bord"
            });

            await _context.SaveChangesAsync();

            var archiveItemGroup = new Archive_ItemGroup
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30,
                ArchiveNote = "Test archive note",
            };

            _context.Archive_ItemGroup.Add(archiveItemGroup);
            await _context.SaveChangesAsync();

            // Act
            var restoredItemGroup = await _archive_ItemGroupRepository.RestoreByIdAsync(itemGroupId);

            // Assert
            Assert.NotNull(restoredItemGroup); 
            Assert.Equal(itemGroupId, restoredItemGroup.Id);
            Assert.Equal(archiveItemGroup.ItemTypeId, restoredItemGroup.ItemTypeId);
            Assert.Equal(archiveItemGroup.ModelName, restoredItemGroup.ModelName);
            Assert.Equal(archiveItemGroup.Price, restoredItemGroup.Price);
            Assert.Equal(archiveItemGroup.Manufacturer, restoredItemGroup.Manufacturer);
            Assert.Equal(archiveItemGroup.WarrantyPeriod, restoredItemGroup.WarrantyPeriod);
            Assert.Equal(archiveItemGroup.Quantity, restoredItemGroup.Quantity);

            var itemInDatabase = await _context.ItemGroup.FindAsync(itemGroupId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_ItemGroup.FindAsync(itemGroupId);
            Assert.Null(archiveItemInDatabase);
        }

        [Fact]
        public async Task RestoreByIdAsync_ShouldReturnNull_WhenItemGroupDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync(); 
            int nonExistentId = 99;

            // Act
            var result = await _archive_ItemGroupRepository.RestoreByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result); 
        }
    }
}
