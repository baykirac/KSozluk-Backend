using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Features.Words.Commands.UpdateWordById;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.UpdateWord
{
    public class UpdateWordCommandHandler : RequestHandlerBase<UpdateWordCommand, UpdateWordResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;

        public UpdateWordCommandHandler(IWordRepository wordRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _unit = unit;
        }

        public async override Task<UpdateWordResponse> Handle(UpdateWordCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<UpdateWordResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _wordRepository.FindAsync(request.WordId);

            if (word == null)
            {
                return Response.Failure<UpdateWordResponse>(OperationMessages.PermissionFailure);
            }

            word.ChangeContent(request.WordContent);
            word.Descriptions.SingleOrDefault(d => d.Id == request.DescriptionId).UpdateContent(request.DescriptionContent);
            word.Descriptions.SingleOrDefault(d => d.Id == request.DescriptionId).UpdateRecommender(userId);

            await _unit.SaveChangesAsync();

            return Response.SuccessWithBody<UpdateWordResponse>(word, OperationMessages.WordUpdatedSuccessfully);
        }
    }
}
