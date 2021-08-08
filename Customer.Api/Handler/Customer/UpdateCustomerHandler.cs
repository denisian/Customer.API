using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Helpers;
using Customer.Api.Persistence.Data;
using Customer.Api.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Handler.Customer
{
    public class UpdateCustomerRequest : IRequest<UpdateCustomerResponse>
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Status { get; set; }
    }

    public class UpdateCustomerResponse
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? UpdatedDateTimeUtc { get; set; }
        public Status Status { get; set; }
    }

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
    {
        private readonly ICustomerDbContext _customerDbContext;

        public UpdateCustomerHandler(ICustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _customerDbContext.Customers
                .Include(c => c.Status)
                .SingleOrDefaultAsync(c => c.CustomerId.Equals(request.CustomerId.ToInt()),
                    cancellationToken: cancellationToken);

            if (customer is null)
                return null;

            var updatedStatus = await _customerDbContext.Statuses
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    s => s.Description.Equals(request.Status),
                    cancellationToken: cancellationToken);

            if (updatedStatus is null)
                return null;

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.StatusId = updatedStatus.StatusId;
            customer.UpdatedDateTimeUtc = DateTime.UtcNow;

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
                        if (entry.Entity is Persistence.Entities.Customer or Status)
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

            return new UpdateCustomerResponse()
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                UpdatedDateTimeUtc = customer.UpdatedDateTimeUtc,
                Status = updatedStatus
            };
        }
    }
}
