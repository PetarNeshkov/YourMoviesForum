using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Common.Models;
using YourMoviesForum.Data.Models;

namespace YourMoviesForum
{
    public class YourMoviesDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
     
        public YourMoviesDbContext(DbContextOptions<YourMoviesDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; init; }

        public DbSet<Post> Posts { get; init; }

        public DbSet<Tag> Tags { get; init; }

        public DbSet<Reply> Replies { get; init; }

        public DbSet<PostReaction> PostReactions { get; init; }

        public DbSet<ReplyReaction> ReplyReactions { get; init; }

        public override int SaveChanges() => SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditforRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            =>SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditforRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //Disable cascade delete
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
    }
}
