using Microsoft.AspNetCore.Mvc;
using SOP.Entities;
using SOP.Repositories;

namespace SOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpGet]
        public async Task<IActionResult> GetAllASync()
        {
            try
            {
                var statuses = await _statusRepository.GetAllAsync();

                List<StatusResponse> statusResponses = statuses.Select(
                    status => MapStatusToStatusResponse(status)).ToList();

                return Ok(statusResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] StatusRequest statusRequest)
        {
            try
            {
                Status newStatus = MapStatusRequestToStatus(statusRequest);

                var status = await _statusRepository.CreateAsync(newStatus);

                StatusResponse statusResponse = MapStatusToStatusResponse(status);

                return Ok(statusResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int Id)
        {
            try
            {
                var status = await _statusRepository.FindByIdAsync(Id);
                if (status == null)
                {
                    return NotFound();
                }

                return Ok(MapStatusToStatusResponse(status));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private StatusResponse MapStatusToStatusResponse(Status status)
        {
            return new StatusResponse
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        private Status MapStatusRequestToStatus(StatusRequest statusRequest)
        {
            return new Status
            {
                Name = statusRequest.Name,
            };
        }
    }
}
