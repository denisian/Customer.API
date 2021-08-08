using System.Linq;
using Customer.Api.Handler.Contact;
using Customer.Api.Handler.Customer;
using Customer.Api.Handler.Note;

namespace Customer.Api.Mappings
{
    public static class MappingExtensions
    {
        public static GetCustomerResponse ToGetCustomerResponse(this Persistence.Entities.Customer customer)
        {
            return new GetCustomerResponse()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CreatedDateTimeUtc = customer.CreatedDateTimeUtc,
                UpdatedDateTimeUtc = customer.UpdatedDateTimeUtc,
                Status = customer.Status,
                Contacts = customer.Contacts.Select(c => new GetContactResponse()
                {
                    ContactId = c.ContactId,
                    Type = c.Type,
                    Detail = c.Detail,
                    CreatedDateTimeUtc = c.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = c.UpdatedDateTimeUtc
                }).ToList(),
                Notes = customer.Notes.Select(n => new GetNoteResponse()
                {
                    NoteId = n.NoteId,
                    Detail = n.Detail,
                    CreatedDateTimeUtc = n.CreatedDateTimeUtc,
                    UpdatedDateTimeUtc = n.UpdatedDateTimeUtc
                }).ToList()
            };
        }
    }
}
