using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Customer.Api.Handler.Contact;
using MediatR;

namespace Customer.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}/contacts/{contactId}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetContact(string customerId, string contactId)
        {
            var response = await _mediator.Send(new GetContactRequest()
            {
                CustomerId = customerId,
                ContactId = contactId
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{customerId}/contacts")]
        [Produces("application/json")]
        public async Task<ActionResult> GetContacts(string customerId)
        {
            var response = await _mediator.Send(new GetContactsRequest()
            {
                CustomerId = customerId
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }
    }
}
