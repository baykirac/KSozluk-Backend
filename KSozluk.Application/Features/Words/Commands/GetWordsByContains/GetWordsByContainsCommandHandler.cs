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
        private readonly IWordRepository _wordRepository;

        public GetWordsByContainsCommandHandler(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }   

        public async override Task<GetWordsByContainsResponse> Handle(GetWordsByContainsCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
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
