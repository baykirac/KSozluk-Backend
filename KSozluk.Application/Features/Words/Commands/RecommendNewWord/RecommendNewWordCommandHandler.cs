using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.RecommendNewWord
{
    public class RecommendNewWordCommandHandler : RequestHandlerBase<RecommendNewWordCommand, RecommendNewWordResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;
        public RecommendNewWordCommandHandler(IWordRepository wordRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _unit = unit;
        }
        public async override Task<RecommendNewWordResponse> Handle(RecommendNewWordCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<RecommendNewWordResponse>(OperationMessages.PermissionFailure);
            }

            var now = DateTime.Now;
            
            var existingWord = await _wordRepository.FindByContentAsync(request.WordContent);

            if (existingWord is null)
            {
                var word = Word.Create(request.WordContent, userId, now, now);
                foreach (var descriptionText in request.DescriptionContent)
                {
                    var description = Description.Create(descriptionText, 0, 0, userId, null);
                    word.AddDescription(description);
                }
                await _wordRepository.CreateAsync(word);
                await _unit.SaveChangesAsync();

                return Response.SuccessWithBody<RecommendNewWordResponse>(word, OperationMessages.DescriptionRecommendedSuccessFully);
            }

            var updated = Word.UpdateOperationDate(existingWord, now);
            foreach (var descriptionText in request.DescriptionContent)
            {
                var description = Description.Create(descriptionText, 0, 0, userId, null);
                existingWord.AddDescription(description);
            }
            await _unit.SaveChangesAsync();
            return Response.SuccessWithBody<RecommendNewWordResponse>(existingWord, OperationMessages.DescriptionRecommendedSuccessFully);
        }
    }
}