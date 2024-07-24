using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using MediatR.Wrappers;
using System.Runtime.CompilerServices;

namespace KSozluk.Application.Features.Descriptions
{
    public class GetDescriptionsCommandHandler : RequestHandlerBase<GetDescriptionsCommand, GetDescriptionsResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public GetDescriptionsCommandHandler(IDescriptionRepository descriptionRepository, IUserService userService, IUserRepository userRepository)
        {
            _descriptionRepository = descriptionRepository;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async override Task<GetDescriptionsResponse> Handle(GetDescriptionsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<GetDescriptionsResponse>(OperationMessages.PermissionFailure);
            }

            var descriptions = await _descriptionRepository.FindByWordAsync(request.WordId);

            return Response.SuccessWithBody<GetDescriptionsResponse>(descriptions, OperationMessages.DescriptionsGettedSuccessfully);
        }
    }
}
