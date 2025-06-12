using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IComputerPartRepository
    {
        Task<ComputerPart> CreateAsync(ComputerPart newComputerPart);
        Task<ComputerPart?> UpdateByIdAsync(int computerPartId, ComputerPart updateComputerPart);
        Task<ComputerPart?> DeleteByIdAsync(int computerPartId);
        Task<ComputerPart?> FindByIdAsync(int computerPartId);
        Task<List<ComputerPart>> GetAllAsync();
    }
    public class ComputerPartRepository : IComputerPartRepository
    {
        private readonly DatabaseContext _context;

        // Initializes the repository with the database context for accessing data
        public ComputerPartRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Adds a new ComputerPart, saves changes, retrieves, and returns it
        public async Task<ComputerPart> CreateAsync(ComputerPart newComputerPart)
        {
            _context.ComputerPart.Add(newComputerPart);
            await _context.SaveChangesAsync();
            newComputerPart = await FindByIdAsync(newComputerPart.Id);
            return newComputerPart;
        }

        // Finds and deletes a ComputerPart by ID, then saves changes and returns it
        public async Task<ComputerPart?> DeleteByIdAsync(int computerPartId)
        {
            var computerPart = await FindByIdAsync(computerPartId);
            if (computerPart != null)
            {
                _context.ComputerPart.Remove(computerPart);
                await _context.SaveChangesAsync();
            }
            return computerPart;
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Finds a ComputerPart by ID, including related entities and returns it
        public async Task<ComputerPart?> FindByIdAsync(int computerPartId)
        {
            return await _context.ComputerPart
                .Include(cp => cp.PartGroup)
                .ThenInclude(cp => cp.PartType)
                .Include(cp => cp.Computer_ComputerPart)
                .FirstOrDefaultAsync(cp => cp.Id == computerPartId);
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Retrieves all ComputerParts, including related entities and returns them
        public async Task<List<ComputerPart>> GetAllAsync()
        {
            return await _context.ComputerPart
                .Include(cp => cp.PartGroup)
                .ThenInclude(cp => cp.PartType)
                .Include(cp => cp.Computer_ComputerPart)
                .ToListAsync();
        }

        // Updates a ComputerPart by ID and returns the updated entity. 
        public async Task<ComputerPart?> UpdateByIdAsync(int computerPartId, ComputerPart updateComputerPart)
        {
            var computerPart = await FindByIdAsync(computerPartId);
            if (computerPart != null)
            {
                computerPart.PartGroupId = updateComputerPart.PartGroupId;
                computerPart.SerialNumber = updateComputerPart.SerialNumber;
                computerPart.ModelNumber = updateComputerPart.ModelNumber;

                await _context.SaveChangesAsync();

                computerPart = await FindByIdAsync(computerPartId);
            }
            return computerPart;
        }
    }
}
