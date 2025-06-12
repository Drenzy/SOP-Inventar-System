using SOP.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class StatusHistoryResponse
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int StatusId { get; set; }
        public DateTime StatusUpdateDate { get; set; }
        public string Note { get; set; } = string.Empty;

        public StatusHistoryStatusResponse Status { get; set; }
        public StatusItemResponse Item { get; set; }
    }

    public class StatusHistoryStatusResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class StatusItemResponse
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
    }
}
