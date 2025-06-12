using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IStatusHistoryRepository
    {
        Task<StatusHistory> CreateAsync(StatusHistory newStatusHistory);
        Task<StatusHistory?> UpdateByIdAsync(int statusHistoryId, StatusHistory updateStatusHistory);
        Task<StatusHistory?> FindByIdAsync(int statusHistoryId);
        Task<List<StatusHistory>> GetAllAsync();
    }
    public class StatusHistoryRepository : IStatusHistoryRepository
    {
        private readonly DatabaseContext _context;

        public StatusHistoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<StatusHistory> CreateAsync(StatusHistory newStatusHistory)
        {
            _context.StatusHistory.Add(newStatusHistory);
            await _context.SaveChangesAsync();
            newStatusHistory = await FindByIdAsync(newStatusHistory.Id);
            return newStatusHistory;
        }

        public async Task<StatusHistory?> FindByIdAsync(int statusHistoryId)
        {
            return await _context.StatusHistory
                .Include(sh => sh.Status)
                .Include(sh => sh.Item)
                .FirstOrDefaultAsync(sh => sh.Id == statusHistoryId);
        }

        public async Task<List<StatusHistory>> GetAllAsync()
        {
            return await _context.StatusHistory
               .Include(sh => sh.Item)
               .Include(sh => sh.Status)
               .ToListAsync();
        }

        public async Task<StatusHistory?> UpdateByIdAsync(int statusHistoryId, StatusHistory updateStatusHistory)
        {
            var statusHistory = await FindByIdAsync(statusHistoryId);
            if (statusHistory != null)
            {
                statusHistory.ItemId = updateStatusHistory.ItemId;
                statusHistory.StatusId = updateStatusHistory.StatusId;
                statusHistory.StatusUpdateDate = updateStatusHistory.StatusUpdateDate;
                statusHistory.Note = updateStatusHistory.Note;

                await _context.SaveChangesAsync();

                statusHistory = await FindByIdAsync(statusHistoryId);
            }
            return statusHistory;
        }
    }
}
