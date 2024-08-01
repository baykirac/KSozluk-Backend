using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.UpdateWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Descriptions.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : RequestHandlerBase<UpdateStatusCommand, UpdateStatusResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStatusCommandHandler(IUserService userService, IUserRepository userRepository, IDescriptionRepository descriptionRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<UpdateStatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateStatusResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);

            description.UpdateStatus(request.Status);
            description.UpdateAcceptor(userId);

            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithMessage<UpdateStatusResponse>(OperationMessages.UpdatedDescriptionStatusSuccessfully);
        }
    }
}
