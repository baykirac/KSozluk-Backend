using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetWordsByLetter;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using MediatR.Wrappers;

namespace KSozluk.Application.Features.Words.Commands.GetWordsByContains
{
    public class GetWordsByContainsCommandHandler : RequestHandlerBase<GetWordsByContainsCommand, GetWordsByContainsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IWordRepository _wordRepository;

        public GetWordsByContainsCommandHandler(IUserRepository userRepository, IUserService userService, IWordRepository wordRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _wordRepository = wordRepository;
        }

        public async override Task<GetWordsByContainsResponse> Handle(GetWordsByContainsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<GetWordsByContainsResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetWordsByContainsAsync(request.Content);

            if (!words.Any())
            {
                return Response.Failure<GetWordsByContainsResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<GetWordsByContainsResponse>(words, OperationMessages.GettedAllWords);
        }
    }
}
