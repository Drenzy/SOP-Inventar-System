using SOP.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.Archive.DTOs
{
    public class Archive_ItemResponse
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }

        public List<Archive_ItemStatusHistoryResponse> StatusHistories { get; set; }

    }

    public class Archive_ItemStatusHistoryResponse
    {
        public int Id { get; set; }
        public DateTime DeleteTime { get; set; }
        public int ItemId { get; set; }
        public int StatusId { get; set; }
        public DateTime StatusUpdateDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public string ArchiveNote { get; set; }

    }

}