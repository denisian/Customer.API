using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Contact
{
    public class GetContactsRequest : IRequest<GetContactsResponse>
    {
        public string CustomerId { get; set; }
    }

    public class GetContactsResponse
    {
        public List<GetContactResponse> Contacts { get; set; }

        public GetContactsResponse()
        {
            Contacts = new List<GetContactResponse>();
        }
    }

    public class GetContactsHandler : IRequestHandler<GetContactsRequest, GetContactsResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetContactsHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetContactsResponse> Handle(GetContactsRequest request, CancellationToken cancellationToken)
        {
            var contacts = await _customerDbContext.Contacts
                .AsNoTracking()
                .Where(c => c.CustomerId.Equals(request.CustomerId.ToInt()))
                .Select(c => new GetContactResponse()
                {
                    ContactId = c.ContactId,
                    Type = c.Type,
                    Detail = c.Detail,
                    CreatedDateTimeUtc = c.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = c.UpdatedDateTimeUtc
                })
                .OrderBy(c => c.ContactId)
                .ToListAsync(cancellationToken: cancellationToken);

            if (!contacts.Any())
                return null;

            return new GetContactsResponse()
            {
                Contacts = contacts
            };
        }
    }
}
