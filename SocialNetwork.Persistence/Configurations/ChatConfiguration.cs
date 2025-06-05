using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Elomoas.Domain.Entities;

namespace Elomoas.Persistence.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.User1Id)
                .IsRequired();

            builder.Property(x => x.User2Id)
                .IsRequired();

            builder.HasMany(x => x.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 