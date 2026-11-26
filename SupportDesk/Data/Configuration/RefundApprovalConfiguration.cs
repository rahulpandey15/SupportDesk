using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Configuration;

public sealed class RefundApprovalConfiguration : IEntityTypeConfiguration<RefundApproval>
{
    public void Configure(EntityTypeBuilder<RefundApproval> b)
    {
        b.ToTable("RefundApprovals");
        b.HasKey(x => x.ApprovalId);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.DecidedBy).HasMaxLength(200);
        b.Property(x => x.Comment).HasMaxLength(500);
    }
}
