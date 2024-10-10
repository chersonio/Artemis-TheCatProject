using Artemis.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<CatEntity> Cat { get; set; }
        public DbSet<CatTag> CatTag { get; set; }
        public DbSet<TagEntity> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatEntity>()
                .HasMany(c => c.Tags)
                .WithMany(t => t.Cats)
                .UsingEntity<CatTag>();

            modelBuilder.Entity<CatEntity>()
                .Property(c => c.Created)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<TagEntity>()
                .Property(c => c.Created)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
