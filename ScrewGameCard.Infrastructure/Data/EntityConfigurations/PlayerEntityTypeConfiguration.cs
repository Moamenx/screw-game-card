using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfigurations
{
    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable(nameof(Player));

            builder.Property(p => p.Language).HasMaxLength(5);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.AvatarUrl).HasMaxLength(200);
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasMany(p => p.HostedGames)
                .WithOne(r => r.Host)
                .HasForeignKey(r => r.HostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Statistics)
                .WithOne(s => s.Player)
                .HasForeignKey<Player>(p => p.StatisticsId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
