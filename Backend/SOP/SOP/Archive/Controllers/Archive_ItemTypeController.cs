
namespace SOP.Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_ItemTypeController : ControllerBase
    {
        private readonly IArchive_ItemTypeRepository _archive_itemTypeRepository;

        public Archive_ItemTypeController(IArchive_ItemTypeRepository archive_itemTypeRepository)
        {
            _archive_itemTypeRepository = archive_itemTypeRepository;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var archive_itemTypes = await _archive_itemTypeRepository.GetAllAsync();

                List<Archive_ItemTypeResponse> archive_itemTypeResponses = archive_itemTypes.Select(
                    archive_itemType => MapArchive_ItemTypeToArchive_ItemTypeResponse(archive_itemType)).ToList();

                return Ok(archive_itemTypeResponses);
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
                var archive_itemType = await _archive_itemTypeRepository.FindByIdAsync(Id);
                if (archive_itemType == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemTypeToArchive_ItemTypeResponse(archive_itemType));
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
                var archive_itemType = await _archive_itemTypeRepository.DeleteByIdAsync(Id);
                if (archive_itemType == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemTypeToArchive_ItemTypeResponse(archive_itemType));
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

                var archive_itemType = await _archive_itemTypeRepository.RestoreByIdAsync(Id);
                if (archive_itemType == null)
                {
                    return NotFound();
                }

                ItemTypeResponse response = new ItemTypeResponse
                {
                    Id = archive_itemType.Id,
                    TypeName = archive_itemType.TypeName,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        private Archive_ItemTypeResponse MapArchive_ItemTypeToArchive_ItemTypeResponse(Archive_ItemType archive_itemType)
        {
            return new Archive_ItemTypeResponse
            {
                Id = archive_itemType.Id,
                DeleteTime = archive_itemType.DeleteTime,
                TypeName = archive_itemType.TypeName,
                ArchiveNote = archive_itemType.ArchiveNote,
            };
        }
    }
}
