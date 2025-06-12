using Microsoft.EntityFrameworkCore;
using SOP.Database;
using SOP.Entities;

namespace SOP.Repositories
{
    public interface IArchive_LoanRepository
    {
        Task<List<Archive_Loan>> GetAllAsync();
        Task<Archive_Loan> FindByIdAsync(int id);
        Task<Archive_Loan> DeleteByIdAsync(int id);
        Task<Loan> RestoreByIdAsync(int archiveLoanId);
    }
    public class Archive_LoanRepository : IArchive_LoanRepository
    {
        private readonly DatabaseContext _context;

        public Archive_LoanRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Archive_Loan>> GetAllAsync()
        {
            // Er ikke sikker med det her include stuff. 
            return await _context.Archive_Loan.ToListAsync();
        }

        public async Task<Archive_Loan?> FindByIdAsync(int loanId)
        {
            return await _context.Archive_Loan
                .FirstOrDefaultAsync(x => x.Id == loanId);
        }

        public async Task<Archive_Loan> DeleteByIdAsync(int loanId)
        {
            var loan = await FindByIdAsync(loanId);
            if (loan != null)
            {
                _context.Archive_Loan.Remove(loan);
                await _context.SaveChangesAsync();
            }
            return loan;
        }

        public async Task<Loan> RestoreByIdAsync(int archiveLoanId)
        {
            Archive_Loan archiveLoan = await FindByIdAsync(archiveLoanId);
            if (archiveLoan == null)
            {
                return null;
            }

            Loan loan = new Loan
            {
                Id = archiveLoan.Id,
                LoanDate = archiveLoan.LoanDate,
                ReturnDate = archiveLoan.ReturnDate,
                ItemId = archiveLoan.ItemId,
                UserId = archiveLoan.UserId,
            };

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await RestoreLoanAsync(loan, archiveLoan);
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
                await RestoreLoanAsync(loan, archiveLoan);
            }

            return loan;
        }

        private async Task RestoreLoanAsync(Loan loan, Archive_Loan archiveLoan)
        {
            // Skip IDENTITY_INSERT for in-memory database
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Loan ON");
            }

            _context.Loan.Add(loan);
            await _context.SaveChangesAsync();

            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Loan OFF");
            }

            _context.Archive_Loan.Remove(archiveLoan);
            await _context.SaveChangesAsync();
        }
    }
}
