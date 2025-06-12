using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace SOP.Entities
{
    public class Address
    {
        [Key]
        public int ZipCode { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Region { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Road { get; set; }
    }
}
