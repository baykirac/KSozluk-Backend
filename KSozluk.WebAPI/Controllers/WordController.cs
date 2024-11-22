using KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked;
using KSozluk.Application.Features.Words.Commands.AddWord;
using KSozluk.Application.Features.Words.Commands.AddWords;
using KSozluk.Application.Features.Words.Commands.DeleteWord;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Features.Words.Commands.GetApprovedWordsPaginated;
using KSozluk.Application.Features.Words.Commands.GetPaginatedWords;
using KSozluk.Application.Features.Words.Commands.GetWordsByContains;
using KSozluk.Application.Features.Words.Commands.GetWordsByLetter;
using KSozluk.Application.Features.Words.Commands.LikeWord;
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
    public class WordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> WeeklyLiked([FromQuery] WeeklyLikedCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

          return Ok(response);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> AddWord(AddWordCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddWords(AddWordsCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetApprovedWordsPaginated([FromQuery] GetApprovedWordsPaginatedCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateWord(UpdateWordCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteWord(DeleteWordCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RecommendWord(RecommendNewWordCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> LikeWord(LikeWordCommand command, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }

}
