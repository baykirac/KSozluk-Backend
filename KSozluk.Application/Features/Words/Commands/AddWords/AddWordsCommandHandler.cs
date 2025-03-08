using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.AddWords
{
    public class AddWordsCommandHandler : RequestHandlerBase<AddWordsCommand, AddWordsResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;


        public AddWordsCommandHandler(IWordRepository wordRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _unit = unit;
        }

        public async override Task<AddWordsResponse> Handle(AddWordsCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<AddWordsResponse>(OperationMessages.PermissionFailure);
            }

            var existedWord = await _wordRepository.FindByContentAsync(request.WordContent);
            if (existedWord != null)
            {
                return Response.SuccessWithBody<AddWordsResponse>(new AddWordsResponse
                {
                    WordExists = true,
                    AddedWord = null
                }, OperationMessages.WordExist);
            }

            var word = Word.Create(request.WordContent, userId);
            await _wordRepository.CreateAsync(word);
            await _unit.SaveChangesAsync();
            Word.ClearResponse(word);

            return Response.SuccessWithBody<AddWordsResponse>(new AddWordsResponse
            {
                WordExists = false,
                AddedWord = word
            }, OperationMessages.WordAddedSuccessfully);
        }
    }
}