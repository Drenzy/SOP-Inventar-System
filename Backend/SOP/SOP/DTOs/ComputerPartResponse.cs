using SOP.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class ComputerPartResponse
    {
        public int Id { get; set; }
        public int PartGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;

        public ComputerPartPartGroupResponse Group { get; set; }
        public ComputerPartComputer_ComputerPartResponse Computer_ComputerPart { get; set; }
    }

    public class ComputerPartPartGroupResponse
    {
        public int Id { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string Compatibility { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string WarrantyPeriod { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int Quantity { get; set; }
        public int PartTypeId { get; set; }

        public ComputerPartPartGroupPartTypeResponse PartType { get; set; }
    }

    public class ComputerPartPartGroupPartTypeResponse
    {
        public int Id { get; set; }
        public string PartTypeName {  get; set; } = string.Empty;
    }

    public class ComputerPartComputer_ComputerPartResponse
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
    }
}
