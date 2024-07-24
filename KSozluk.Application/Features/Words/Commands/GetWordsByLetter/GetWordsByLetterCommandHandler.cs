using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.GetWordsByLetter
{
    public class GetWordsByLetterCommandHandler : RequestHandlerBase<GetWordsByLetterCommand, GetWordsByLetterResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetWordsByLetterCommandHandler(IWordRepository wordRepository, IUserService userService, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _wordRepository = wordRepository;
            _userService = userService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<GetWordsByLetterResponse> Handle(GetWordsByLetterCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<GetWordsByLetterResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetWordsByLetterAsync(request.Letter, request.PageNumber, request.PageSize);

            return Response.SuccessWithBody<GetWordsByLetterResponse>(words, OperationMessages.WordsGettedSuccessfully);
        }
    }
}
