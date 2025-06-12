

namespace SOP.Archive.Controllers
{
    //create the route for our angular to call
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_RequestController : ControllerBase
    {
        private readonly IArchive_RequestRepository _archive_requestRepository;

        public Archive_RequestController(IArchive_RequestRepository archive_requestRepository)
        {
            _archive_requestRepository = archive_requestRepository;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Archive_Request> archive_request = await _archive_requestRepository.GetAllAsync();

                List<Archive_RequestResponse> archive_requestResponses = archive_request.Select(
                    archive_request => MapArchive_RequestToArchive_RequestResponse(archive_request)).ToList();
                return Ok(archive_requestResponses);
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
                var archive_request = await _archive_requestRepository.FindByIdAsync(Id);
                if (archive_request == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_RequestToArchive_RequestResponse(archive_request));
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
                var archive_request = await _archive_requestRepository.DeleteByIdAsync(Id);
                if (archive_request == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_RequestToArchive_RequestResponse(archive_request));
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
                var request = await _archive_requestRepository.RestoreByIdAsync(Id);
                if (request == null)
                {
                    return NotFound();
                }
                RequestResponse requestResponse = new RequestResponse
                {
                    Id = request.Id,
                    Date = request.Date,
                    Item = request.Item,
                    Message = request.Message,
                    UserId = request.UserId,
                    Status = request.Status,
                    RecipientEmail = request.RecipientEmail,
                };

                return Ok(requestResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static Archive_RequestResponse MapArchive_RequestToArchive_RequestResponse(Archive_Request archive_Request)
        {
            Archive_RequestResponse response = new Archive_RequestResponse
            {
                Id = archive_Request.Id,
                DeleteTime = archive_Request.DeleteTime,
                Date = archive_Request.Date,
                Item = archive_Request.Item,
                Message = archive_Request.Message,
                UserId = archive_Request.UserId,
                Status = archive_Request.Status,
                RecipientEmail = archive_Request.RecipientEmail,
                ArchiveNote = archive_Request.ArchiveNote,
            };
            return response;
        }
    }
}
