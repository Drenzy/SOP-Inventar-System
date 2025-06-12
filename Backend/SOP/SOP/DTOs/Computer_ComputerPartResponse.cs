using SOP.Entities;

namespace SOP.DTOs
{
    public class Computer_ComputerPartResponse
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int ComputerPartId { get; set; }

        public Computer_ComputerPartComputerResponse Computer { get; set; }
        public Computer_ComputerPartComputerPartResponse ComputerPart { get; set; }
    }

    public class Computer_ComputerPartComputerResponse
    {
        public int Id { get; set; }
    }

    public class Computer_ComputerPartComputerPartResponse
    {
        public int Id { get; set; }
        public int PartGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;

    }
}
