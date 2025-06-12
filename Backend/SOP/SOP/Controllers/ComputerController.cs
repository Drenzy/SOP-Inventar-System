using Microsoft.AspNetCore.Mvc;
using SOP.Entities;
using SOP.Repositories;

namespace SOP.Controllers
{
    //create the route for our angular to call
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        // Injecting the IRoomRepository interface and storing it in a private readonly variable
        // This allows access to the room repository methods throughout the class
        private readonly IComputerRepository _computerRepository;

        // Initializes the controller with the address repository
        public ComputerController(IComputerRepository computerRepository)
        {
            // Assigning the repository to the private variable
            _computerRepository = computerRepository;
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
                var computers = await _computerRepository.GetAllAsync();

                //We are selecting and mapping the statusHistories we got from the database and making it into a list of computer responses
                List<ComputerResponse> computerResponses = computers.Select(
                    computer => MapComputerToComputerResponse(computer)).ToList();

                return Ok(computerResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ComputerRequest computerRequest)
        {
            try
            {
                Computer newComputer = MapComputerRequestToComputer(computerRequest);

                var computer = await _computerRepository.CreateAsync(newComputer);

                ComputerResponse computerResponse = MapComputerToComputerResponse(computer);

                return Ok(computerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpGet]
        [Route("{computerId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int computerId)
        {
            try
            {
                var computer = await _computerRepository.FindByIdAsync(computerId);
                if (computer == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapComputerToComputerResponse(computer));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør")]
        [HttpDelete]
        [Route("{computerId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int computerId)
        {
            try
            {
                var computer = await _computerRepository.DeleteByIdAsync(computerId);
                if (computer == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapComputerToComputerResponse(computer));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize("Admin")]
        [HttpDelete]
        [Route("deleteComputerAndItem/{computerId}")]
        public async Task<IActionResult> DeleteComputerAndItemByIdAsync([FromRoute] int computerId)
        {
            try
            {
                var computer = await _computerRepository.DeleteComputerAndItemByIdAsync(computerId);
                if (computer == null)
                {
                    return NotFound();
                }

                return Ok(MapComputerToComputerResponse(computer));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private ComputerResponse MapComputerToComputerResponse(Computer computer)
        {
            // Initialize the response object
            ComputerResponse response = new ComputerResponse
            {
                Id = computer.Id,
            };

            // Check if Item is not null
            if (computer.Item != null)
            {
                response.Item = new ComputerItemResponse
                {
                    Id = computer.Item.Id,
                    RoomId = computer.Item.RoomId,
                    ItemGroupId = computer.Item.ItemGroupId,
                    SerialNumber = computer.Item.SerialNumber
                };

                // Check if ItemGroup is not null
                if (computer.Item.ItemGroup != null)
                {
                    response.Item.ItemGroup = new ComputerItemItemGroupResponse
                    {
                        Id = computer.Item.ItemGroup.Id,
                        ItemTypeId = computer.Item.ItemGroup.ItemTypeId,
                        Manufacturer = computer.Item.ItemGroup.Manufacturer,
                        Price = computer.Item.ItemGroup.Price,
                        ModelName = computer.Item.ItemGroup.ModelName,
                        Quantity = computer.Item.ItemGroup.Quantity,
                        WarrantyPeriod = computer.Item.ItemGroup.WarrantyPeriod,
                    };

                    // Check if ItemType is not null
                    if (computer.Item.ItemGroup.ItemType != null)
                    {
                        response.Item.ItemGroup.ItemType = new ComputerItemGroupItemTypeResponse
                        {
                            Id = computer.Item.ItemGroup.ItemType.Id,
                            TypeName = computer.Item.ItemGroup.ItemType.TypeName,
                        };
                    }
                }
            }

            // Check if Computer_ComputerParts is not null
            if (computer.Computer_ComputerParts != null)
            {
                response.Computer_ComputerParts = computer.Computer_ComputerParts
                    .Select(computer_computerPart =>
                    {
                        var partResponse = new ComputerComputer_ComputerPartResponse
                        {
                            Id = computer_computerPart.Id,
                            ComputerId = computer_computerPart.ComputerId,
                            ComputerPartId = computer_computerPart.ComputerPartId
                        };

                        // Check if ComputerPart is not null
                        if (computer_computerPart.ComputerPart != null)
                        {
                            partResponse.ComputerPart = new ComputerComputer_ComputerPartComputerPartResponse
                            {
                                Id = computer_computerPart.ComputerPart.Id,
                                PartGroupId = computer_computerPart.ComputerPart.PartGroupId,
                                SerialNumber = computer_computerPart.ComputerPart.SerialNumber,
                                ModelNumber = computer_computerPart.ComputerPart.ModelNumber
                            };

                            // Check if PartGroup is not null
                            if (computer_computerPart.ComputerPart.PartGroup != null)
                            {
                                partResponse.ComputerPart.group = new ComputerComputer_ComputerPartComputerPartPartGroupResponse
                                {
                                    Id = computer_computerPart.ComputerPart.PartGroup.Id,
                                    PartTypeId = computer_computerPart.ComputerPart.PartGroup.PartTypeId,
                                    PartName = computer_computerPart.ComputerPart.PartGroup.PartName,
                                    Price = computer_computerPart.ComputerPart.PartGroup.Price,
                                    Manufacturer = computer_computerPart.ComputerPart.PartGroup.Manufacturer,
                                    WarrantyPeriod = computer_computerPart.ComputerPart.PartGroup.WarrantyPeriod,
                                    ReleaseDate = computer_computerPart.ComputerPart.PartGroup.ReleaseDate,
                                    Quantity = computer_computerPart.ComputerPart.PartGroup.Quantity
                                };

                                // Check if PartType is not null
                                if (computer_computerPart.ComputerPart.PartGroup.PartType != null)
                                {
                                    partResponse.ComputerPart.group.PartType = new ComputerPartTypeResponse
                                    {
                                        Id = computer_computerPart.ComputerPart.PartGroup.PartType.Id,
                                        partTypeName = computer_computerPart.ComputerPart.PartGroup.PartType.PartTypeName,
                                    };
                                }
                            }
                        }

                        return partResponse;
                    })
                    .ToList();
            }

            return response;
        }

        private Computer MapComputerRequestToComputer(ComputerRequest computerRequest)
        {
            return new Computer
            {
                Id = computerRequest.Id,
            };
        }
    }
}
