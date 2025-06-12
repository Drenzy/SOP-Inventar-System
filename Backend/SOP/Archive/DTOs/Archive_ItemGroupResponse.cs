using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class Archive_ItemGroupResponse
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string WarrantyPeriod { get; set; } = string.Empty;
        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }
    }

}
