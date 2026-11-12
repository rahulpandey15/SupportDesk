using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Configuration;

public class AgentSessionConfiguration : IEntityTypeConfiguration<AgentSession>
{
    public void Configure(EntityTypeBuilder<AgentSession> builder)
    {
        builder.HasKey(x => x.SessionKey);
        builder.Property(x => x.SessionKey)
            .IsRequired();
        builder.Property(x => x.CustomerId)
            .IsRequired();
        builder.Property(x => x.AgentName)
            .IsRequired();
        builder.Property(x => x.ConversationId)
            .IsRequired();
        builder.Property(x => x.StateJson)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.CustomerId,
                x.AgentName,
                x.ConversationId
            })
            .IsUnique();

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}