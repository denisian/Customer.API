using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Mappings;
using Customer.Api.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Customer
{
    public class GetCustomersRequest : IRequest<GetCustomersResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string SortBy { get; set; }
        public bool IsDescending { get; set; }
    }

    public class GetCustomersResponse
    {
        public List<GetCustomerResponse> Customers { get; set; }

        public GetCustomersResponse()
        {
            Customers = new List<GetCustomerResponse>();
        }
    }

    public class GetCustomersHandler : IRequestHandler<GetCustomersRequest, GetCustomersResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public GetCustomersHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<GetCustomersResponse> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
        {
            var customers = await _customerDbContext.Customers
                .AsNoTracking()
                .Include(c => c.Status)
                .Include(c => c.Contacts)
                .Include(c => c.Notes)
                .ToListAsync(cancellationToken: cancellationToken);

            if (!customers.Any())
                return null;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                customers = customers.Where(c =>
                    c.FirstName.Contains(request.FirstName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(request.LastName))
                customers = customers.Where(c =>
                    c.LastName.Contains(request.LastName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(request.Status))
                customers = customers.Where(c =>
                    c.Status.Description.Equals(request.Status, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (string.Equals(request.SortBy, nameof(request.FirstName),
                        StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(request.SortBy, nameof(request.LastName),
                        StringComparison.InvariantCultureIgnoreCase))
                    customers = customers.AsQueryable().OrderBy(request.SortBy, request.IsDescending).ToList();
            }

            return new GetCustomersResponse()
            {
                Customers = customers.Select(c => c.ToGetCustomerResponse()).ToList()
            };
        }
    }
}
