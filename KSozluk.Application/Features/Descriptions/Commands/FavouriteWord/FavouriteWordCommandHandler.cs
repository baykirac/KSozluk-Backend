// LikeDescriptionCommandHandler.cs
using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;
using System;

namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWord
{
    public class FavouriteWordCommandHandler : RequestHandlerBase<FavouriteWordCommand, FavouriteWordResponse>
    {
        private readonly IFavoriteWordRepository _favouriteLikeRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteWordCommandHandler(IFavoriteWordRepository favouriteLikeRepository, IUserService userService, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _favouriteLikeRepository = favouriteLikeRepository;
            _userService = userService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<FavouriteWordResponse> Handle(FavouriteWordCommand request, CancellationToken cancellationToken)
        {

            //Map
            var userId = _userService.GetUserId();
            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<FavouriteWordResponse>(OperationMessages.PermissionFailure);
            }

            var existingLike = await _favouriteLikeRepository.GetByFavoriteWordAndUserAsync(request.WordId, userId);

            if (existingLike != null)
            {
                _favouriteLikeRepository.Delete(existingLike);
                await _unitOfWork.SaveChangesAsync();

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
                await _unitOfWork.SaveChangesAsync();
                return Response.SuccessWithBody<FavouriteWordResponse>(request.WordId, OperationMessages.WordFavouritedSuccessfully);

            }
        }
    }
}