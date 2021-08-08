using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Customer.Api.Handler.Customer;
using MediatR;

namespace Customer.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetCustomer(string customerId)
        {
            var response = await _mediator.Send(new GetCustomerRequest()
            {
                CustomerId = customerId
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> GetCustomers(string firstName, string lastName, string status, string sortBy, bool isDescending = false)
        {
            var response = await _mediator.Send(new GetCustomersRequest()
            {
                FirstName = firstName,
                LastName = lastName,
                Status = status,
                SortBy = sortBy,
                IsDescending = isDescending
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut("{customerId}")]
        [Produces("application/json")]
        public async Task<ActionResult> UpdateCustomer([FromRoute] string customerId, [FromBody] UpdateCustomerRequest request)
        {
            request.CustomerId = customerId;
            var response = await _mediator.Send(request);

            if (response is null)
                return NotFound();

            return Ok(response);
        }
    }
}
