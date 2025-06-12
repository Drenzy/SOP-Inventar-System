using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.DTOs;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IArchive_ItemGroupRepository
    {
        Task<Archive_ItemGroup?> FindByIdAsync(int itemGroupId);
        Task<List<Archive_ItemGroup>> GetAllAsync();
        Task<Archive_ItemGroup> DeleteByIdAsync(int id);
        Task<ItemGroup> RestoreByIdAsync(int itemGroupId);
    }
    public class Archive_ItemGroupRepository : IArchive_ItemGroupRepository
    {
        private readonly DatabaseContext _context;

        public Archive_ItemGroupRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Archive_ItemGroup?> FindByIdAsync(int itemGroupId)
        {
            return await _context.Archive_ItemGroup
                .FirstOrDefaultAsync(i => i.Id == itemGroupId);
        }

        public async Task<List<Archive_ItemGroup>> GetAllAsync()
        {
            return await _context.Archive_ItemGroup
                .ToListAsync();
        }

        public async Task<Archive_ItemGroup> DeleteByIdAsync(int itemGroupId)
        {
            var itemGroup = await FindByIdAsync(itemGroupId);
            if (itemGroup != null)
            {
                _context.Archive_ItemGroup.Remove(itemGroup);
                await _context.SaveChangesAsync();
            }
            return itemGroup;
        }

        public async Task<ItemGroup> RestoreByIdAsync(int itemGroupId)
        {
            Archive_ItemGroup archiveItemGroup = await FindByIdAsync(itemGroupId);
            if (archiveItemGroup == null)
            {
                return null;
            }
            ItemGroup itemGroup = new ItemGroup
            {
                Id = archiveItemGroup.Id,
                ItemTypeId = archiveItemGroup.ItemTypeId,
                ModelName = archiveItemGroup.ModelName,
                Price = archiveItemGroup.Price,
                Manufacturer = archiveItemGroup.Manufacturer,
                WarrantyPeriod = archiveItemGroup.WarrantyPeriod,
                Quantity = archiveItemGroup.Quantity,
            };

            // Only use transactions when using a real database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreItemGroupAsync(itemGroup, archiveItemGroup);
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
                await RestoreItemGroupAsync(itemGroup, archiveItemGroup);
            }

            return itemGroup;
        }

        private async Task RestoreItemGroupAsync(ItemGroup itemGroup, Archive_ItemGroup archiveItemGroup)
        {
            // Skip IDENTITY_INSERT for in-memory database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ItemGroup ON");
            }

            _context.ItemGroup.Add(itemGroup);
            await _context.SaveChangesAsync();
           
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ItemGroup OFF");
            }

            _context.Archive_ItemGroup.Remove(archiveItemGroup);
            await _context.SaveChangesAsync();
        }

    }
}
