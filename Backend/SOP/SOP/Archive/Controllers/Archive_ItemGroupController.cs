using Azure;
using SOP.Archive.DTOs;
using SOP.Entities;

namespace SOP.Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_ItemGroupController : ControllerBase
    {
        private readonly IArchive_ItemGroupRepository _archive_itemGroupRepository;

        public Archive_ItemGroupController(IArchive_ItemGroupRepository archive_itemGroupRepository)
        {
            _archive_itemGroupRepository = archive_itemGroupRepository;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var archive_itemGroups = await _archive_itemGroupRepository.GetAllAsync();

                List<Archive_ItemGroupResponse> archive_itemGroupResponses = archive_itemGroups.Select(
                    archive_itemGroup => MapArchive_ItemGroupToArchive_ItemGroupResponse(archive_itemGroup)).ToList();

                return Ok(archive_itemGroupResponses);
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
                var archive_itemGroup = await _archive_itemGroupRepository.FindByIdAsync(Id);
                if (archive_itemGroup == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemGroupToArchive_ItemGroupResponse(archive_itemGroup));
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
                var archive_itemGroup = await _archive_itemGroupRepository.DeleteByIdAsync(Id);
                if (archive_itemGroup == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemGroupToArchive_ItemGroupResponse(archive_itemGroup));
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

                var archive_itemGroup = await _archive_itemGroupRepository.RestoreByIdAsync(Id);
                if (archive_itemGroup == null)
                {
                    return NotFound();
                }

                ItemGroupResponse response = new ItemGroupResponse
                {
                    Id = archive_itemGroup.Id,
                    ItemTypeId = archive_itemGroup.ItemTypeId,
                    ModelName = archive_itemGroup.ModelName,
                    Price = archive_itemGroup.Price,
                    Manufacturer = archive_itemGroup.Manufacturer,
                    WarrantyPeriod = archive_itemGroup.WarrantyPeriod,
                    Quantity = archive_itemGroup.Quantity,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private Archive_ItemGroupResponse MapArchive_ItemGroupToArchive_ItemGroupResponse(Archive_ItemGroup archive_itemGroup)
        {
            Archive_ItemGroupResponse response = new Archive_ItemGroupResponse
            {
                Id = archive_itemGroup.Id,
                DeleteTime = archive_itemGroup.DeleteTime,
                ItemTypeId = archive_itemGroup.ItemTypeId,
                ModelName = archive_itemGroup.ModelName,
                Price = archive_itemGroup.Price,
                Manufacturer = archive_itemGroup.Manufacturer,
                WarrantyPeriod = archive_itemGroup.WarrantyPeriod,
                Quantity = archive_itemGroup.Quantity,
                ArchiveNote = archive_itemGroup.ArchiveNote,
            };
            return response;
        }

    }
}
