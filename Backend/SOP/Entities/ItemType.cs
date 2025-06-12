namespace SOP.Entities
{
    public class ItemType
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string TypeName { get; set; }
    }
}
