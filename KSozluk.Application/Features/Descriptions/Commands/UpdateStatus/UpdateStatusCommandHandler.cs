using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.UpdateWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Descriptions.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : RequestHandlerBase<UpdateStatusCommand, UpdateStatusResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStatusCommandHandler(IUserService userService, IUserRepository userRepository, IDescriptionRepository descriptionRepository, IUnitOfWork unitOfWork, IWordRepository wordRepository = null)
        {
            _userService = userService;
            _userRepository = userRepository;
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
            _wordRepository = wordRepository;
        }

        public async override Task<UpdateStatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateStatusResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);
            var previousDescriptionId = description.PreviousDescriptionId;

            if (previousDescriptionId.HasValue && request.Status == ContentStatus.Onaylı) // eğer onaylanmışsa alt açıklamayı reddet
            {
                var previousDescription = await _descriptionRepository.FindAsync(previousDescriptionId);
                previousDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            var parentDescription = await _descriptionRepository.FindParentDescription(request.DescriptionId);

            if(parentDescription is not null && request.Status == ContentStatus.Onaylı)
            {
                parentDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            var word = await _wordRepository.FindAsync(description.WordId);

            if(word.Descriptions.Count == 1 || word.Descriptions.All(d => d.Status == ContentStatus.Bekliyor))
            {
                word.UpdateStatus(request.Status);
            }
            description.UpdateStatus(request.Status);
            description.UpdateAcceptor(userId);

            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithMessage<UpdateStatusResponse>(OperationMessages.UpdatedDescriptionStatusSuccessfully);
        }
    }
}
