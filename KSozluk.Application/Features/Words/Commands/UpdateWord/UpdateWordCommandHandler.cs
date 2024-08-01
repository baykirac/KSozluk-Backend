using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.UpdateWord
{
    public class UpdateWordCommandHandler : RequestHandlerBase<UpdateWordCommand, UpdateWordResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateWordCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<UpdateWordResponse> Handle(UpdateWordCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateWordResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _wordRepository.FindAsync(request.WordId);

            word.ChangeContent(request.WordContent);
            word.Descriptions.SingleOrDefault(d => d.Id == request.DescriptionId).UpdateContent(request.DescriptionContent);
            word.Descriptions.SingleOrDefault(d => d.Id == request.DescriptionId).UpdateRecommender(userId);

            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithBody<UpdateWordResponse>(word, OperationMessages.WordUpdatedSuccessfully);
        }
    }
}
