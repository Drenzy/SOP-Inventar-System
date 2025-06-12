using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IArchive_RequestRepository
    {
        Task<List<Archive_Request>> GetAllAsync();
        Task<Archive_Request> FindByIdAsync(int id);
        Task<Archive_Request> DeleteByIdAsync(int id);
        Task<Request> RestoreByIdAsync(int archiveRequestId);
    }
    public class Archive_RequestRepository : IArchive_RequestRepository
    {
        private readonly DatabaseContext _context;

        public Archive_RequestRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Archive_Request>> GetAllAsync()
        {
            return await _context.Archive_Request
                .ToListAsync();
        }

        public async Task<Archive_Request?> FindByIdAsync(int requestId)
        {
            return await _context.Archive_Request
                .FirstOrDefaultAsync(x => x.Id == requestId);
        }

        public async Task<Archive_Request> DeleteByIdAsync(int requestId)
        {
            var request = await FindByIdAsync(requestId);
            if (request != null)
            {
                _context.Archive_Request.Remove(request);
                await _context.SaveChangesAsync();
            }
            return request;
        }

        public async Task<Request> RestoreByIdAsync(int archiveRequestId)
        {
            Archive_Request archiveRequest = await FindByIdAsync(archiveRequestId);
            if (archiveRequest == null)
            {
                return null;
            }

            Request request = new Request
            {
                Id = archiveRequest.Id,
                UserId = archiveRequest.UserId,
                RecipientEmail = archiveRequest.RecipientEmail,
                Item = archiveRequest.Item,
                Message = archiveRequest.Message,
                Date = archiveRequest.Date,
                Status = archiveRequest.Status,
            };

            // Only use transactions when using a real database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreRequestAsync(request, archiveRequest);
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
                await RestoreRequestAsync(request, archiveRequest);
            }

            return request;
        }

        private async Task RestoreRequestAsync(Request request, Archive_Request archiveRequest)
        {
            // Skip IDENTITY_INSERT for in-memory database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Request ON");
            }

            _context.Request.Add(request);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Request OFF");
            }

            _context.Archive_Request.Remove(archiveRequest);
            await _context.SaveChangesAsync();
        }

    }
}
