using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync();
        Task<Loan> CreateAsync(Loan loan);
        Task<Loan> FindByIdAsync(int id);
        Task<Loan> UpdateByIdAsync(int id, Loan loan);
        Task<Archive_Loan> ArchiveByIdAsync(int id, string archiveNote);
    }
    public class LoanRepository : ILoanRepository
    {
        private readonly DatabaseContext _context;

        public LoanRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loan
                .Include(x => x.User)
                .Include(x => x.Item)
                .ToListAsync();
        }

        public async Task<Loan> CreateAsync(Loan newLoan)
        {
            _context.Loan.Add(newLoan);
            await _context.SaveChangesAsync();
            return newLoan;
        }

        public async Task<Loan?> FindByIdAsync(int loanId)
        {
            return await _context.Loan
                .Include(x => x.User)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == loanId);
        }

        public async Task<Loan> UpdateByIdAsync(int id, Loan newLoan)
        {
            var loan = await FindByIdAsync(id);

            if (loan != null)
            {
                loan.ReturnDate = newLoan.ReturnDate;
                loan.LoanDate = newLoan.LoanDate;
                loan.UserId = newLoan.UserId;
                loan.ItemId = newLoan.ItemId;

                await _context.SaveChangesAsync();

                loan = await FindByIdAsync(id);
            }
            return loan;
        }

        public async Task<Archive_Loan> ArchiveByIdAsync(int loanId, string archiveNote)
        {
            var loan = await FindByIdAsync(loanId);
            if (loan == null)
            {
                return null;
            }
            Archive_Loan archiveLoan = new Archive_Loan
            {
                Id = loan.Id,
                DeleteTime = DateTime.Now,
                UserId = loan.UserId,
                ItemId = loan.ItemId,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                ArchiveNote = archiveNote,
            };
            if (loan != null)
            {
                _context.Archive_Loan.Add(archiveLoan);
                _context.Loan.Remove(loan);
                await _context.SaveChangesAsync();
            }
            return archiveLoan;
        }
    }
}
