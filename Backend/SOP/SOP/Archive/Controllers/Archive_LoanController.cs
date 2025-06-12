

namespace SOP.Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_LoanController : ControllerBase
    {
        private readonly IArchive_LoanRepository _archive_loanRepository;

        public Archive_LoanController(IArchive_LoanRepository archive_loanRepository)
        {
            _archive_loanRepository = archive_loanRepository;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Archive_Loan> archive_loan = await _archive_loanRepository.GetAllAsync();

                List<Archive_LoanResponse> archive_roleResponses = archive_loan.Select(
                    archive_loan => MapArchive_LoanToArchive_LoanResponse(archive_loan)).ToList();
                return Ok(archive_roleResponses);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int Id)
        {
            try
            {
                var archive_loan = await _archive_loanRepository.FindByIdAsync(Id);
                if (archive_loan == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_LoanToArchive_LoanResponse(archive_loan));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin")]
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int Id)
        {
            try
            {
                var archive_loan = await _archive_loanRepository.DeleteByIdAsync(Id);
                if (archive_loan == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_LoanToArchive_LoanResponse(archive_loan));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin")]
        [HttpPost]
        [Route("RestoreById/{Id}")]
        public async Task<IActionResult> RestoreByIdAsync([FromRoute] int Id)
        {
            try
            {
                var loan = await _archive_loanRepository.RestoreByIdAsync(Id);
                if (loan == null)
                {
                    return NotFound();
                }

                LoanResponse loanResponse = new LoanResponse
                {
                    Id = loan.Id,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    ItemId = loan.ItemId,
                    UserId = loan.UserId,
                };

                return Ok(loanResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static Archive_LoanResponse MapArchive_LoanToArchive_LoanResponse(Archive_Loan archive_loan)
        {
            Archive_LoanResponse response = new Archive_LoanResponse
            {
                Id = archive_loan.Id,
                DeleteTime = archive_loan.DeleteTime,
                LoanDate = archive_loan.LoanDate,
                ReturnDate = archive_loan.ReturnDate,
                ItemId = archive_loan.ItemId,
                UserId = archive_loan.UserId,
                ArchiveNote = archive_loan.ArchiveNote,
            };

            return response;
        }
    }
}
