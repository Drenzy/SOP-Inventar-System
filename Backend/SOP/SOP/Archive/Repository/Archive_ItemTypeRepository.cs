using SOP.DTOs;
using SOP.Entities;

namespace SOP.Archive.Repository
{
    public interface IArchive_ItemTypeRepository
    {
        Task<Archive_ItemType?> FindByIdAsync(int itemTypeId);
        Task<List<Archive_ItemType>> GetAllAsync();
        Task<Archive_ItemType> DeleteByIdAsync(int id);
        Task<ItemType> RestoreByIdAsync(int itemTypeId);
    }
    public class Archive_ItemTypeRepository : IArchive_ItemTypeRepository
    {
        private readonly DatabaseContext _context;

        public Archive_ItemTypeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Archive_ItemType?> FindByIdAsync(int itemTypeId)
        {
            return await _context.Archive_ItemType.FindAsync(itemTypeId);
        }

        public async Task<List<Archive_ItemType>> GetAllAsync()
        {
            return await _context.Archive_ItemType
               .ToListAsync();
        }

        public async Task<Archive_ItemType> DeleteByIdAsync(int itemTypeId)
        {
            var itemType = await FindByIdAsync(itemTypeId);
            if (itemType != null)
            {
                _context.Archive_ItemType.Remove(itemType);
                await _context.SaveChangesAsync();
            }
            return itemType;
        }

        public async Task<ItemType> RestoreByIdAsync(int itemTypeId)
        {
            Archive_ItemType archiveItemType = await FindByIdAsync(itemTypeId);
            if (archiveItemType == null)
            {
                return null;
            }

            ItemType itemType = new ItemType
            {
                Id = archiveItemType.Id,
                TypeName = archiveItemType.TypeName,
            };

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreItemTypeAsync(itemType, archiveItemType);
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
                await RestoreItemTypeAsync(itemType, archiveItemType);
            }

            return itemType;
        }

        private async Task RestoreItemTypeAsync(ItemType itemType, Archive_ItemType archiveItemType)
        {
            // Skip IDENTITY_INSERT for in-memory database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ItemType ON");
            }

            _context.ItemType.Add(itemType);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ItemType OFF");
            }

            _context.Archive_ItemType.Remove(archiveItemType);
            await _context.SaveChangesAsync();
        }

    }
}
