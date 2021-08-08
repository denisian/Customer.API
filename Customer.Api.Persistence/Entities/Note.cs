using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Api.Persistence.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public int CustomerId { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }

        public Customer Customer { get; set; }
    }

    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder
                .ToTable("Note")
                .HasKey(c => c.NoteId);
        }
    }
}