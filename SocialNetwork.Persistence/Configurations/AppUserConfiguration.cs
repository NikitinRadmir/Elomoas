using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Elomoas.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ProfileImage)
                .HasMaxLength(500);

            builder.Property(x => x.IdentityId)
                .IsRequired();

            builder.HasOne(x => x.IdentityUser)
                .WithOne()
                .HasForeignKey<AppUser>(x => x.IdentityId);

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Img)
                .HasMaxLength(1000);
        }
    }
} 