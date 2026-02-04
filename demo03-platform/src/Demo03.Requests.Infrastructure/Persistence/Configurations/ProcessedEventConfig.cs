using Demo03.Requests.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo03.Requests.Infrastructure.Persistence.Configurations
{
    public sealed class ProcessedEventConfig : IEntityTypeConfiguration<ProcessedEvent>
    {
        public void Configure(EntityTypeBuilder<ProcessedEvent> b)
        {
            b.ToTable("processed_events");

            b.HasKey(x => x.MessageId);

            b.Property(x => x.MessageId)
                .HasColumnName("message_id")
                .HasMaxLength(200)
                .IsRequired();

            b.Property(x => x.ProcessedAt)
                .HasColumnName("processed_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();
        }
    }
}
