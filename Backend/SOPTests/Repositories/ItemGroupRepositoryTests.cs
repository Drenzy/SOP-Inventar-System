using SOP.Archive.DTOs;

namespace SOPTests.Repositories
{
    public class ItemGroupRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ItemGroupRepository _itemGroupRepository;
        public ItemGroupRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ItemGroupRepositoryTests")
                .Options;

            _context = new(_optinons);

            _itemGroupRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfItemGroups_WhenItemGroupExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Computer"
            };

            _context.ItemType.Add(itemType);

            ItemGroup itemGroup1 = new()
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30
            };

            _context.ItemGroup.Add(itemGroup1);

            ItemGroup itemGroup2 = new()
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "HP bærbar",
                Price = 4500.00m,
                Manufacturer = "HP",
                WarrantyPeriod = "2 år",
                Quantity = 5
            };

            _context.ItemGroup.Add(itemGroup2);

            await _context.SaveChangesAsync();

            // Act
            var result = await _itemGroupRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<ItemGroup>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(itemGroup1.Id, result[0].Id);
            Assert.Equal(itemGroup1.ItemTypeId, result[0].ItemTypeId);
            Assert.Equal(itemGroup1.ModelName, result[0].ModelName);
            Assert.Equal(itemGroup1.Price, result[0].Price);
            Assert.Equal(itemGroup1.Manufacturer, result[0].Manufacturer);
            Assert.Equal(itemGroup1.WarrantyPeriod, result[0].WarrantyPeriod);
            Assert.Equal(itemGroup1.Quantity, result[0].Quantity);

            Assert.Equal(itemGroup2.Id, result[1].Id);
            Assert.Equal(itemGroup2.ItemTypeId, result[1].ItemTypeId);
            Assert.Equal(itemGroup2.ModelName, result[1].ModelName);
            Assert.Equal(itemGroup2.Price, result[1].Price);
            Assert.Equal(itemGroup2.Manufacturer, result[1].Manufacturer);
            Assert.Equal(itemGroup2.WarrantyPeriod, result[1].WarrantyPeriod);
            Assert.Equal(itemGroup2.Quantity, result[1].Quantity);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOItemGroups_WhenNoItemGroupExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            // Act
            var result = await _itemGroupRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<ItemGroup>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToItemGroup_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            int expectedId = 1;

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Computer"
            };

            _context.ItemType.Add(itemType);

            await _context.SaveChangesAsync();

            ItemGroup itemGroup = new()
            {
                Id = 1,
                ModelName = "Acer Nitro 5",
                ItemTypeId = 1,
                Quantity = 30,
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
            };

            //Act
            var result = await _itemGroupRepository.CreateAsync(itemGroup);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ItemGroup>(result);
            Assert.Equal(expectedId, result?.Id);
            Assert.Equal(itemType.Id, result?.ItemType.Id);
            Assert.Equal(itemType.TypeName, result?.ItemType.TypeName);
            Assert.Equal(itemGroup.ModelName, result?.ModelName);
            Assert.Equal(itemGroup.Price, result?.Price);
            Assert.Equal(itemGroup.Manufacturer, result?.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, result?.WarrantyPeriod);
            Assert.Equal(itemGroup.Quantity, result?.Quantity);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewItemGroup_WhenItemGroupIdAlreadyExists()
        {
            await _context.Database.EnsureDeletedAsync();


            ItemGroup itemGroup = new()
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30
            };

            await _itemGroupRepository.CreateAsync(itemGroup);
            //Act

            async Task action() => await _itemGroupRepository.CreateAsync(itemGroup);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnItemGroup_WhenItemGroupExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int ItemGroupId = 1;

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Computer"
            };

            _context.ItemType.Add(itemType);

            ItemGroup itemGroup = new()
            {
                Id = ItemGroupId,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30,
            };

            _context.ItemGroup.Add(itemGroup);

            await _context.SaveChangesAsync();

            //Act
            var reusult = await _itemGroupRepository.FindByIdAsync(ItemGroupId);

            //Assert
            Assert.NotNull(reusult);
            Assert.Equal(ItemGroupId, reusult.Id);
            Assert.Equal(itemType.Id, reusult.ItemType.Id);
            Assert.Equal(itemType.TypeName, reusult.ItemType.TypeName);
            Assert.Equal(itemGroup.ModelName, reusult.ModelName);
            Assert.Equal(itemGroup.Price, reusult.Price);
            Assert.Equal(itemGroup.Manufacturer, reusult.Manufacturer);
            Assert.Equal(itemGroup.WarrantyPeriod, reusult.WarrantyPeriod);
            Assert.Equal(itemGroup.Quantity, reusult.Quantity);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenItemGroupDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemGroupId = 1;

            //Act
            var reusult = await _itemGroupRepository.FindByIdAsync(itemGroupId);

            // Assert
            Assert.Null(reusult);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedItemGroup_WhenItemGroupExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int ItemGroupId = 1;

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Computer"
            };

            _context.ItemType.Add(itemType);

            ItemGroup itemGroup = new()
            {
                Id = ItemGroupId,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30,
            };

            _context.ItemGroup.Add(itemGroup);

            await _context.SaveChangesAsync();

            ItemGroup updateItemGroup = new()
            {
                ItemTypeId = 1,
                ModelName = "HP bærbar",
                Price = 4500.00m,
                Manufacturer = "HP",
                WarrantyPeriod = "2 år",
                Quantity = 5,
            };

            // Act
            var result = await _itemGroupRepository.UpdateByIdAsync(ItemGroupId, updateItemGroup);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<ItemGroup>(result);

            Assert.Equal(updateItemGroup.ModelName, result.ModelName);
            Assert.Equal(updateItemGroup.Price, result.Price);
            Assert.Equal(updateItemGroup.Manufacturer, result.Manufacturer);
            Assert.Equal(updateItemGroup.WarrantyPeriod, result.WarrantyPeriod);
            Assert.Equal(updateItemGroup.Quantity, result.Quantity);

            Assert.Equal(updateItemGroup.ItemTypeId, result.ItemType.Id);
        }


        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenItemGroupDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int itemGroupId = 1;

            ItemGroup updateItemGroup = new()
            {
                ItemTypeId = 1,
                ModelName = "HP bærbar",
                Price = 4500.00m,
                Manufacturer = "HP",
                WarrantyPeriod = "2 år",
                Quantity = 5
            };
            var result = await _itemGroupRepository.UpdateByIdAsync(itemGroupId, updateItemGroup);

            Assert.Null(result);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnArchive_ItemGroup_WhenItemGroupIsArchived()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemGroupId = 1;

            string archiveNote = "ItemGroup is archived";

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Computer"
            };

            _context.ItemType.Add(itemType);

            ItemGroup itemGroup = new()
            {
                Id = itemGroupId,
                ItemTypeId = 1,
                ModelName = "Acer Nitro 5",
                Price = 9875.99m,
                Manufacturer = "Acer",
                WarrantyPeriod = "3 år",
                Quantity = 30,
            };

            _context.ItemGroup.Add(itemGroup);

            await _context.SaveChangesAsync();

            // Act
            var result = await _itemGroupRepository.ArchiveByIdAsync(itemGroupId, archiveNote);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Archive_ItemGroup>(result);
            Assert.Equal(archiveNote, result.ArchiveNote);
            Assert.Equal(itemGroup.ItemTypeId, result.ItemTypeId);
            Assert.Equal(itemGroup.ModelName, result.ModelName);
            Assert.Equal(itemGroup.Price, result.Price);

        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnNull_WhhenItemGroupIsArchived()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int ItemGroupId = 1;

            string archiveNote = "ItemGroup is archived";

            // Act
            var result = await _itemGroupRepository.ArchiveByIdAsync(ItemGroupId, archiveNote);

            // Assert
            Assert.Null(result);
        }
    }
}