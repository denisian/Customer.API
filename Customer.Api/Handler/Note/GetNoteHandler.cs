using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Note
{
    public class GetNoteRequest : IRequest<GetNoteResponse>
    {
        public string CustomerId { get; set; }
        public string NoteId { get; set; }
    }

    public class GetNoteResponse
    {
        public int NoteId { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
    }

    public class GetNoteHandler : IRequestHandler<GetNoteRequest, GetNoteResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetNoteHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetNoteResponse> Handle(GetNoteRequest request, CancellationToken cancellationToken)
        {
            var note = await _customerDbContext.Notes
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    n => n.CustomerId.Equals(request.CustomerId.ToInt()) &&
                         n.NoteId.Equals(request.NoteId.ToInt()),
                    cancellationToken: cancellationToken);

            if (note is null)
                return null;

            return new GetNoteResponse()
            {
                NoteId = note.NoteId,
                Detail = note.Detail,
                CreatedDateTimeUtc = note.CreatedDateTimeUtc,
                UpdatedDateTimeUtc = note.UpdatedDateTimeUtc
            };
        }
    }
}
