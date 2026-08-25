namespace AutoGestion.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ToCostaRicaTime(this DateTime utcDateTime)
        {
            // En contenedores Linux (como Fly.io), la zona horaria IANA es "America/Costa_Rica"
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }
    }
}
