

namespace SOP.Archive.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class Archive_UserController : ControllerBase
    {
        private readonly IArchive_UserRepository _archive_UserRepository;
        private readonly IJwtUtils _jwtUtils;

        public Archive_UserController(IArchive_UserRepository archive_UserRepository, IJwtUtils jwtUtils)
        {
            _archive_UserRepository = archive_UserRepository;
            _jwtUtils = jwtUtils;
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Archive_User> archive_user = await _archive_UserRepository.GetAllAsync();

                List<Archive_UserResponse> archive_userResponses = archive_user
                    .Select(archive_user => MapArchive_UserToArchive_UserResponse(archive_user))
                    .ToList();

                return Ok(archive_userResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin")]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int Id)
        {
            try
            {
                var archive_user = await _archive_UserRepository.FindByIdAsync(Id);
                if (archive_user == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_UserToArchive_UserResponse(archive_user));
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
                var archive_user = await _archive_UserRepository.DeleteByIdAsync(Id);
                if (archive_user == null)
                {
                    return NotFound();
                }

                return Ok(MapArchive_UserToArchive_UserResponse(archive_user));
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
                var user = await _archive_UserRepository.RestoreByIdAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }

                UserResponse userResponse = new UserResponse
                {
                    Id = user.Id,
                    RoleId = user.RoleId,
                    Email = EncryptionHelper.Decrypt(user.Email),
                    Name = user.Name,
                    Password = user.Password,
                    TwoFactorAuthentication = user.TwoFactorAuthentication,
                };

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static Archive_UserResponse MapArchive_UserToArchive_UserResponse(Archive_User archive_user)
        {
            Archive_UserResponse response = new Archive_UserResponse
            {
                Id = archive_user.Id,
                DeleteTime = archive_user.DeleteTime,
                RoleId = archive_user.RoleId,
                Email = EncryptionHelper.Decrypt(archive_user.Email),
                Name = archive_user.Name,
                Password = archive_user.Password,
                TwoFactorAuthentication = archive_user.TwoFactorAuthentication,
                ArchiveNote = archive_user.ArchiveNote,
            };
            return response;
        }
    }
}
