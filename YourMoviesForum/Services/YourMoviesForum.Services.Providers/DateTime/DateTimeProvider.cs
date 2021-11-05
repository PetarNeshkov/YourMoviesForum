namespace YourMoviesForum.Services.Providers.DateTime
{
    using ForumNet.Services.Providers.DateTime;
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.UtcNow;

    }
}
