using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using MediatR.Wrappers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KSozluk.Application.Features.Descriptions.Commands.GetDescriptions
{
    public class GetDescriptionsCommandHandler : RequestHandlerBase<GetDescriptionsCommand, GetDescriptionsResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IFavoriteWordRepository _favoriteWordRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public GetDescriptionsCommandHandler(IDescriptionRepository descriptionRepository, ILikeRepository likeRepository, IUserService userService, IUserRepository userRepository, IFavoriteWordRepository favouriteWordRepository)
        {
            _descriptionRepository = descriptionRepository;
            _userService = userService;
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _favoriteWordRepository = favouriteWordRepository;
        }

        public async override Task<GetDescriptionsResponse> Handle(GetDescriptionsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<GetDescriptionsResponse>(OperationMessages.PermissionFailure);
            }
            
            var descriptions = await _descriptionRepository.FindByWordAsync(request.WordId,userId);

            var isFavourited = false;
            var isLikedWord = false;

            var favoriteWord = await _favoriteWordRepository.GetByFavoriteWordAndUserAsync(request.WordId,userId);
            var likedWord = await _likeRepository.FindLikedWordAsync(request.WordId,userId);

            if(favoriteWord != null)
            {
                isFavourited = true;
            }

            if(likedWord != null)
            {
                isLikedWord = true;
            }

            //var favouriteWord = await _favouriteWordRepository.GetByFavouriteWordAndUserAsync(request.WordId, userId);

            //if (favouriteWord != null)
            //{
            //    isFavourited = true;
            //}

            var _response = new
            {
                Body = descriptions,
                IsFavourited = isFavourited,
                IsLikedWord = isLikedWord
            };


            return Response.SuccessWithBody<GetDescriptionsResponse>(_response, OperationMessages.DescriptionsGettedSuccessfully);
        }
    }
}
