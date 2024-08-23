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
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteDescriptionCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IUnitOfWork unitOfWork, IDescriptionRepository descriptionRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<DeleteDescriptionResponse> Handle(DeleteDescriptionCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<DeleteDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _descriptionRepository.FindByDescription(request.DescriptionId);
            var id = word.Id;
            word = await _wordRepository.FindAsync(id);
            var description = await _descriptionRepository.FindAsync(request.DescriptionId);

            if(word.Descriptions.Count == 1)
            {
                await _wordRepository.DeleteAsync(word.Id);
                await _unitOfWork.SaveChangesAsync();
                return Response.SuccessWithMessage<DeleteDescriptionResponse>(OperationMessages.WordAndDescriptionDeletedSuccessfully);
            }
            word.RemoveDescription(description);
            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithMessage<DeleteDescriptionResponse>(OperationMessages.DescriptionDeletedSuccessfully);
        }
    }
}
