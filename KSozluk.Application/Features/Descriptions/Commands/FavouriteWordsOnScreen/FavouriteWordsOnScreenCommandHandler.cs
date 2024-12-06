using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.LikeDescription;
using KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWordsOnScreen
{
    public class FavouriteWordsOnScreenCommandHandler : RequestHandlerBase<FavouriteWordsOnScreenCommand, FavouriteWordsOnScreenResponse>
    {

        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IFavoriteWordRepository _favoriteWordRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;


        public FavouriteWordsOnScreenCommandHandler(IUserService userService, IUserRepository userRepository, IFavoriteWordRepository favoriteWordRepository, IUnitOfWork unitOfWork, IWordRepository wordRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _favoriteWordRepository = favoriteWordRepository;
            _unitOfWork = unitOfWork;
            _wordRepository = wordRepository;
        }

        public async override Task<FavouriteWordsOnScreenResponse> Handle(FavouriteWordsOnScreenCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<FavouriteWordsOnScreenResponse>(OperationMessages.PermissionFailure);
            }

            var favouriteWords = await _favoriteWordRepository.GetFavouriteWordsByUserIdAsync(userId);
            var test = new FavouriteWordsOnScreenResponse()
            {
                responseFavouriteWordsDtos = favouriteWords,
            };


            return Response.SuccessWithBody<FavouriteWordsOnScreenResponse>(test, OperationMessages.DescriptionsGettedSuccessfully);

        }

    }
}
