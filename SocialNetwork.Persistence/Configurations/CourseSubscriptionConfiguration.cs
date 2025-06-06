using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elomoas.Persistence.Configurations;

public class CourseSubscriptionConfiguration : IEntityTypeConfiguration<CourseSubscription>
{
    public void Configure(EntityTypeBuilder<CourseSubscription> builder)
    {
        builder.ToTable("CourseSubscriptions");

        builder.Property(cs => cs.SubscriptionPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(cs => cs.DurationInMonths)
            .IsRequired();

        builder.Property(cs => cs.ExpirationDate)
            .IsRequired();

        // Foreign key constraints
        builder.HasIndex(cs => cs.UserId);
        builder.HasIndex(cs => cs.CourseId);

        // Composite unique index
        builder.HasIndex(cs => new { cs.UserId, cs.CourseId })
            .IsUnique();

        // Add foreign key constraints without navigation properties
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(cs => cs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(cs => cs.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 