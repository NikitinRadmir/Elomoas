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

            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<AppUser>(au => au.IdentityId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Img)
                .HasMaxLength(1000);
        }
    }
} 