using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IStatusRepository
    {
        Task<Status> CreateAsync(Status newStatus);
        Task<Status?> FindByIdAsync(int statusId);
        Task<List<Status>> GetAllAsync();
    }
    public class StatusRepository : IStatusRepository
    {
        private readonly DatabaseContext _context;

        public StatusRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Status> CreateAsync(Status newStatus)
        {
            _context.Status.Add(newStatus);
            await _context.SaveChangesAsync();
            newStatus = await FindByIdAsync(newStatus.Id);
            return newStatus;
        }

        public async Task<Status?> FindByIdAsync(int statusId)
        {
            return await _context.Status.FindAsync(statusId);
        }

        public async Task<List<Status>> GetAllAsync()
        {
            return await _context.Status
               .ToListAsync();
        }

    }
}
