using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.LikeDescription;
using KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWordsOnScreen
{
    public class FavouriteWordsOnScreenCommandHandler : RequestHandlerBase<FavouriteWordsOnScreenCommand, FavouriteWordsOnScreenResponse>
    {

        private readonly IFavoriteWordRepository _favoriteWordRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;


        public FavouriteWordsOnScreenCommandHandler( IFavoriteWordRepository favoriteWordRepository, IUnit unit, IWordRepository wordRepository)
        {
            _favoriteWordRepository = favoriteWordRepository;
            _unit = unit;
            _wordRepository = wordRepository;
        }

        public async override Task<FavouriteWordsOnScreenResponse> Handle(FavouriteWordsOnScreenCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<FavouriteWordsOnScreenResponse>(OperationMessages.PermissionFailure);
            }

            var favouriteWords = await _favoriteWordRepository.GetFavouriteWordsByUserIdAsync(userId);

            if (!favouriteWords.Any()) 
            {
                return Response.Failure<FavouriteWordsOnScreenResponse>(OperationMessages.PermissionFailure);
            }
            var response = new FavouriteWordsOnScreenResponse()
            {
                responseFavouriteWordsDtos = favouriteWords,
            };


            return Response.SuccessWithBody<FavouriteWordsOnScreenResponse>(response, OperationMessages.DescriptionsGettedSuccessfully);

        }

    }
}
