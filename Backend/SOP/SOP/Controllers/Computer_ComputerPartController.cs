using Microsoft.AspNetCore.Mvc;
using SOP.Entities;
using SOP.Repositories;

namespace SOP.Controllers
{
    //create the route for our angular to call
    [Route("api/[controller]")]
    [ApiController]
    public class Computer_ComputerPartController : ControllerBase
    {
        // Injecting the IRoomRepository interface and storing it in a private readonly variable
        // This allows access to the room repository methods throughout the class
        private readonly IComputer_ComputerPartRepository _computer_ComputerPartRepository;

        // Initializes the controller with the address repository
        public Computer_ComputerPartController(IComputer_ComputerPartRepository computer_ComputerPartRepository)
        {
            // Assigning the repository to the private variable
            _computer_ComputerPartRepository = computer_ComputerPartRepository;
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
                var computer_ComputerParts = await _computer_ComputerPartRepository.GetAllAsync();

                //We are selecting and mapping the statusHistories we got from the database and making it into a list of computer_ComputerPart responses
                List<Computer_ComputerPartResponse> computer_ComputerPartResponses = computer_ComputerParts.Select(
                    computer_ComputerPart => MapComputer_ComputerPartToComputer_ComputerPartResponse(computer_ComputerPart)).ToList();

                return Ok(computer_ComputerPartResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Computer_ComputerPartRequest computer_ComputerPartRequest)
        {
            try
            {
                Computer_ComputerPart newComputer_ComputerPart = MapComputer_ComputerPartRequestToComputer_ComputerPart(computer_ComputerPartRequest);

                var computer_ComputerPart = await _computer_ComputerPartRepository.CreateAsync(newComputer_ComputerPart);

                Computer_ComputerPartResponse computer_ComputerPartResponse = MapComputer_ComputerPartToComputer_ComputerPartResponse(computer_ComputerPart);

                return Ok(computer_ComputerPartResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        [Route("{computer_ComputerPartId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int computer_ComputerPartId)
        {
            try
            {
                var computer_ComputerPart = await _computer_ComputerPartRepository.FindByIdAsync(computer_ComputerPartId);
                if (computer_ComputerPart == null)
                {
                    return NotFound();
                }

                return Ok(MapComputer_ComputerPartToComputer_ComputerPartResponse(computer_ComputerPart));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpDelete]
        [Route("{computer_ComputerPartId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int computer_ComputerPartId)
        {
            try
            {
                var computer_ComputerPart = await _computer_ComputerPartRepository.DeleteByIdAsync(computer_ComputerPartId);
                if (computer_ComputerPart == null)
                {
                    return NotFound();
                }

                return Ok(MapComputer_ComputerPartToComputer_ComputerPartResponse(computer_ComputerPart));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private Computer_ComputerPartResponse MapComputer_ComputerPartToComputer_ComputerPartResponse(Computer_ComputerPart computer_ComputerPart)
        {
            Computer_ComputerPartResponse response = new Computer_ComputerPartResponse
            {
                Id = computer_ComputerPart.Id,
                ComputerId = computer_ComputerPart.ComputerId,
                ComputerPartId = computer_ComputerPart.ComputerPartId
            };
            if(computer_ComputerPart.Computer != null)
            {
                response.Computer = new Computer_ComputerPartComputerResponse
                {
                    Id = computer_ComputerPart.Computer.Id,
                };
            }
            if(computer_ComputerPart.ComputerPart != null)
            {
                response.ComputerPart = new Computer_ComputerPartComputerPartResponse
                {
                    Id = computer_ComputerPart.ComputerPart.Id,
                    PartGroupId = computer_ComputerPart.ComputerPart.PartGroupId,
                    SerialNumber = computer_ComputerPart.ComputerPart.SerialNumber,
                    ModelNumber = computer_ComputerPart.ComputerPart.ModelNumber
                };
            }

            return response;
        }

        private Computer_ComputerPart MapComputer_ComputerPartRequestToComputer_ComputerPart(Computer_ComputerPartRequest computer_ComputerPartRequest)
        {
            return new Computer_ComputerPart
            {
                ComputerId = computer_ComputerPartRequest.ComputerId,
                ComputerPartId = computer_ComputerPartRequest.ComputerPartId
            };
        }
    }
}
