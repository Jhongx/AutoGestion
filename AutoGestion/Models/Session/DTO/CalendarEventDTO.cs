namespace AutoGestion.Models.Session.DTO
{
    public class CalendarEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty; // Formato ISO: YYYY-MM-DD
        public string Url { get; set; } = string.Empty;
        public string Color { get; set; } = "#0d6efd"; // Opcional: Para personalizar por ServiceType
    }
}
