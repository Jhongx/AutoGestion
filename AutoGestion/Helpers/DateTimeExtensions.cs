namespace AutoGestion.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ToCostaRicaTime(this DateTime utcDateTime)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);

            // Forzamos el Kind para que Npgsql no discuta al momento de serializar
            return DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
        }
    }
}
