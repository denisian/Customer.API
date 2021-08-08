using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Note
{
    public class GetNotesRequest : IRequest<GetNotesResponse>
    {
        public string CustomerId { get; set; }
    }

    public class GetNotesResponse
    {
        public List<GetNoteResponse> Notes { get; set; }

        public GetNotesResponse()
        {
            Notes = new List<GetNoteResponse>();
        }
    }

    public class GetNotesHandler : IRequestHandler<GetNotesRequest, GetNotesResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetNotesHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetNotesResponse> Handle(GetNotesRequest request, CancellationToken cancellationToken)
        {
            var notes = await _customerDbContext.Notes
                .AsNoTracking()
                .Where(c => c.CustomerId.Equals(request.CustomerId.ToInt()))
                .Select(c => new GetNoteResponse()
                {
                    NoteId = c.NoteId,
                    Detail = c.Detail,
                    CreatedDateTimeUtc = c.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = c.UpdatedDateTimeUtc
                })
                .OrderBy(n=>n.NoteId)
                .ToListAsync(cancellationToken: cancellationToken);

            if (!notes.Any())
                return null;

            return new GetNotesResponse()
            {
                Notes = notes
            };
        }
    }
}
