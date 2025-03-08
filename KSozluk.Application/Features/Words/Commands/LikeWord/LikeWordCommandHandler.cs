using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.LikeWord
{
    public class LikeWordCommandHandler : RequestHandlerBase<LikeWordCommand, LikeWordResponse>

    {
        private readonly ILikeRepository _wordLikeRepository;
        private readonly IUnit _unit;
        private readonly IDescriptionRepository _descriptionRepository;

        public LikeWordCommandHandler( ILikeRepository wordLikeRepository, IUnit unit, IDescriptionRepository descriptionRepository)
        {
            _wordLikeRepository = wordLikeRepository;
            _unit = unit;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<LikeWordResponse> Handle(LikeWordCommand request, CancellationToken cancellationToken)
        { 
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<LikeWordResponse>(OperationMessages.PermissionFailure);
            }

            var existingLike = await _wordLikeRepository.GetByWordAndUserAsync(request.WordId, userId);

            if (existingLike != null)
            {
                _wordLikeRepository.DeleteWordLike(existingLike);
                await _unit.SaveChangesAsync();
                return Response.SuccessWithBody<LikeWordResponse>(request.WordId, OperationMessages.WordLikedSuccessfully);
            }

            var now = DateTime.UtcNow;

            var newWordLike = WordLike.Create(Guid.NewGuid(), request.WordId, userId, now);

            await _wordLikeRepository.CreateWordLike(newWordLike);
            await _unit.SaveChangesAsync();

            return Response.SuccessWithBody<LikeWordResponse>(request.WordId, OperationMessages.WordLikedSuccessfully); 
        }
    }
}
