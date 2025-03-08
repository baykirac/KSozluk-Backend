using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;


namespace KSozluk.Application.Features.Descriptions.Commands.DeleteDescriptions
{
    public class DeleteDescriptionCommandHandler : RequestHandlerBase<DeleteDescriptionCommand, DeleteDescriptionResponse>
    {

        private readonly IUnit _unit;
        private readonly IWordRepository _wordRepository;
        private readonly IDescriptionRepository _descriptionRepository;

        public DeleteDescriptionCommandHandler(IWordRepository wordRepository, IUnit unit, IDescriptionRepository descriptionRepository)
        {
            _wordRepository = wordRepository;
            _unit = unit;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<DeleteDescriptionResponse> Handle(DeleteDescriptionCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<DeleteDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _descriptionRepository.FindByDescription(request.DescriptionId);
            var id = word.Id;
            word = await _wordRepository.FindAsync(id);
            var description = await _descriptionRepository.FindAsync(request.DescriptionId);

            if (word.Descriptions.Count == 1)
            {
                await _wordRepository.DeleteAsync(word.Id);
                await _unit.SaveChangesAsync();
                return Response.SuccessWithMessage<DeleteDescriptionResponse>(OperationMessages.WordAndDescriptionDeletedSuccessfully);
            }
            word.RemoveDescription(description);
            await _unit.SaveChangesAsync();

            return Response.SuccessWithMessage<DeleteDescriptionResponse>(OperationMessages.DescriptionDeletedSuccessfully);
        }
    }
}
