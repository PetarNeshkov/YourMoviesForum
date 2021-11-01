using Microsoft.EntityFrameworkCore;
using YourMoviesForum;

namespace Program
{
    public class Program
    {
        public static void Main()
        {
            using var db = new YourMoviesDbContext();

            db.Database.EnsureDeleted();
            db.Database.Migrate();
        }
    }
}
