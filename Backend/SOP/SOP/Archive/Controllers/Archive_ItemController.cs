namespace SOP.Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_ItemController : ControllerBase
    {
        private readonly IArchive_ItemRepository _archive_itemRepository;

        public Archive_ItemController(IArchive_ItemRepository archive_itemRepository)
        {
            _archive_itemRepository = archive_itemRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var archive_items = await _archive_itemRepository.GetAllAsync();

                List<Archive_ItemResponse> archive_itemResponses = archive_items.Select(
                    archive_item => MapArchive_ItemToArchive_ItemResponse(archive_item)).ToList();

                return Ok(archive_itemResponses);
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
                var archive_item = await _archive_itemRepository.FindByIdAsync(Id);
                if (archive_item == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemToArchive_ItemResponse(archive_item));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int Id)
        {
            try
            {
                var archive_item = await _archive_itemRepository.DeleteByIdAsync(Id);
                if (archive_item == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_ItemToArchive_ItemResponse(archive_item));
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

                var archive_item = await _archive_itemRepository.RestoreByIdAsync(Id);
                if (archive_item == null)
                {
                    return NotFound();
                }

                ItemResponse response = new ItemResponse
                {
                    Id = archive_item.Id,
                    RoomId = archive_item.RoomId,
                    ItemGroupId = archive_item.ItemGroupId,
                    SerialNumber = archive_item.SerialNumber,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private Archive_ItemResponse MapArchive_ItemToArchive_ItemResponse(Archive_Item archive_item)
        {
            Archive_ItemResponse response = new Archive_ItemResponse
            {
                Id = archive_item.Id,
                RoomId = archive_item.RoomId,
                DeleteTime = archive_item.DeleteTime,
                ItemGroupId = archive_item.ItemGroupId,
                SerialNumber = archive_item.SerialNumber,
                ArchiveNote = archive_item.ArchiveNote,
            };

            if (archive_item.StatusHistories != null)
            {
                response.StatusHistories =
                    archive_item.StatusHistories.Select(statusHistory =>
                    {
                        var statisHistoryResponse = new Archive_ItemStatusHistoryResponse
                        {
                            Id = statusHistory.Id,
                            DeleteTime = statusHistory.DeleteTime,
                            ItemId = statusHistory.ItemId,
                            StatusId = statusHistory.StatusId,
                            StatusUpdateDate = statusHistory.StatusUpdateDate,
                            Note = statusHistory.Note,
                            ArchiveNote = statusHistory.ArchiveNote,
                        };
                        return statisHistoryResponse;
                    }).ToList();
            }
            return response;
        }
    }
}
