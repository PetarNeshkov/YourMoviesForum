namespace YourMoviesForum.Services.Providers.DateTime
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public string Now() => DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm");

    }
}
