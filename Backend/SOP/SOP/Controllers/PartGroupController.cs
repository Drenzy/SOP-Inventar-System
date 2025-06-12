using Azure;
using Microsoft.AspNetCore.Mvc;
using SOP.Entities;
using SOP.Repositories;

namespace SOP.Controllers
{
    //create the route for our angular to call
    [Route("api/[controller]")]
    [ApiController]
    public class PartGroupController : ControllerBase
    {
        // Injecting the IRoomRepository interface and storing it in a private readonly variable
        // This allows access to the room repository methods throughout the class
        private readonly IPartGroupRepository _partGroupRepository;

        // Initializes the controller with the address repository
        public PartGroupController(IPartGroupRepository partGroupRepository)
        {
            // Assigning the repository to the private variable
            _partGroupRepository = partGroupRepository;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //We use a try methode to get an answer if anything goes wrong,
            //we can print a message and not let the user completle blind over the problem
            try
            {
                //We are using the GetAllAsync methode from the Interface and set it into a var
                var partGroups = await _partGroupRepository.GetAllAsync();

                //We are selecting and mapping the statusHistories we got from the database and making it into a list of partGroup responses
                List<PartGroupResponse> partGroupResponses = partGroups.Select(
                    partGroup => MapPartGroupToPartGroupResponse(partGroup)).ToList();

                return Ok(partGroupResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PartGroupRequest partGroupRequest)
        {
            try
            {
                PartGroup newPartGroup = MapPartGroupRequestToPartGroup(partGroupRequest);

                var partGroup = await _partGroupRepository.CreateAsync(newPartGroup);

                PartGroupResponse partGroupResponse = MapPartGroupToPartGroupResponse(partGroup);

                return Ok(partGroupResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        [Route("{partGroupId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int partGroupId)
        {
            try
            {
                var partGroup = await _partGroupRepository.FindByIdAsync(partGroupId);
                if (partGroup == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapPartGroupToPartGroupResponse(partGroup));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpPut]
        [Route("{partGroupId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int partGroupId, [FromBody] PartGroupRequest partGroupRequest)
        {
            try
            {
                var updatePartGroup = MapPartGroupRequestToPartGroup(partGroupRequest);

                var partGroup = await _partGroupRepository.UpdateByIdAsync(partGroupId, updatePartGroup);

                if (partGroup == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapPartGroupToPartGroupResponse(partGroup));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private static PartGroupResponse MapPartGroupToPartGroupResponse(PartGroup partGroup)
        {
            PartGroupResponse response = new PartGroupResponse
            {
                Id = partGroup.Id,
                PartName = partGroup.PartName,
                Price = partGroup.Price,
                Manufacturer = partGroup.Manufacturer,
                WarrantyPeriod = partGroup.WarrantyPeriod,
                ReleaseDate = partGroup.ReleaseDate,
                Quantity = partGroup.Quantity,
                PartTypeId = partGroup.PartTypeId,
            };
            if (partGroup.PartType != null)
            {
                response.PartType = new PartGroupPartTypeResponse
                {
                    Id = partGroup.PartType.Id,
                    PartTypeName = partGroup.PartType.PartTypeName,
                };
            }
            return response;
        }

        private PartGroup MapPartGroupRequestToPartGroup(PartGroupRequest partGroupRequest)
        {
            return new PartGroup
            {
                PartName = partGroupRequest.PartName,
                Price = partGroupRequest.Price,
                Manufacturer = partGroupRequest.Manufacturer,
                WarrantyPeriod= partGroupRequest.WarrantyPeriod,
                ReleaseDate = partGroupRequest.ReleaseDate,
                Quantity = partGroupRequest.Quantity,
                PartTypeId = partGroupRequest.PartTypeId,
            };
        }
    }
}
