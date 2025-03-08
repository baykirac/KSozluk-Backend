using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.UpdateWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.GetWordsByLetter
{
    public class GetWordsByLetterCommandHandler : RequestHandlerBase<GetWordsByLetterCommand, GetWordsByLetterResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;


        public GetWordsByLetterCommandHandler(IWordRepository wordRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _unit= unit;
        }

        public async override Task<GetWordsByLetterResponse> Handle(GetWordsByLetterCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<GetWordsByLetterResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetWordsByLetterAsync(request.Letter, request.PageNumber, request.PageSize);

            if (!words.Any())
            {
                return Response.Failure<GetWordsByLetterResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<GetWordsByLetterResponse>(words, OperationMessages.WordsGettedSuccessfully);
        }
    }
}
