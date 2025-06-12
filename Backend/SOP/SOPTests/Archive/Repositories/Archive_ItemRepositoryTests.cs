using SOP.Archive.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SOPTests.Archive.Repositories
{
    public class Archive_ItemRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly Archive_ItemRepository _archive_ItemRepository;
        public Archive_ItemRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Archive_ItemRepositoryTests")
                .Options;

            _context = new(_optinons);

            _archive_ItemRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfArchive_Items_WhenArchive_ItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Archive_Item.Add(new Archive_Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                ArchiveNote = "Test Archive Note"
            });

            _context.Archive_Item.Add(new Archive_Item
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "3456345GB45",
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Archive_Item>>(result);

            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfArchive_Items_WhenNoArchive_ItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Archive_Item>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnArchive_Item_WhenArchive_ItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int itemId = 1;

            _context.Archive_Item.Add(new Archive_Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                ArchiveNote = "Test Archive Note"
            });

            _context.Archive_StatusHistory.Add(new Archive_StatusHistory
            {
                Id = 1,
                DeleteTime = new DateTime(2025,1,1),
                StatusUpdateDate = new DateTime(2025, 2, 9),
                ItemId = 1,
                Note = "",
                StatusId = 1,
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemRepository.FindByIdAsync(itemId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(itemId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenArchive_ItemDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            // Act
            var result = await _archive_ItemRepository.FindByIdAsync(itemId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnItem_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            Archive_Item item = new Archive_Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_Item.Add(item);
            await _context.SaveChangesAsync();

            // Act
            Archive_Item result = await _archive_ItemRepository.DeleteByIdAsync(itemId);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Item>(result);

            Assert.Equal(itemId, result.Id);

            Assert.Equal(item.RoomId, result.RoomId);
            Assert.Equal(item.ItemGroupId, result.ItemGroupId);
            Assert.Equal(item.DeleteTime, result.DeleteTime);
            Assert.Equal(item.SerialNumber, result.SerialNumber);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            // Act
            var result = await _archive_ItemRepository.DeleteByIdAsync(itemId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestoreByIdAsync_ShouldReturnItem_WhenItemIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();

            int itemId = 1;

            _context.ItemType.Add(new ItemType
            {
                Id = 1,
                TypeName = "Bord"
            });

            await _context.SaveChangesAsync();

            _context.ItemGroup.Add(new ItemGroup
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            });

            await _context.SaveChangesAsync();

            Archive_Item item = new Archive_Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
                ArchiveNote = "Test Archive Note"
            };

            _context.Archive_Item.Add(item);
            await _context.SaveChangesAsync();

            _context.Archive_StatusHistory.Add(new Archive_StatusHistory
            {
                Id = 1,
                DeleteTime = new DateTime(2025, 1, 1),
                StatusUpdateDate = new DateTime(2025, 2, 9),
                ItemId = itemId,
                Note = "",
                StatusId = 1,
                ArchiveNote = "Test Archive Note"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _archive_ItemRepository.RestoreByIdAsync(itemId);


            // Assert
            Assert.NotNull(result);

            Assert.IsType<Item>(result);

            Assert.Equal(itemId, result.Id);

            Assert.Equal(item.RoomId, result.RoomId);
            Assert.Equal(item.SerialNumber, result.SerialNumber);
            Assert.Equal(item.ItemGroupId, result.ItemGroupId);

            var itemInDatabase = await _context.Item.FindAsync(itemId);
            Assert.NotNull(itemInDatabase);

            var archiveItemInDatabase = await _context.Archive_Item.FindAsync(itemId);
            Assert.Null(archiveItemInDatabase);
        }


        [Fact]
        public async void RestoreByIdAsync_ShouldReturnNull_WhenArchive_ItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            //Act
            var result = await _archive_ItemRepository.RestoreByIdAsync(itemId);

            //Assert
            Assert.Null(result);
        }
    }
}
