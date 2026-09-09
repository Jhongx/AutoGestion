namespace AutoGestion.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ToCostaRicaTime(this DateTime dateTime)
        {
            // 1. Normalizamos el Kind de entrada para que .NET/TimeZoneInfo no falle
            var utcDateTime = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            // 2. Buscamos la zona horaria (compatible con Linux/Fly.io)
            var timeZoneId = OperatingSystem.IsWindows()
                ? "Central America Standard Time"
                : "America/Costa_Rica";

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // 3. Convertimos de UTC a la hora local de Costa Rica
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);

            // 4. Forzamos el Kind a Unspecified para que Npgsql no discuta al momento de guardar
            return DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
        }
    }
}
