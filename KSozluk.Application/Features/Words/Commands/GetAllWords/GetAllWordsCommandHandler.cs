using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetWordsByLetter;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;


namespace KSozluk.Application.Features.Words.Commands.GetAllWords
{
    public class GetAllWordsCommandHandler : RequestHandlerBase<GetAllWordsCommand, GetAllWordsResponse>
    {
        private readonly IWordRepository _wordRepository;

        public GetAllWordsCommandHandler(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public async override Task<GetAllWordsResponse> Handle(GetAllWordsCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<GetAllWordsResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetAllWordsAsync();

            if (!words.Any())
            {
                return Response.Failure<GetAllWordsResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<GetAllWordsResponse>(words, OperationMessages.GettedAllWords);
        }
    }
}
