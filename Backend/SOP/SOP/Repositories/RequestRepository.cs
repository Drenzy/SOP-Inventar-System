using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IRequestRepository
    {
        Task<List<Request>> GetAllAsync();
        Task<Request> CreateAsync(Request request);
        Task<Request> FindByIdAsync(int id);
        Task<Request> UpdateByIdAsync(int id, Request request);
        Task<Archive_Request> ArchiveByIdAsync(int id, string archiveNote);
    }
    public class RequestRepository : IRequestRepository
    {
        private readonly DatabaseContext _context;

        public RequestRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Request>> GetAllAsync()
        {
            return await _context.Request
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<Request> CreateAsync(Request newRequest)
        {
            _context.Request.Add(newRequest);
            await _context.SaveChangesAsync();
            return newRequest;
        }

        public async Task<Request?> FindByIdAsync(int requestId)
        {
            return await _context.Request
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == requestId);
        }

        public async Task<Request> UpdateByIdAsync(int id, Request newRequest)
        {
            var request = await FindByIdAsync(id);

            if (request != null)
            {
                request.UserId = newRequest.UserId;
                request.RecipientEmail = newRequest.RecipientEmail;
                request.Item = newRequest.Item;
                request.Message = newRequest.Message;
                request.Date = newRequest.Date;
                request.Status = newRequest.Status;

                await _context.SaveChangesAsync();

                request = await FindByIdAsync(id);
            }
            return request;
        }

        // Archive Request by ID.

        public async Task<Archive_Request> ArchiveByIdAsync(int requestId, string archiveNote)

        {
            var request = await FindByIdAsync(requestId);
            if (request == null)
            {
                return null;
            }
            Archive_Request archiveRequest = new Archive_Request
            {
                Id = request.Id,
                DeleteTime = DateTime.Now,
                UserId = request.UserId,
                RecipientEmail = request.RecipientEmail,
                Item = request.Item,
                Message = request.Message,
                Date = request.Date,
                Status = request.Status,
                ArchiveNote = archiveNote,
            };
            if (request != null)
            {
                _context.Archive_Request.Add(archiveRequest);
                _context.Request.Remove(request);
                await _context.SaveChangesAsync();
            }
            return archiveRequest;
        }
    }
}
