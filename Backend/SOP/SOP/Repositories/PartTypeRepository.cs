namespace SOP.Repositories
{
    public interface IPartTypeRepository
    {
        Task<PartType> CreateAsync(PartType newPartType);
        Task<PartType?> UpdateByIdAsync(int partTypeId, PartType updatePartType);
        Task<PartType?> FindByIdAsync(int partTypeId);
        Task<List<PartType>> GetAllAsync();
    }

    public class PartTypeRepository : IPartTypeRepository
    {
        private readonly DatabaseContext _context;

        // Initializes the repository with the database context for accessing data
        public PartTypeRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Adds a new PartType, saves changes, retrieves, and returns it
        public async Task<PartType> CreateAsync(PartType newPartType)
        {
            _context.PartType.Add(newPartType);
            await _context.SaveChangesAsync();
            newPartType = await FindByIdAsync(newPartType.Id);
            return newPartType;
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Finds a PartType by ID, including related entities and returns it
        public async Task<PartType?> FindByIdAsync(int partTypeId)
        {
            return await _context.PartType.FindAsync(partTypeId);
        }

        // Please refer to the class diagram or ER diagram for entity relationships
        // Retrieves all PartTypes, including related entities and returns them
        public async Task<List<PartType>> GetAllAsync()
        {
            return await _context.PartType.ToListAsync();
        }

        // Updates a PartType by ID and returns the updated entity. 
        public async Task<PartType?> UpdateByIdAsync(int partTypeId, PartType updatePartType)
        {
            var partType = await FindByIdAsync(partTypeId);
            if (partType != null)
            {
                partType.PartTypeName = updatePartType.PartTypeName;

                await _context.SaveChangesAsync();

                partType = await FindByIdAsync(partTypeId);
            }
            return partType;
        }
    }
}
