using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace SOP.Repositories
{
    public interface IArchive_UserRepository
    {
        Task<List<Archive_User>> GetAllAsync();
        Task<Archive_User> FindByIdAsync(int id);
        Task<Archive_User> DeleteByIdAsync(int id);
        Task<List<Archive_User>> GetArchive_UsersByRoleAsync(int RoleId);
        Task<User> RestoreByIdAsync(int archiveUserId);
    }
    public class Archive_UserRepository : IArchive_UserRepository
    {
        private readonly DatabaseContext _context;

        public Archive_UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Archive_User>> GetAllAsync()
        {
            return await _context.Archive_User
                .ToListAsync();
        }

        public async Task<Archive_User?> FindByIdAsync(int Archive_UserId)
        {
            return await _context.Archive_User
                .FirstOrDefaultAsync(x => x.Id == Archive_UserId);
        }

        public async Task<Archive_User> DeleteByIdAsync(int Archive_UserId)
        {
            var Archive_User = await FindByIdAsync(Archive_UserId);
            if (Archive_User != null)
            {
                _context.Archive_User.Remove(Archive_User);
                await _context.SaveChangesAsync();
            }
            return Archive_User;
        }

        public async Task<List<Archive_User>> GetArchive_UsersByRoleAsync(int RoleId)
        {
            return await _context.Archive_User.Where(u => u.RoleId == RoleId).ToListAsync();
        }

        public async Task<User> RestoreByIdAsync(int archiveUserId)
        {
            Archive_User archiveUser = await FindByIdAsync(archiveUserId);
            if (archiveUser == null)
            {
                return null;
            }

            User user = new User
            {
                Id = archiveUser.Id,
                Name = archiveUser.Name,
                Email = archiveUser.Email,
                RoleId = archiveUser.RoleId,
                Password = archiveUser.Password,
                TwoFactorAuthentication = archiveUser.TwoFactorAuthentication,
                TwoFactorSecretKey = archiveUser.TwoFactorSecretKey,
            };

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreUserAsync(user, archiveUser);
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
                await RestoreUserAsync(user, archiveUser);
            }

            return user;
        }

        private async Task RestoreUserAsync(User user, Archive_User archiveUser)
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [User] ON");
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [User] OFF");
            }

            _context.Archive_User.Remove(archiveUser);
            await _context.SaveChangesAsync();
        }

    }
}
