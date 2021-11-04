using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YourMoviesForum.Data.Common.Models;
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

        public override int SaveChanges() => SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditforRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        //Disable cascade delete
         protected override void OnModelCreating(ModelBuilder builder)
        {
            var entityTypes = builder.Model.GetEntityTypes().ToList();

            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys()
                .Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        //To apply abstractly DateTime everywhere with IAuditInfo when something is changed
        private void ApplyAuditforRules()
        {
            var changedEntries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State==EntityState.Added && entity.CreatedOn==default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //      => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
