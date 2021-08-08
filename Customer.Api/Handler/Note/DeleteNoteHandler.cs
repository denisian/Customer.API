using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Note
{
    public class DeleteNoteRequest : IRequest<DeleteNoteResponse>
    {
        public string CustomerId { get; set; }
        public string NoteId { get; set; }
    }

    public class DeleteNoteResponse
    {
        public int CustomerId { get; set; }
        public int NoteId { get; set; }
        public string Detail { get; set; }
    }

    public class DeleteNoteHandler : IRequestHandler<DeleteNoteRequest, DeleteNoteResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public DeleteNoteHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<DeleteNoteResponse> Handle(DeleteNoteRequest request, CancellationToken cancellationToken)
        {
            var note = await _customerDbContext.Notes
                .SingleOrDefaultAsync(
                    n => n.CustomerId.Equals(request.CustomerId.ToInt()) &&
                         n.NoteId.Equals(request.NoteId.ToInt()),
                    cancellationToken: cancellationToken);

            if (note is null)
                return null;

            _customerDbContext.Notes.Remove(note);
            await _customerDbContext.SaveChangesAsync(cancellationToken);

            return new DeleteNoteResponse()
            {
                CustomerId = note.CustomerId,
                NoteId = note.NoteId,
                Detail = note.Detail
            };
        }
    }
}
