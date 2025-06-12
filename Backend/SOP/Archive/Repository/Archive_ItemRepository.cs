using SOP.Entities;

namespace SOP.Repositories
{
    public interface IArchive_ItemRepository
    {
        Task<Archive_Item?> FindByIdAsync(int itemId);
        Task<Archive_Item?> DeleteByIdAsync(int itemId);
        Task<List<Archive_Item>> GetAllAsync();
        Task<Item> RestoreByIdAsync(int itemId);
    }
    public class Archive_ItemRepository : IArchive_ItemRepository
    {
        private readonly DatabaseContext _context;

        public Archive_ItemRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Archive_Item?> DeleteByIdAsync(int itemId)
        {
            var item = await FindByIdAsync(itemId);
            if (item != null)
            {
                _context.Archive_Item.Remove(item);

                await _context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<Archive_Item?> FindByIdAsync(int itemId)
        {
            return await _context.Archive_Item
                .Include(i => i.StatusHistories)
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task<List<Archive_Item>> GetAllAsync()
        {
            return await _context.Archive_Item
                .Include(i => i.StatusHistories)
                .ToListAsync();
        }

        public async Task<Item> RestoreByIdAsync(int itemId)
        {
            Archive_Item archiveItem = await FindByIdAsync(itemId);
            if (archiveItem == null)
            {
                return null;
            }

            Item item = new Item
            {
                Id = archiveItem.Id,
                RoomId = archiveItem.RoomId,
                ItemGroupId = archiveItem.ItemGroupId,
                SerialNumber = archiveItem.SerialNumber,
            };

            // Only use transactions when using a real database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreItemAsync(item, archiveItem);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await RestoreItemAsync(item, archiveItem);
            }

            return item;
        }

        private async Task RestoreItemAsync(Item item, Archive_Item archiveItem)
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Item ON");
            }


            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Item OFF");
            }

            var statusHistories = archiveItem.StatusHistories.Select(statusHistory => new StatusHistory
            {
                Id = statusHistory.Id,
                ItemId = item.Id,
                StatusId = statusHistory.StatusId,
                StatusUpdateDate = statusHistory.StatusUpdateDate,
                Note = statusHistory.Note
            }).ToList();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT StatusHistory ON");
            }

            _context.StatusHistory.AddRange(statusHistories);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT StatusHistory OFF");
            }


            var archiveStatusHistories = _context.Archive_StatusHistory.Where(ash => ash.ItemId == archiveItem.Id);
            _context.Archive_StatusHistory.RemoveRange(archiveStatusHistories);
            await _context.SaveChangesAsync();

            _context.Archive_Item.Remove(archiveItem);
            await _context.SaveChangesAsync();
        }

    }
}
