using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
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
        }
    }
}
