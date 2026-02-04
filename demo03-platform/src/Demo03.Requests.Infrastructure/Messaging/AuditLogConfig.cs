using Demo03.Requests.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo03.Requests.Infrastructure.Messaging
{
    public sealed class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> b)
        {
            b.ToTable("audit_logs");
            b.HasKey(x => x.Id);

            b.Property(x => x.Action).HasColumnName("action").HasMaxLength(60).IsRequired();
            b.Property(x => x.Actor).HasColumnName("actor").HasMaxLength(120).IsRequired();
            b.Property(x => x.At).HasColumnName("at").HasDefaultValueSql("NOW()").IsRequired();
            b.Property(x => x.CorrelationId).HasColumnName("correlation_id").HasMaxLength(100);
            b.Property(x => x.DataJson).HasColumnName("data_json");

            b.Property(x => x.RequestId).HasColumnName("request_id").IsRequired();
            b.HasIndex(x => x.RequestId);
            b.HasIndex(x => x.At);
        }
    }
}
