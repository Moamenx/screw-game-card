using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class GamePlayerEntityTypeConfiguration : IEntityTypeConfiguration<GamePlayer>
    {
        public void Configure(EntityTypeBuilder<GamePlayer> builder)
        {
            builder.ToTable(nameof(GamePlayer));

            builder.HasOne(gp => gp.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(gp => gp.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gp => gp.Player)
                .WithMany(p => p.GamePlayers)
                .HasForeignKey(gp => gp.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(gp => gp.RoundParticipations)
                .WithOne(grp => grp.GamePlayer)
                .HasForeignKey(grp => grp.GamePlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}