namespace SOP.DTOs
{
    public class ComputerResponse
    {
        public int Id { get; set; }

        public ComputerItemResponse Item { get; set; }
        public List<ComputerComputer_ComputerPartResponse> Computer_ComputerParts { get; set; }
    }

    public class ComputerItemResponse
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;

        public ComputerItemItemGroupResponse ItemGroup { get; set; }
    }

    public class ComputerItemItemGroupResponse
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string WarrantyPeriod { get; set; } = string.Empty;

        public ComputerItemGroupItemTypeResponse ItemType { get; set; }
    }

    public class ComputerItemGroupItemTypeResponse
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
    }

    public class ComputerComputer_ComputerPartResponse
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int ComputerPartId { get; set; }
        public ComputerComputer_ComputerPartComputerPartResponse ComputerPart { get; set; }
    }

    public class ComputerComputer_ComputerPartComputerPartResponse
    {
        public int Id { get; set; }
        public int PartGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public ComputerComputer_ComputerPartComputerPartPartGroupResponse group { get; set; }
    }

    public class ComputerComputer_ComputerPartComputerPartPartGroupResponse
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

        public ComputerPartTypeResponse PartType { get; set; }
    }

    public class ComputerPartTypeResponse
    {
        public int Id { get; set; }
        public string partTypeName { get; set; } = string.Empty;
    }
}
