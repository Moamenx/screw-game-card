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
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<GameRoundPlayer> GameRoundPlayers { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //public DbSet<Leaderboard> Leaderboards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new FriendshipEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new GameEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new GamePlayerEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new RoundEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new GameRoundPlayerEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new StatisticsEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new LeaderboardEntityTypeConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScrewGameCardDbContext).Assembly);
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

