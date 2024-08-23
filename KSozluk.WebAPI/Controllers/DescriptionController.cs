using KSozluk.Application.Features.Descriptions.Commands.DeleteDescriptions;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Features.Descriptions.Commands.RecommendNewDescription;
using KSozluk.Application.Features.Descriptions.Commands.UpdateOrder;
using KSozluk.Application.Features.Descriptions.Commands.UpdateStatus;
using KSozluk.Application.Features.Words.Commands.RecommendNewWord;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RecommendDescription(RecommendNewDescriptionCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

    }
}
