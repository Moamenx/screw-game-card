using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class StatisticsEntityTypeConfiguration : IEntityTypeConfiguration<Statistics>
    {
        public void Configure(EntityTypeBuilder<Statistics> builder)
        {
            builder.ToTable(nameof(Statistics));

            builder.HasOne(s => s.Player)
                .WithOne(p => p.Statistics)
                .HasForeignKey<Statistics>(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}