using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Handler.Contact;
using Customer.Api.Handler.Note;
using Customer.Api.Helpers;
using Customer.Api.Mappings;
using Customer.Api.Persistence.Data;
using Customer.Api.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Customer
{
    public class GetCustomerRequest : IRequest<GetCustomerResponse>
    {
        public string CustomerId { get; set; }
    }

    public class GetCustomerResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
        public Status Status { get; set; }
        public List<GetContactResponse> Contacts { get; set; }
        public List<GetNoteResponse> Notes { get; set; }
    }

    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetCustomerHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetCustomerResponse> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerDbContext.Customers
                .AsNoTracking()
                .Include(c => c.Status)
                .Include(c => c.Contacts)
                .Include(c => c.Notes)
                .SingleOrDefaultAsync(c => c.CustomerId.Equals(request.CustomerId.ToInt()),
                    cancellationToken: cancellationToken);

            return customer?.ToGetCustomerResponse();
        }
    }
}
