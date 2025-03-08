// LikeDescriptionCommandHandler.cs
using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWord
{
    public class FavouriteWordCommandHandler : RequestHandlerBase<FavouriteWordCommand, FavouriteWordResponse>
    {
        private readonly IFavoriteWordRepository _favouriteLikeRepository;
        private readonly IUnit _unit;

       public FavouriteWordCommandHandler(IFavoriteWordRepository favouriteLikeRepository, IUnit unit)
        {
            _favouriteLikeRepository = favouriteLikeRepository;
            _unit = unit;
        }

        public async override Task<FavouriteWordResponse> Handle(FavouriteWordCommand request, CancellationToken cancellationToken)
        {

             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<FavouriteWordResponse>(OperationMessages.PermissionFailure);
            }

 

            var existingLike = await _favouriteLikeRepository.GetByFavoriteWordAndUserAsync(request.WordId, userId);

            if (existingLike != null)
            {
                _favouriteLikeRepository.Delete(existingLike);
                await _unit.SaveChangesAsync();

                return Response.SuccessWithBody<FavouriteWordResponse>(request.WordId, OperationMessages.WordUnfavouritedSuccessfully);
            }
            else
           {
                var newLike = new KSozluk.Domain.FavoriteWord
                {
                    Id = Guid.NewGuid(),
                    WordId = request.WordId,
                    UserId = userId
                };

                await _favouriteLikeRepository.CreateAsync(newLike);
                await _unit.SaveChangesAsync();
                return Response.SuccessWithBody<FavouriteWordResponse>(request.WordId, OperationMessages.WordFavouritedSuccessfully);

            }
        }
    }
}