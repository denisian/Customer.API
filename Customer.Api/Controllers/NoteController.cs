using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Customer.Api.Handler.Note;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

namespace Customer.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}/notes/{noteId}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetNote(string customerId, string noteId)
        {
            var response = await _mediator.Send(new GetNoteRequest()
            {
                CustomerId = customerId,
                NoteId = noteId
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{customerId}/notes")]
        [Produces("application/json")]
        public async Task<ActionResult> GetNotes(string customerId)
        {
            var response = await _mediator.Send(new GetNotesRequest()
            {
                CustomerId = customerId
            });

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("{customerId}/notes")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateNote([FromRoute] string customerId, [FromBody] CreateNoteRequest request)
        {
            request.CustomerId = customerId;
            var response = await _mediator.Send(request);

            if (response is null)
                return NotFound();

            return Created(Request.GetDisplayUrl(), response);
        }

        [HttpPut("{customerId}/notes/{noteId}")]
        [Produces("application/json")]
        public async Task<ActionResult> UpdateNote([FromRoute] string customerId, string noteId, [FromBody] UpdateNoteRequest request)
        {
            request.CustomerId = customerId;
            request.NoteId = noteId;
            var response = await _mediator.Send(request);

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpDelete("{customerId}/notes/{noteId}")]
        [Produces("application/json")]
        public async Task<ActionResult> UpdateNote([FromRoute] string customerId, string noteId, [FromBody] DeleteNoteRequest request)
        {
            request.CustomerId = customerId;
            request.NoteId = noteId;
            var response = await _mediator.Send(request);
        
            if (response is null)
                return NotFound();
        
            return Ok(response);
        }
    }
}
