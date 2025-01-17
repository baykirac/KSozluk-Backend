using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.FavouriteWord;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Descriptions.Commands.DescriptionTimeline
{
    public class DescriptionTimelineCommandHandler : RequestHandlerBase<DescriptionTimelineCommand, DescriptionTimelineResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DescriptionTimelineCommandHandler(IDescriptionRepository descriptionRepository, ILikeRepository likeRepository, IUserService userService, IUserRepository userRepository, IFavoriteWordRepository favouriteWordRepository, IUnitOfWork unitOfWork)
        {
            _descriptionRepository = descriptionRepository;
            _userService = userService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<DescriptionTimelineResponse> Handle(DescriptionTimelineCommand request, CancellationToken cancellationToken)
        {

            var userId = _userService.GetUserId();

            var response = await _descriptionRepository.GetDescriptionForTimelineAsync(userId);

            if (!response.Any()) {
                return Response.Failure<DescriptionTimelineResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<DescriptionTimelineResponse>(response, OperationMessages.DescriptionsGettedSuccessfully);


        }
    }

}