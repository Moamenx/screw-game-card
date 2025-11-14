using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class RoundEntityTypeConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            builder.ToTable(nameof(Round));

            builder.HasOne(r => r.Game)
                .WithMany(g => g.Rounds)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Players)
                .WithOne(grp => grp.Round)
                .HasForeignKey(grp => grp.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}