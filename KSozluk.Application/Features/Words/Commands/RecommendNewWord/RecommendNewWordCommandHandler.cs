using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.RecommendNewWord
{
    public class RecommendNewWordCommandHandler : RequestHandlerBase<RecommendNewWordCommand, RecommendNewWordResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecommendNewWordCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<RecommendNewWordResponse> Handle(RecommendNewWordCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<RecommendNewWordResponse>(OperationMessages.PermissionFailure);
            }

            var now = DateTime.Now;

            var description = Description.Create(request.DescriptionContent, 0, null, userId, null);

            var existingWord = await _wordRepository.FindByContentAsync(request.WordContent);
            if (existingWord is null)
            {
                var word = Word.Create(request.WordContent, userId, now);
                word.AddDescription(description);
                await _wordRepository.CreateAsync(word);
                await _unitOfWork.SaveChangesAsync();
                return Response.SuccessWithBody<RecommendNewWordResponse>(word, OperationMessages.DescriptionRecommendedSuccessFully);
            }

            existingWord.AddDescription(description);
            await _unitOfWork.SaveChangesAsync();
            return Response.SuccessWithBody<RecommendNewWordResponse>(existingWord, OperationMessages.DescriptionRecommendedSuccessFully);



        }
    }
}
