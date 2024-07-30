using KSozluk.Application.Features.Descriptions.Commands.DeleteDescriptions;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Features.Words.Commands.UpdateWord;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DescriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DescriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDescriptions([FromQuery] GetDescriptionsCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteDescription(DeleteDescriptionCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
