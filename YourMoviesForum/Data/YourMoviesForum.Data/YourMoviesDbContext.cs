using Microsoft.EntityFrameworkCore;
using YourMoviesForum.Data.Configuration;
using YourMoviesForum.Data.Models;

namespace YourMoviesForum
{
    public class YourMoviesDbContext:DbContext
    {
        public YourMoviesDbContext()
        {
        }

        public YourMoviesDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; init; }

        public DbSet<Post> Posts { get; init; }

        public DbSet<Tag> Tags { get; init; }

        public DbSet<Reply> Replies { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DataSettings.ConnectionString);
            }
        }
        //protected override void OnModelCreating(ModelBuilder builder)
          //      => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
