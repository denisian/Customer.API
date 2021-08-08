using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Api.Persistence.Entities
{
    public class Status
    {
        public byte StatusId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
    }

    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder
                .ToTable("Status")
                .HasKey(c => c.StatusId);
        }
    }
}