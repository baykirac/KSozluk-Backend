using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Descriptions.Commands.RecommendNewDescription
{
    public class RecommendNewDescriptionCommandHandler : RequestHandlerBase<RecommendNewDescriptionCommand, RecommendNewDescriptionResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;
        private readonly IDescriptionRepository _descriptionRepository;

        public RecommendNewDescriptionCommandHandler(IWordRepository wordRepository, IUnit unit, IDescriptionRepository descriptionRepository)
        {
            _wordRepository = wordRepository;
            _unit = unit;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<RecommendNewDescriptionResponse> Handle(RecommendNewDescriptionCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<RecommendNewDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _wordRepository.FindAsync(request.WordId);

            if (word == null) 
            {
             return Response.Failure<RecommendNewDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var greatestOrder = await _descriptionRepository.FindGreatestOrder(word.Id);
            var newOrder = greatestOrder + 1;

            var description = Description.Create(request.Content, newOrder, 0, userId, request.PreviousDescriptionId);  

            word.AddDescription(description);

            await _unit.SaveChangesAsync();
            return Response.SuccessWithBody<RecommendNewDescriptionResponse>(description, OperationMessages.DescriptionRecommendedSuccessFully);
        }
    }
}
