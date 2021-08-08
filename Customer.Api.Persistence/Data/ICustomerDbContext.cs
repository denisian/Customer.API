using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Persistence.Data
{
    public interface ICustomerDbContext
    {
        DbSet<Entities.Customer> Customers { get; set; }
        DbSet<Entities.Status> Statuses { get; set; }
        DbSet<Entities.Contact> Contacts { get; set; }
        DbSet<Entities.Note> Notes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}