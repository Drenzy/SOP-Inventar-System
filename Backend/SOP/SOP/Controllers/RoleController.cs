using Microsoft.AspNetCore.Mvc;
using SOP.DTOs;
using SOP.Entities;
using SOP.Repositories;
using System.Data;

namespace SOP.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Role> role = await _roleRepository.GetAllAsync();

                List<RoleResponse> roleResponses = role.Select(
                    role => MapRoleToRoleResponse(role)).ToList();
                return Ok(roleResponses);
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
                var role = await _roleRepository.FindByIdAsync(Id);
                if (role == null)
                {
                    return NotFound();
                }

                return Ok(MapRoleToRoleResponse(role));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static RoleResponse MapRoleToRoleResponse(Role role)
        {
            RoleResponse response = new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
            };

            return response;
        }

        private static Role MapRoleRequestToRole(RoleRequest roleRequest)
        {
            return new Role
            {
                Name = roleRequest.Name,
                Description = roleRequest.Description,
            };
        }
    }
}
