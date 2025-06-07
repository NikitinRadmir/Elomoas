using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elomoas.Persistence.Configurations;

public class GroupSubscriptionConfiguration : IEntityTypeConfiguration<GroupSubscription>
{
    public void Configure(EntityTypeBuilder<GroupSubscription> builder)
    {
        builder.ToTable("GroupSubscriptions");

        // Foreign key constraints
        builder.HasIndex(cs => cs.UserId);
        builder.HasIndex(cs => cs.GroupId);

        // Composite unique index
        builder.HasIndex(cs => new { cs.UserId, cs.GroupId })
            .IsUnique();

        // Add foreign key constraints without navigation properties
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(cs => cs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Group>()
            .WithMany()
            .HasForeignKey(cs => cs.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 