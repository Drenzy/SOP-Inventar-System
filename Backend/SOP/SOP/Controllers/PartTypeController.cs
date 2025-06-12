namespace SOP.Controllers
{
    //create the route for our angular to call
    [Route("api/[controller]")]
    [ApiController]
    public class PartTypeController : ControllerBase
    {
        // Injecting the IRoomRepository interface and storing it in a private readonly variable
        // This allows access to the room repository methods throughout the class
        private readonly IPartTypeRepository _partTypeRepository;

        // Initializes the controller with the address repository
        public PartTypeController(IPartTypeRepository partTypeRepository)
        {
            // Assigning the repository to the private variable
            _partTypeRepository = partTypeRepository;
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
                var partTypes = await _partTypeRepository.GetAllAsync();

                //We are selecting and mapping the statusHistories we got from the database and making it into a list of partType responses
                List<PartTypeResponse> partTypeResponses = partTypes.Select(
                    partType => MapPartTypeToPartTypeResponse(partType)).ToList();

                return Ok(partTypeResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PartTypeRequest partTypeRequest)
        {
            try
            {
                PartType newPartType = MapPartTypeRequestToPartType(partTypeRequest);

                var partType = await _partTypeRepository.CreateAsync(newPartType);

                PartTypeResponse partTypeResponse = MapPartTypeToPartTypeResponse(partType);

                return Ok(partTypeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        [Route("{partTypeId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int partTypeId)
        {
            try
            {
                var partType = await _partTypeRepository.FindByIdAsync(partTypeId);
                if (partType == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapPartTypeToPartTypeResponse(partType));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //[Authorize("Admin", "Instruktør")]
        [AllowAnonymous]
        [HttpPut]
        [Route("{partTypeId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int partTypeId, [FromBody] PartTypeRequest partTypeRequest)
        {
            try
            {
                var updatePartType = MapPartTypeRequestToPartType(partTypeRequest);

                var partType = await _partTypeRepository.UpdateByIdAsync(partTypeId, updatePartType);

                if (partType == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapPartTypeToPartTypeResponse(partType));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private PartTypeResponse MapPartTypeToPartTypeResponse(PartType partType)
        {
            return new PartTypeResponse
            {
                Id = partType.Id,
                PartTypeName = partType.PartTypeName,
            };
        }

        private PartType MapPartTypeRequestToPartType(PartTypeRequest partTypeRequest)
        {
            return new PartType
            {
                PartTypeName = partTypeRequest.PartTypeName,
            };
        }
    }
}
