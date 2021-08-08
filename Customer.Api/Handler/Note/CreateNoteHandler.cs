using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Note
{
    public class CreateNoteRequest : IRequest<CreateNoteResponse>
    {
        public string CustomerId { get; set; }
        public string Detail { get; set; }
    }

    public class CreateNoteResponse
    {
        public int CustomerId { get; set; }
        public int NoteId { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
    }

    public class CreateNoteHandler : IRequestHandler<CreateNoteRequest, CreateNoteResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public CreateNoteHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<CreateNoteResponse> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var isCustomerExists = await _customerDbContext.Customers
                .AsNoTracking()
                .AnyAsync(c => c.CustomerId.Equals(request.CustomerId.ToInt()),
                    cancellationToken: cancellationToken);

            if (!isCustomerExists)
                return null;

            var newNote = new Persistence.Entities.Note()
            {
                CustomerId = request.CustomerId.ToInt(),
                Detail = request.Detail,
                CreatedDateTimeUtc = DateTime.UtcNow
            };

            await _customerDbContext.Notes.AddAsync(newNote, cancellationToken);
            await _customerDbContext.SaveChangesAsync(cancellationToken);

            return new CreateNoteResponse()
            {
                CustomerId = newNote.CustomerId,
                NoteId = newNote.NoteId,
                Detail = newNote.Detail,
                CreatedDateTimeUtc = newNote.CreatedDateTimeUtc
            };
        }
    }
}
