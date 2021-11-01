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

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Reply> Replies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
                => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
