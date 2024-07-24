using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Features.Words.Commands.GetWordsByContains;
using KSozluk.Application.Features.Words.Commands.GetWordsByLetter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllWords([FromQuery] GetAllWordsCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWordsByLetter([FromQuery] GetWordsByLetterCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWordsByContains([FromQuery] GetWordsByContainsCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
