namespace SOP.DTOs
{
    public class Archive_ItemTypeResponse
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }
    }
}
