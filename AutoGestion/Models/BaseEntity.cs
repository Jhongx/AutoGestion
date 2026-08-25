namespace AutoGestion.Models
{
    public abstract class BaseEntity
    {
        public int CompanyId { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica"));
        public DateTime? UpdatedAt { get; set; }
    }
}
