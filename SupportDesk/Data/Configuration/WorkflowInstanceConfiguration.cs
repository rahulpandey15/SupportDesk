using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Configuration;

public sealed class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> b)
    {
        b.ToTable("WorkflowInstances");
        b.HasKey(x => x.WorkflowId);
        b.Property(x => x.Type).HasMaxLength(100);
        b.Property(x => x.CorrelationId).HasMaxLength(100);
        b.Property(x => x.State).HasConversion<string>().HasMaxLength(30);
    }
}
