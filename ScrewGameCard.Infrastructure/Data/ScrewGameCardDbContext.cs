using Microsoft.EntityFrameworkCore;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Infrastructure.Data.EntityConfiguration;

namespace ScrewGameCard.Infrastructure.Data
{
    public class ScrewGameCardDbContext(DbContextOptions<ScrewGameCardDbContext> options) : DbContext(options)
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendshipEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var now = DateTime.Now;
            foreach (var e in ChangeTracker.Entries<IAuditable>())
                if (e.State == EntityState.Added)
                    e.Entity.CreatedDate = now;
                else if (e.State == EntityState.Modified) e.Entity.ModifiedDate = now;

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

