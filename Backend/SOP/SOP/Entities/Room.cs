using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Building.Id")]
        public int BuildingId { get; set; }

        [Column(TypeName = "int")]
        public int RoomNumber { get; set; }

        public Building Building { get; set; }

        public List<Item> Items { get; set; }
    }
}
