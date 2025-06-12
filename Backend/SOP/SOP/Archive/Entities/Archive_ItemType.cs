namespace SOP.Archive.Entities
{
    public class Archive_ItemType
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string TypeName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }
    }
}
