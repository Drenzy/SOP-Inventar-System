namespace SOP.DTOs
{
    public class PartGroupResponse
    {
        public int Id { get; set; }
        public string PartName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string WarrantyPeriod { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int Quantity { get; set; }
        public int PartTypeId { get; set; }
        public PartGroupPartTypeResponse PartType { get; set; }
    }

    public class PartGroupPartTypeResponse
    {
        public int Id { get; set; }
        public string PartTypeName { get; set; } = string.Empty;
    }
}
