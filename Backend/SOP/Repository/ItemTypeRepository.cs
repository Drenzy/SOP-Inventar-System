namespace SOP.Repositories
{
    public interface IItemTypeRepository
    {
        Task<ItemType> CreateAsync(ItemType newItemType);
        Task<ItemType?> FindByIdAsync(int itemTypeId);
        Task<List<ItemType>> GetAllAsync();

        Task<Archive_ItemType> ArchiveByIdAsync(int itemTypeId, string archiveNote);
    }
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly DatabaseContext _context;

        public ItemTypeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ItemType> CreateAsync(ItemType newItemType)
        {
            _context.ItemType.Add(newItemType);
            await _context.SaveChangesAsync();
            newItemType = await FindByIdAsync(newItemType.Id);
            return newItemType;
        }

        public async Task<ItemType?> FindByIdAsync(int itemTypeId)
        {
            return await _context.ItemType.FindAsync(itemTypeId);
        }

        public async Task<List<ItemType>> GetAllAsync()
        {
            return await _context.ItemType
               .ToListAsync();
        }

        // Archive an ItemType by ID, including all associated itemGroups and items. 

        public async Task<Archive_ItemType> ArchiveByIdAsync(int itemTypeId, string archiveNote)

        {
            ItemType itemType = await FindByIdAsync(itemTypeId);

            if (itemType == null)
            {
                return null;
            }

            // Fetch all ItemGroups associated with this ItemType
            List<ItemGroup> itemGroupsToArchive = await _context.ItemGroup
                .Where(ig => ig.ItemTypeId == itemTypeId)
                .ToListAsync();

            // Fetch all Items directly related to these ItemGroups
            List<Item> itemsToArchive = await _context.Item
                .Where(item => itemGroupsToArchive.Select(ig => ig.Id).Contains(item.ItemGroupId))
                .ToListAsync();

            // Archive each Item first
            foreach (var item in itemsToArchive)
            {
                await ArchiveItem(item, archiveNote);
            }

            // Archive each ItemGroup
            foreach (var itemGroup in itemGroupsToArchive)
            {
                await ArchiveItemGroup(itemGroup, archiveNote);
            }

            Archive_ItemType archiveItemType = new Archive_ItemType
            {
                Id = itemType.Id,
                DeleteTime = DateTime.Now,
                TypeName = itemType.TypeName,
                ArchiveNote = archiveNote,
            };

            _context.Archive_ItemType.Add(archiveItemType);
            _context.ItemType.Remove(itemType);
            await _context.SaveChangesAsync();

            return archiveItemType;
        }

        private async Task ArchiveItemGroup(ItemGroup itemGroup, string archiveNote)
        {
            Archive_ItemGroup archiveItemGroup = new Archive_ItemGroup
            {
                Id = itemGroup.Id,
                DeleteTime = DateTime.Now,
                Quantity = itemGroup.Quantity,
                Price = itemGroup.Price,
                Manufacturer = itemGroup.Manufacturer,
                ItemTypeId = itemGroup.ItemTypeId,
                ModelName = itemGroup.ModelName,
                WarrantyPeriod = itemGroup.WarrantyPeriod,
                ArchiveNote = archiveNote,
            };

            _context.Archive_ItemGroup.Add(archiveItemGroup);
            _context.ItemGroup.Remove(itemGroup);
            await _context.SaveChangesAsync();
        }

        private async Task ArchiveItem(Item item, string archiveNote)
        {
            Archive_Item archiveItem = new Archive_Item
            {
                Id = item.Id,
                DeleteTime = DateTime.Now,
                ItemGroupId = item.ItemGroupId,
                RoomId = item.RoomId,
                SerialNumber = item.SerialNumber,
                ArchiveNote = archiveNote,
                StatusHistories = item.StatusHistories?.Select(statusHistory => new Archive_StatusHistory
                {
                    Id = statusHistory.Id,
                    ItemId = item.Id,
                    StatusId = statusHistory.StatusId,
                    StatusUpdateDate = statusHistory.StatusUpdateDate,
                    Note = statusHistory.Note,
                    DeleteTime = DateTime.Now,
                    ArchiveNote = archiveNote,
                }).ToList()
            };

            _context.Archive_Item.Add(archiveItem);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
