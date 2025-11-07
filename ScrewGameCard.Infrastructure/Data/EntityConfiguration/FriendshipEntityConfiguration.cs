using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Infrastructure.Data.EntityConfiguration
{
    public class FriendshipEntityConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable(nameof(Friendship));

            builder.HasIndex(f => new { f.PlayerId, f.FriendId })
                .IsUnique();

            // This ensures only one direction can exist
            builder.HasIndex(f => new { f.FriendId, f.PlayerId })
                .IsUnique();

            builder
                .HasOne(f => f.Player)
                .WithMany(p => p.Friendships)
                .HasForeignKey(f => f.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(f => f.Friend)
                .WithMany(p => p.FriendOf)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
