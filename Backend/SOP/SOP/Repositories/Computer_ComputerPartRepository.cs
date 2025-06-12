using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IComputer_ComputerPartRepository
    {
        Task<Computer_ComputerPart> CreateAsync(Computer_ComputerPart newComputer_ComputerPart);
        Task<Computer_ComputerPart?> DeleteByIdAsync(int computer_ComputerPartId);
        Task<Computer_ComputerPart?> FindByIdAsync(int computer_ComputerPartId);
        Task<List<Computer_ComputerPart>> GetAllAsync();
    }
    public class Computer_ComputerPartRepository : IComputer_ComputerPartRepository
    {
        private readonly DatabaseContext _context;

        public Computer_ComputerPartRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Adds a new Computer_ComputerPart, saves changes, retrieves, and returns it
        public async Task<Computer_ComputerPart> CreateAsync(Computer_ComputerPart newComputer_ComputerPart)
        {
            _context.Computer_ComputerPart.Add(newComputer_ComputerPart);
            await _context.SaveChangesAsync();
            newComputer_ComputerPart = await FindByIdAsync(newComputer_ComputerPart.Id);
            return newComputer_ComputerPart;
        }

        // Finds and deletes a Computer_ComputerPart by ID, then saves changes and returns it
        public async Task<Computer_ComputerPart?> DeleteByIdAsync(int computer_ComputerPartId)
        {
            var computer_ComputerPart = await FindByIdAsync(computer_ComputerPartId);
            if (computer_ComputerPart != null)
            {
                _context.Computer_ComputerPart.Remove(computer_ComputerPart);
                await _context.SaveChangesAsync();
            }
            return computer_ComputerPart;
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Finds a Computer_ComputerPart by ID, including related entities and returns it
        public async Task<Computer_ComputerPart?> FindByIdAsync(int computer_ComputerPartId)
        {
            return await _context.Computer_ComputerPart
                .Include(ccp => ccp.Computer)
                .ThenInclude(c => c.Item)
                .Include(ccp => ccp.ComputerPart)
                .ThenInclude(cp => cp.PartGroup)
                .FirstOrDefaultAsync(ccp => ccp.Id == computer_ComputerPartId);
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Retrieves all Computer_ComputerParts, including related entities and returns them
        public async Task<List<Computer_ComputerPart>> GetAllAsync()
        {
            return await _context.Computer_ComputerPart
                .Include(ccp => ccp.Computer)
                .ThenInclude(c => c.Item)
                .Include(ccp => ccp.ComputerPart)
                .ThenInclude(cp => cp.PartGroup)
                .ToListAsync();
        }
    }
}
