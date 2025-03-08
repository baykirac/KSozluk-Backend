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
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnit _unit;

        public UpdateOrderCommandHandler( IDescriptionRepository descriptionRepository, IUnit unit)
        {
            _descriptionRepository = descriptionRepository;
            _unit = unit;
        }

        public async override Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<UpdateOrderResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);

            if (description == null)
            {
                return Response.Failure<UpdateOrderResponse>(OperationMessages.PermissionFailure);
            }

            description.UpdateOrder(request.Order);

            await _unit.SaveChangesAsync();

            return Response.SuccessWithMessage<UpdateOrderResponse>(OperationMessages.UpdatedOrderSuccessfully);
        }
    }
}
