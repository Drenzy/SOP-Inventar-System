using SOP.Archive.Entities;

namespace SOPTests.Repositories
{
    public class ItemRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ItemRepository _itemRepository;
        public ItemRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ItemRepositoryTests")
                .Options;

            _context = new(_optinons);

            _itemRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfItems_WhenItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Address address1 = new()
            {
                ZipCode = 1,
                City = "Stockholm",
                Region = "Stockholm",
                Road = "Drottninggatan",
            };

            _context.Address.Add(address1);

            Building building1 = new()
            {
                Id = 1,
                BuildingName = "KTH",
                ZipCode = 1,
            };

            _context.Building.Add(building1);

            Room room1 = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1
            };

            _context.Room.Add(room1);

            Room room2 = new()
            {
                Id = 2,
                BuildingId = 1,
                RoomNumber = 2
            };

            _context.Room.Add(room2);

            ItemType itemType1 = new()
            {
                Id = 1,
                TypeName = "Bord"
            };

            _context.ItemType.Add(itemType1);

            ItemGroup itemGroup1 = new()
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            _context.ItemGroup.Add(itemGroup1);

            ItemGroup itemGroup2 = new()
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            _context.ItemGroup.Add(itemGroup2);

            Item item1 = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            _context.Item.Add(item1);

            Item item2 = new()
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            _context.Item.Add(item2);

            StatusHistory statusHistory1 = new()
            {
                Id = 1,
                StatusUpdateDate = new DateTime(2021, 10, 10),
                ItemId = 1,
                Note = "Item is in good condition",
                StatusId = 1
            };

            _context.StatusHistory.Add(statusHistory1);

            Status status1 = new()
            {
                Id = 1,
                Name = "Good"
            };

            _context.Status.Add(status1);

            await _context.SaveChangesAsync();

            // Act
            var result = await _itemRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            Assert.IsType<List<Item>>(result);

            Assert.Equal(2, result.Count);
            Assert.Equal(item1.Id, result[0].Id);
            Assert.Equal(item1.RoomId, result[0].RoomId);
            Assert.Equal(item1.ItemGroupId, result[0].ItemGroupId);
            Assert.Equal(item1.SerialNumber, result[0].SerialNumber);

            Assert.Equal(item2.Id, result[1].Id);
            Assert.Equal(item2.RoomId, result[1].RoomId);
            Assert.Equal(item2.ItemGroupId, result[1].ItemGroupId);
            Assert.Equal(item2.SerialNumber, result[1].SerialNumber);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfItems_WhenNoItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _itemRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Item>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToItem_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Address address1 = new()
            {
                ZipCode = 1,
                City = "Stockholm",
                Region = "Stockholm",
                Road = "Drottninggatan",
            };

            _context.Address.Add(address1);

            Building building1 = new()
            {
                Id = 1,
                BuildingName = "KTH",
                ZipCode = 1,
            };

            _context.Building.Add(building1);

            Room room1 = new()
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1
            };

            _context.Room.Add(room1);

            Room room2 = new()
            {
                Id = 2,
                BuildingId = 1,
                RoomNumber = 2
            };

            _context.Room.Add(room2);

            ItemType itemType = new()
            {
                Id = 1,
                TypeName = "Bord"
            };

            _context.ItemType.Add(itemType);

            ItemGroup itemGroup1 = new()
            {
                Id = 1,
                ItemTypeId = 1,
                ModelName = "MILLBERGET",
                Price = 559.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            _context.ItemGroup.Add(itemGroup1);

            ItemGroup itemGroup2 = new()
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            };

            _context.ItemGroup.Add(itemGroup2);

            StatusHistory statusHistory = new()
            {
                Id = 1,
                StatusUpdateDate = new DateTime(2021, 10, 10),
                ItemId = 1,
                Note = "Item is in good condition",
                StatusId = 1
            };

            _context.StatusHistory.Add(statusHistory);

            Status status = new()
            {
                Id = 1,
                Name = "Good"
            };

            _context.Status.Add(status);

            await _context.SaveChangesAsync();

            Item item3 = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            //Act
            var result = await _itemRepository.CreateAsync(item3);

            //Assert
            Assert.NotNull(result);

            Assert.IsType<Item>(result);

            Assert.Equal(expectedId, result?.Id);
            Assert.Equal(item3.RoomId, result?.RoomId);
            Assert.Equal(item3.ItemGroupId, result?.ItemGroupId);
            Assert.Equal(item3.SerialNumber, result?.SerialNumber);
            Assert.Equal(item3.ItemGroupId, result?.ItemGroupId);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewitem_WhenItemIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Item item = new()
            {
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            await _itemRepository.CreateAsync(item);

            //Act
            async Task action() => await _itemRepository.CreateAsync(item);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnItem_WhenItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            _context.Address.Add(new Address
            {
                ZipCode = 1,
                City = "Stockholm",
                Region = "Stockholm",
                Road = "Drottninggatan",
            });

            _context.Building.Add(new Building
            {
                Id = 1,
                BuildingName = "KTH",
                ZipCode = 1,
            });

            _context.Room.Add(new Room
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1
            });

            _context.Room.Add(new Room
            {
                Id = 2,
                BuildingId = 1,
                RoomNumber = 2
            });

            _context.ItemType.Add(new ItemType
            {
                Id = 1,
                TypeName = "Bord"
            });

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

            _context.ItemGroup.Add(new ItemGroup
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            });

            _context.Item.Add(new Item
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.Item.Add(new Item
            {
                Id = 2,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.StatusHistory.Add(new StatusHistory
            {
                Id = 1,
                StatusUpdateDate = new DateTime(2021, 10, 10),
                ItemId = 1,
                Note = "Item is in good condition",
                StatusId = 1
            });

            _context.Status.Add(new Status
            {
                Id = 1,
                Name = "Good"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _itemRepository.FindByIdAsync(itemId);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(itemId, result.Id);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            //Act
            var result = await _itemRepository.FindByIdAsync(itemId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedItem_WhenItemExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            int itemId = 1;

            _context.Address.Add(new Address
            {
                ZipCode = 1,
                City = "Stockholm",
                Region = "Stockholm",
                Road = "Drottninggatan",
            });

            _context.Building.Add(new Building
            {
                Id = 1,
                BuildingName = "KTH",
                ZipCode = 1,
            });

            _context.Room.Add(new Room
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1
            });

            _context.Room.Add(new Room
            {
                Id = 2,
                BuildingId = 1,
                RoomNumber = 2
            });

            _context.ItemType.Add(new ItemType
            {
                Id = 1,
                TypeName = "Bord"
            });

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

            _context.ItemGroup.Add(new ItemGroup
            {
                Id = 2,
                ItemTypeId = 1,
                ModelName = "MITTZON",
                Price = 3900.00m,
                Manufacturer = "IKEA",
                WarrantyPeriod = "10 år",
                Quantity = 30
            });

            _context.Item.Add(new Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            });

            _context.StatusHistory.Add(new StatusHistory
            {
                Id = 1,
                StatusUpdateDate = new DateTime(2021, 10, 10),
                ItemId = 1,
                Note = "Item is in good condition",
                StatusId = 1
            });

            _context.Status.Add (new Status
            {
                Id = 1,
                Name = "Good"
            });

            await _context.SaveChangesAsync();

            Item updateItem = new()
            {
                RoomId = 2,
                ItemGroupId = 2,
                SerialNumber = "14123VGE98"
            };

            // Act
            var result = await _itemRepository.UpdateByIdAsync(itemId, updateItem);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Item>(result);

            Assert.Equal(updateItem.RoomId, result.RoomId);
            Assert.Equal(updateItem.SerialNumber, result.SerialNumber);
            Assert.Equal(updateItem.ItemGroupId, result.ItemGroupId);
        }


        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            Item updateitem = new()
            {
                Id = 1,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34"
            };

            //Act
            var result = await _itemRepository.UpdateByIdAsync(itemId, updateitem);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnArchivedItem_WhenItemIsArchived()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            string archiveNote = "Item is archived";

            _context.Address.Add(new Address
            {
                ZipCode = 1,
                City = "Stockholm",
                Region = "Stockholm",
                Road = "Drottninggatan",
            });

            _context.Building.Add(new Building
            {
                Id = 1,
                BuildingName = "KTH",
                ZipCode = 1,
            });

            _context.Room.Add(new Room
            {
                Id = 1,
                BuildingId = 1,
                RoomNumber = 1
            });

            _context.ItemType.Add(new ItemType
            {
                Id = 1,
                TypeName = "Bord"
            });

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


            _context.StatusHistory.Add(new StatusHistory
            {
                Id = 1,
                StatusUpdateDate = new DateTime(2021, 10, 10),
                ItemId = 1,
                Note = "Item is in good condition",
                StatusId = 1
            });

            _context.Status.Add(new Status
            {
                Id = 1,
                Name = "Good"
            });

            _context.Item.Add(new Item
            {
                Id = itemId,
                RoomId = 1,
                ItemGroupId = 1,
                SerialNumber = "14123VGE34",
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _itemRepository.ArchiveByIdAsync(itemId, archiveNote);

            // Assert
            Assert.NotNull(result);

            Assert.IsType<Archive_Item>(result);
            Assert.Equal(itemId, result.Id);
            Assert.Equal(archiveNote, result.ArchiveNote);
            Assert.Equal(1, result.ItemGroupId);
            Assert.Equal(1, result.RoomId);
            Assert.Equal("14123VGE34", result.SerialNumber);

        }


        [Fact]
        public async void ArchiveByIdAsync_ShouldReturnNull_WhenItemIsArchived()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int itemId = 1;

            string archiveNote = "Item is archived";

            //Act
            var result = await _itemRepository.ArchiveByIdAsync(itemId, archiveNote);

            //Assert
            Assert.Null(result);
        }
    }
}
