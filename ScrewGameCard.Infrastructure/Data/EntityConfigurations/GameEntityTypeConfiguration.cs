using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfigurations
{
    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable(nameof(Game));

            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.RoomPasscode).HasMaxLength(50);

            builder.HasOne(g => g.Host)
                .WithMany(p => p.HostedGames)
                .HasForeignKey(g => g.HostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.Players)
                .WithOne(gp => gp.Game)
                .HasForeignKey(gp => gp.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Rounds)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}