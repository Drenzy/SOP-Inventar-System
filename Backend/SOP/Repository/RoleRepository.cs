using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> FindByIdAsync(int id);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _context;

        public RoleRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Role.ToListAsync();
        }

        public async Task<Role?> FindByIdAsync(int roleId)
        {
            return await _context.Role
                .FirstOrDefaultAsync(x => x.Id == roleId);
        }
    }
}
