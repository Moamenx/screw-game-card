using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class GameRoundPlayerEntityTypeConfiguration : IEntityTypeConfiguration<GameRoundPlayer>
    {
        public void Configure(EntityTypeBuilder<GameRoundPlayer> builder)
        {
            builder.ToTable(nameof(GameRoundPlayer));

            builder.HasOne(grp => grp.Game)
                .WithMany()
                .HasForeignKey(grp => grp.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(grp => grp.GamePlayer)
                .WithMany(gp => gp.RoundParticipations)
                .HasForeignKey(grp => grp.GamePlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(grp => grp.Round)
                .WithMany(r => r.Players)
                .HasForeignKey(grp => grp.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}