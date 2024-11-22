using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.LikeWord
{
    public class LikeWordCommandHandler : RequestHandlerBase<LikeWordCommand, LikeWordResponse>

    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _wordLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDescriptionRepository _descriptionRepository;

        public LikeWordCommandHandler(IUserService userService, IUserRepository userRepository, ILikeRepository wordLikeRepository, IUnitOfWork unitOfWork, IDescriptionRepository descriptionRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordLikeRepository = wordLikeRepository;
            _unitOfWork = unitOfWork;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<LikeWordResponse> Handle(LikeWordCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<LikeWordResponse>(OperationMessages.PermissionFailure);
            }

            var existingLike = await _wordLikeRepository.GetByWordAndUserAsync(request.WordId, userId);

            if (existingLike != null)
            {
                _wordLikeRepository.DeleteWordLike(existingLike);
                await _unitOfWork.SaveChangesAsync();
                return Response.SuccessWithBody<LikeWordResponse>(request.WordId, OperationMessages.WordLikedSuccessfully);
            }

            var now = DateTime.UtcNow;

            var newWordLike = WordLike.Create(Guid.NewGuid(), request.WordId, userId, now);

            await _wordLikeRepository.CreateWordLike(newWordLike);
            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithBody<LikeWordResponse>(request.WordId, OperationMessages.WordLikedSuccessfully); ;
        }
    }
}
