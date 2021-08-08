using System;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Contact
{
    public class GetContactRequest : IRequest<GetContactResponse>
    {
        public string CustomerId { get; set; }
        public string ContactId { get; set; }
    }

    public class GetContactResponse
    {
        public int ContactId { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
    }

    public class GetNoteHandler : IRequestHandler<GetContactRequest, GetContactResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetNoteHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetContactResponse> Handle(GetContactRequest request, CancellationToken cancellationToken)
        {
            var contact = await _customerDbContext.Contacts
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    c => c.CustomerId.Equals(request.CustomerId.ToInt()) &&
                         c.ContactId.Equals(request.ContactId.ToInt()),
                    cancellationToken: cancellationToken);

            if (contact is null)
                return null;

            return new GetContactResponse()
            {
                ContactId = contact.ContactId,
                Type = contact.Type,
                Detail = contact.Detail,
                CreatedDateTimeUtc = contact.CreatedDateTimeUtc,
                UpdatedDateTimeUtc = contact.UpdatedDateTimeUtc
            };
        }
    }
}
