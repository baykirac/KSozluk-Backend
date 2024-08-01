using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KSozluk.Application.Features.Descriptions.Commands.RecommendNewDescription
{
    public class RecommendNewDescriptionCommandHandler : RequestHandlerBase<RecommendNewDescriptionCommand, RecommendNewDescriptionResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDescriptionRepository _descriptionRepository;

        public RecommendNewDescriptionCommandHandler(IUserRepository userRepository, IUserService userService, IWordRepository wordRepository, IUnitOfWork unitOfWork, IDescriptionRepository descriptionRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<RecommendNewDescriptionResponse> Handle(RecommendNewDescriptionCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId(); //öneride bulunan normal kullanıcı

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<RecommendNewDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var word = await _wordRepository.FindAsync(request.WordId);

            var greatestOrder = await _descriptionRepository.FindGreatestOrder(word.Id);
            var newOrder = greatestOrder + 1;

            var description = Description.Create(request.Content, newOrder, null, userId);

            word.AddDescription(description);

            await _unitOfWork.SaveChangesAsync();
            return Response.SuccessWithBody<RecommendNewDescriptionResponse>(word, OperationMessages.DescriptionRecommendedSuccessFully);
        }
    }
}
