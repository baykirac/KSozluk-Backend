using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.DeleteWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using System.Runtime.CompilerServices;

namespace KSozluk.Application.Features.Descriptions.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : RequestHandlerBase<UpdateOrderCommand, UpdateOrderResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IUserRepository userRepository, IUserService userService, IDescriptionRepository descriptionRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userService = userService;
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateOrderResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);

            if (description == null)
            {
                return Response.Failure<UpdateOrderResponse>(OperationMessages.PermissionFailure);
            }

            description.UpdateOrder(request.Order);

            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithMessage<UpdateOrderResponse>(OperationMessages.UpdatedOrderSuccessfully);
        }
    }
}
