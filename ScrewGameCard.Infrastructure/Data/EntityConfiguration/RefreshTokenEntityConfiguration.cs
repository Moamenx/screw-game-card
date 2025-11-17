using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken));
        }
    }
}
