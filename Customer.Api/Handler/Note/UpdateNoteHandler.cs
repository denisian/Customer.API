using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Note
{
    public class UpdateNoteRequest : IRequest<UpdateNoteResponse>
    {
        public string CustomerId { get; set; }
        public string NoteId { get; set; }
        public string Detail { get; set; }
    }

    public class UpdateNoteResponse
    {
        public int CustomerId { get; set; }
        public int NoteId { get; set; }
        public string Detail { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
    }

    public class UpdateNoteHandler : IRequestHandler<UpdateNoteRequest, UpdateNoteResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public UpdateNoteHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<UpdateNoteResponse> Handle(UpdateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = await _customerDbContext.Notes
                .SingleOrDefaultAsync(
                    n => n.CustomerId.Equals(request.CustomerId.ToInt()) &&
                         n.NoteId.Equals(request.NoteId.ToInt()),
                    cancellationToken: cancellationToken);

            if (note is null)
                return null;

            note.Detail = request.Detail;
            note.UpdatedDateTimeUtc = DateTime.UtcNow;

            var isSaved = false;
            while (!isSaved)
            {
                try
                {
                    await _customerDbContext.SaveChangesAsync(cancellationToken);
                    isSaved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Persistence.Entities.Note)
                        {
                            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException("Concurrency conflict for non-supported entity " +
                                                            entry.Metadata.Name);
                        }
                    }
                }
            }

            return new UpdateNoteResponse()
            {
                CustomerId = note.CustomerId,
                NoteId = note.NoteId,
                Detail = note.Detail,
                UpdatedDateTimeUtc = note.UpdatedDateTimeUtc
            };
        }
    }
}
