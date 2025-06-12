using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IComputerRepository
    {
        Task<Computer> CreateAsync(Computer newComputer);
        Task<Computer?> DeleteByIdAsync(int computerId);
        Task<Computer?> DeleteComputerAndItemByIdAsync(int computerId);
        Task<Computer?> FindByIdAsync(int computerId);
        Task<List<Computer>> GetAllAsync();
    }
    public class ComputerRepository : IComputerRepository
    {
        private readonly DatabaseContext _context;

        // Initializes the repository with the database context for accessing data
        public ComputerRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Adds a new Computer, saves changes, retrieves, and returns it
        public async Task<Computer> CreateAsync(Computer newComputer)
        {
            _context.Computer.Add(newComputer);
            await _context.SaveChangesAsync();
            newComputer = await FindByIdAsync(newComputer.Id);
            return newComputer;
        }

        // Finds and deletes a Computer by ID, then saves changes and returns it
        public async Task<Computer?> DeleteByIdAsync(int computerId)
        {
            var computer = await FindByIdAsync(computerId);
            if (computer != null)
            {
                _context.Computer.Remove(computer);
                await _context.SaveChangesAsync();
            }
            return computer;
        }

        public async Task<Computer?> DeleteComputerAndItemByIdAsync(int computerId)
        {
            var computer = await FindByIdAsync(computerId);
            if (computer != null)
            {
                _context.Item.Remove(computer.Item);
                _context.Computer.Remove(computer);
                await _context.SaveChangesAsync();
            }
            return computer;
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Finds a Computer by ID, including related entities and returns it
        public async Task<Computer?> FindByIdAsync(int computerId)
        {
            return await _context.Computer
                .Include(c => c.Item)
                .Include(c => c.Computer_ComputerParts)
                .ThenInclude(ccp => ccp.ComputerPart)
                .ThenInclude(cp => cp.PartGroup)
                .ThenInclude(pg => pg.PartType)
                .FirstOrDefaultAsync(c => c.Id == computerId);
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Retrieves all Computers, including related entities and returns them
        public async Task<List<Computer>> GetAllAsync()
        {
            return await _context.Computer
                .Include(c => c.Item)
                .Include(c => c.Computer_ComputerParts)
                .ThenInclude(ccp => ccp.ComputerPart)
                .ThenInclude(cp => cp.PartGroup)
                .ThenInclude(pg => pg.PartType)
                .ToListAsync();
        }
    }
}
