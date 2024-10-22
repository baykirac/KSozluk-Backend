using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using MediatR.Wrappers;
using System.Runtime.CompilerServices;

namespace KSozluk.Application.Features.Descriptions.Commands.GetDescriptions
{
    public class GetDescriptionsCommandHandler : RequestHandlerBase<GetDescriptionsCommand, GetDescriptionsResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IFavouriteWordRepository _favouriteWordRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public GetDescriptionsCommandHandler(IDescriptionRepository descriptionRepository, IUserService userService, IUserRepository userRepository, IFavouriteWordRepository favouriteWordRepository)
        {
            _descriptionRepository = descriptionRepository;
            _userService = userService;
            _userRepository = userRepository;
            _favouriteWordRepository = favouriteWordRepository;
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

            var favouriteWord = await _favouriteWordRepository.GetByFavouriteWordAndUserAsync(request.WordId,userId);

            if(favouriteWord != null)
            {
                isFavourited = true;
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
            };

            return Response.SuccessWithBody<GetDescriptionsResponse>(_response, OperationMessages.DescriptionsGettedSuccessfully);
        }
    }
}
