using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfigurations
{
    public class LocalizationEntityConfiguration : IEntityTypeConfiguration<Localization>
    {
        public void Configure(EntityTypeBuilder<Localization> builder)
        {
            builder.ToTable(nameof(Localization));
            builder.Property(x=>x.Message).IsRequired().HasMaxLength(500);
            builder.Property(x=>x.Language).IsRequired().HasMaxLength(3);
            builder.Property(x=>x.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => new { x.Code, x.Language }).IsUnique();
        }
    }
}
